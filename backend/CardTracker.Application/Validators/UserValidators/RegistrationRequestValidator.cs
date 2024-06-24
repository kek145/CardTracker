using CardTracker.Domain.Requests.Registration;
using FluentValidation;

namespace CardTracker.Application.Validators.UserValidators;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull().WithMessage("Enter your FirstName")
            .NotEmpty().WithMessage("Enter your FirstName")
            .MaximumLength(100).WithMessage("No more than 100 characters")
            .Must(firstName => !firstName.Contains(' ')).WithMessage("The FirstName must not contain spaces");
        
        RuleFor(x => x.LastName)
            .NotNull().WithMessage("Enter your LastName")
            .NotEmpty().WithMessage("Enter your LastName")
            .MaximumLength(100).WithMessage("No more than 100 characters")
            .Must(firstName => !firstName.Contains(' ')).WithMessage("The LastName must not contain spaces");
        
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Enter your email")
            .NotEmpty().WithMessage("Enter your email")
            .MaximumLength(100).WithMessage("No more than 100 characters")
            .EmailAddress().WithMessage("You entered an incorrect email format")
            .Must(firstName => !firstName.Contains(' ')).WithMessage("The email must not contain spaces");
        
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("The password must not be empty.")
            .MinimumLength(6).WithMessage("The password must contain at least 6 characters.")
            .Matches("[A-Z]").WithMessage("The password must contain at least one capital letter.")
            .Matches("[a-z]").WithMessage("The password must contain at least one lowercase letter.")
            .Must(password => !password.Contains(' ')).WithMessage("The password must not contain spaces.")
            .Matches("[^a-zA-Z0-9]").WithMessage("The password must contain at least one special character.");
    }
}