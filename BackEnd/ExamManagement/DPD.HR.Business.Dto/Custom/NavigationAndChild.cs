using DPD.HumanResources.Entities.Entities;

namespace DPD.HumanResources.Dtos.Custom;

public class NavigationAndChild
{
    public Navigation? Navigation { get; set; }
    public List<NavigationChildOfChildren>? NavigationsChild { get; set; }
}

public class NavigationChildOfChildren
{
    public Guid Id { get; set; }
    public string MenuName { get; set; }
    public Guid? IdParent { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Path { get; set; }
    public string? IconLink { get; set; }
    public string? MenuCode { get; set; }
    public int? Sort { get; set; }
    public List<NavigationChildOfChildren>? NavigationsChildOfChild { get; set; }
}