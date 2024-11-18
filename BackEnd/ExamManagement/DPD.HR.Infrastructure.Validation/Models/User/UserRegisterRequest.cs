namespace DPD.HR.Infrastructure.Validation.Models.User;

public class UserRegisterRequest
{
    public string Email { get; } = string.Empty;
    public string? Fullname { get; } = string.Empty;
    public string Password { get; } = string.Empty;
    public string? Phone { get; } = string.Empty;
    public string? Address { get; } = string.Empty;
}