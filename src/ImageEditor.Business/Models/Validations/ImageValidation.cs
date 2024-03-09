using FluentValidation;

namespace ImageEditor.Business.Models.Validations;
public class ImageValidation : AbstractValidator<Image>
{
    public ImageValidation()
    {
        RuleFor(i => i.S3EditedImage)
            .NotEmpty()
            .WithMessage("The field {PropertyName} needs to be filled.")
            .Length(4, 1034)
            .WithMessage("The field {PropertyName} needs to be in between {MinLength} and {MaxLength} characters.");

        RuleFor(i => i.S3OriginalImage)
            .NotEmpty()
            .WithMessage("The field {PropertyName} needs to be filled.")
            .Length(4, 1034)
            .WithMessage("The field {PropertyName} needs to be in between {MinLength} and {MaxLength} characters.");

        RuleFor(i => i.UserId)
            .NotEmpty()
            .WithMessage("The field {PropertyName} needs to be filled.");
    }
}
