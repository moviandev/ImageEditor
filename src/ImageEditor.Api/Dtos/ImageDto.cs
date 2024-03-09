using System.ComponentModel.DataAnnotations;
using ImageEditor.Api.Extensions;
using ImageEditor.Business.EnumTypes;

namespace ImageEditor.Api.Dtos;
public class ImageDto
{
    // [Required]
    // [AllowedExtensions(new[] { ".jpg", ".png" })]
    public IFormFile Image { get; set; }

    [Required]
    public EffectType Effect { get; set; }
}
