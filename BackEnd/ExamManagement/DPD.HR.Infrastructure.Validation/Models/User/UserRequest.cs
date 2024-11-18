using System.ComponentModel.DataAnnotations;

namespace DPD.HR.Infrastructure.Validation.Models.User;

public class UserRequest
{
    public Guid? Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
    public string? Fullname { get; set; }
    public string? Description { get; set; }
    public string? Phone { get; set; }
    public Guid UserTypeId { get; set; }
    public string? Address { get; set; }
    public int? Status { get; set; }
    public Guid UnitId { get; set; }
    public bool IsActive { get; set; }
}