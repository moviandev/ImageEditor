using FluentValidation;

namespace ImageEditor.Business.Models.Validations;
public class UserValidation : AbstractValidator<User>
{
    public UserValidation()
    {
        RuleFor(i => i.Name)
            .NotEmpty()
            .WithMessage("The field {PropertyName} needs to be filled.")
            .Length(4, 100)
            .WithMessage("The field {PropertyName} needs to be in between {MinLength} and {MaxLength} characters.");

        RuleFor(i => i.Email)
            .NotEmpty()
            .WithMessage("The field {PropertyName} needs to be filled.")
            .EmailAddress()
            .WithMessage("The field {PropertyName} should and Email")
            .Length(4, 1034)
            .WithMessage("The field {PropertyName} needs to be in between {MinLength} and {MaxLength} characters.");
    }
}
