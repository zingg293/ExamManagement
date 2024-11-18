using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;

namespace DPD.HR.Infrastructure.Validation.Models.User;

public class DataGetUser
{
    public UserDto? Data { get; set; }
    public IEnumerable<InformationRoleOfUser>? RoleList { get; set; }
    public bool IsAdmin { get; set; }
}