using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;

namespace DPD.HumanResources.Dtos.Custom;

public class UserAndRole
{
    public UserDto? User { get; set; }
    public List<Role>? Roles { get; set; }
}