using ImageEditor.Api.Dtos;
using ImageEditor.Business.EnumTypes;
using ImageEditor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon;
using ImageEditor.Business.Models;
using Microsoft.AspNetCore.Authorization;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageEditor.Api.Controllers.V1;
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
public class ImageController : BaseController
{
    private readonly IUserRepository _userRepository;
    private readonly IImageRepository _imageRepository;
    private readonly ILogger _logger;

    public ImageController(INotifier notifier,
        IIdentityUser identityUser,
        IUserRepository userRepository,
        ILogger<ImageController> logger,
        IImageRepository imageRepository) : base(notifier, identityUser)
    {
        _userRepository = userRepository;
        _logger = logger;
        _imageRepository = imageRepository;
    }

    [HttpPost("ApplyEffect")]
    public async Task<ActionResult<Guid>> AddEffectAsync([FromForm] ImageDto file)
    {
        if (!ModelState.IsValid) CustomResponse(ModelState);
        var email = IdentityUser.GetUserEmail();
        var userId = Guid.Empty;
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user is null)
            {
                user = Business.Models.User.Builder.Create(email);
                userId = user.Id;
                await _userRepository.AddAsync(user);
            }
            else
                userId = user.Id;

            var (originUri, editedUri) = await ApplyEffectAsync(file.Image, file.Effect);
            var newImage = Image.Builder.Create(originUri, editedUri, user.Id);
            await _imageRepository.AddAsync(newImage);
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{GetType().Name}]: Error to apply effects {ex.Message}");
            NotifyError("There was an error, please try again");
            return CustomResponse();
        }

        return CustomResponse(userId);
    }

    [HttpGet("GetAllImages")]
    public async Task<ActionResult<IEnumerable<ImageResponseDto>>> GetAllImagesAsync(Guid userId)
    {
        if (Guid.Empty == userId)
        {
            NotifyError($"Invalid userId");
            CustomResponse();
        }

        try
        {
            var images = await _userRepository.GetAllImagesAsync(userId);

            if (images is null || !images.Any())
            {
                NotifyError("User doesn't exists");
                CustomResponse();
            }

            return CustomResponse(images.Select(i => new ImageResponseDto
            {
                OriginalS3Uri = i.S3OriginalImage,
                EditedS3Uri = i.S3EditedImage
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{GetType().Name}]: Error to apply effects {ex.Message}");
            NotifyError("There was an error, please try again");
            return CustomResponse();
        }
    }

    private async Task<(string OriginalUri, string EditedUri)> ApplyEffectAsync(IFormFile image, EffectType effect)
    {
        if (image == null || image.Length <= 0)
        {
            NotifyError("Provide a Image");
            return (null, null);
        }

        var imgName = image.FileName;
        var imgNameToSave = $"{imgName.Split('.')[0]}-{Guid.NewGuid()}";
        var imgExtension = Path.GetExtension(imgName);

        // Create memory stream and copy the image to it
        using var originalImgStream = new MemoryStream();
        await image.CopyToAsync(originalImgStream);

        // Upload originalImage to s3
        var originalUri = await UploadImageToS3Async(originalImgStream, imgNameToSave + imgExtension);

        if (string.IsNullOrWhiteSpace(originalUri))
        {
            NotifyError("Error while uploading your image, try again");
            return (null, null);
        }

        // Reassigning img stream
        using var newImgStream = new MemoryStream();
        await image.CopyToAsync(newImgStream);
        newImgStream.Seek(0, SeekOrigin.Begin);

        using var sixLabors = SixLabors.ImageSharp.Image.Load<Rgba32>(newImgStream);

        if (effect.IsBlur())
            sixLabors.Mutate(s => s.GaussianBlur(10));

        if (effect.IsSepiaEffect())
            sixLabors.Mutate(s => s.Sepia());

        if (effect.IsGrayEffect())
            sixLabors.Mutate(s => s.Grayscale());

        if (effect.IsVignette())
            sixLabors.Mutate(s => s.Vignette());

        using var outputImg = new MemoryStream();

        outputImg.Position = 0;
        await sixLabors.SaveAsync(outputImg, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
        outputImg.Seek(0, SeekOrigin.Begin);

        // Upload edited Image
        var editedImgUri = await UploadImageToS3Async(outputImg, imgNameToSave + "-edited" + imgExtension);

        if (string.IsNullOrWhiteSpace(editedImgUri))
        {
            NotifyError("Error while uploading your edited image, try again");
            return (null, null);
        }

        return (originalUri, editedImgUri);
    }

    private async Task<string> UploadImageToS3Async(Stream image, string imageName)
    {
        var uri = string.Empty;
        try
        {
            var transferUtility = new TransferUtility(new AmazonS3Client(region: RegionEndpoint.USEast1));
            await transferUtility.UploadAsync(image, "image-editor", imageName);
            uri = $"https://image-editor.s3.amazonaws.com/{imageName}";
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{GetType().Name}]: Error uploading into AWS {ex.Message}");
            NotifyError("Error on uploading image, try again");
            return uri;
        }

        return uri;
    }
}