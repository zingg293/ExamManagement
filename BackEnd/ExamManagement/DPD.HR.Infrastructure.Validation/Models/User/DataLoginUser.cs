using DPD.HumanResources.Entities.Entities;

namespace DPD.HR.Infrastructure.Validation.Models.User;

public class DataLoginUser
{
    public Guid Id { get; set; }
    public string? Data { get; set; }
    public IEnumerable<InformationRoleOfUser>? RoleList { get; set; }
    public bool IsAdmin { get; set; }
}