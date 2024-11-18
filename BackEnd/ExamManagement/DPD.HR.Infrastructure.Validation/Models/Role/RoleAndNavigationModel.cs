namespace DPD.HR.Infrastructure.Validation.Models.Role;

public class RoleAndNavigationModel
{
    public Guid? Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public int NumberRole { get; set; }
    public List<Guid>? listIdNavigation { get; set; }
}