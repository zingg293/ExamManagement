using DPD.HR.Infrastructure.Validation.Models.User;
using FluentValidation;

namespace DPD.HR.Infrastructure.Validation.Validation.User;

public class UserLoginRequestValidator: AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        // Check is not null, empty and is between 1 and 250 characters
        RuleFor(customer => customer.Email).NotNull().NotEmpty().Length(1, 250).EmailAddress();
        RuleFor(customer => customer.Password).NotNull().NotEmpty().Length(1, 250);
    }
}