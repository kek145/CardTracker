using CardTracker.Domain.Requests.Account;
using FluentValidation;

namespace CardTracker.Application.Validators.UserValidators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.ResetToken)
            .NotNull().WithMessage("Enter your token")
            .NotEmpty().WithMessage("Enter your token")
            .Must(token => !token.Contains(' ')).WithMessage("Reset token must not contain spaces");
        
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("The password must not be empty.")
            .MinimumLength(6).WithMessage("The password must contain at least 6 characters.")
            .Matches("[A-Z]").WithMessage("The password must contain at least one capital letter.")
            .Matches("[a-z]").WithMessage("The password must contain at least one lowercase letter.")
            .Must(password => !password.Contains(' ')).WithMessage("The password must not contain spaces.")
            .Matches("[^a-zA-Z0-9]").WithMessage("The password must contain at least one special character.");
    }
}