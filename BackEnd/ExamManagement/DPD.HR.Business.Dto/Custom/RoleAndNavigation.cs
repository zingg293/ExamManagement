using DPD.HumanResources.Entities.Entities;

namespace DPD.HumanResources.Dtos.Custom;

public class RoleAndNavigation
{
    public Role? Role { get; set; }
    public List<Navigation>? Navigation { get; set; }
}