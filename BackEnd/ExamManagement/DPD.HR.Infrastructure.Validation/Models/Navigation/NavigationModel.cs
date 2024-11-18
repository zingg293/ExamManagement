namespace DPD.HR.Infrastructure.Validation.Models.Navigation;

public class NavigationModel
{
    public Guid? Id {get;set;}
    public string MenuName {get;set;}
    public Guid? IdParent {get;set;}
    public string? Path {get;set;}
    public string? IconLink {get;set;}
    public string? MenuCode {get;set;}
    public int? Sort {get;set;}
}