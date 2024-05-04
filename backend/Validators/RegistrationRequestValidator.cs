using FluentValidation;
using Task4.Dto.Requests;

namespace Task4.Validators;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Password).NotEmpty();
        RuleFor(r => r.Name).NotEmpty();
    }
}