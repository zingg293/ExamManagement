namespace DPD.HumanResources.Dtos.Custom;

public class FilterSearchNewsModel
{
    public string? Title {get;set;}
    public Guid? IdCategoryNews {get;set;}
    public string? CreatedDateDisplay {get;set;}
    public bool? IsHide {get;set;}
    public bool? IsApproved {get;set;}
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
    
}