using DPD.HR.Infrastructure.Validation.Models.User;
using FluentValidation;
using FluentValidation.Results;

namespace DPD.HR.Infrastructure.Validation;
public class ValidatorProvider
{
    private readonly IValidator<UserRegisterRequest> _registerRequestValidator;
    private readonly IValidator<UserLoginRequest> _loginRequestValidator;

    public ValidatorProvider(
        IValidator<UserRegisterRequest> registerRequestValidator,
        IValidator<UserLoginRequest> loginRequestValidator)
    {
        _registerRequestValidator = registerRequestValidator;
        _loginRequestValidator = loginRequestValidator;
    }

    public ValidationResult Validate(UserRegisterRequest request)
    {
        return _registerRequestValidator.Validate(request);
    }

    public ValidationResult Validate(UserLoginRequest request)
    {
        return _loginRequestValidator.Validate(request);
    }
}