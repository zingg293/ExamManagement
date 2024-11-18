namespace DPD.HumanResources.Dtos.Dto;

public class CategoryNewsDto
{
    public Guid Id {get;set;}
    public string? NameCategory {get;set;}
    public int? CategoryGroup {get;set;}
    public Guid ParentId {get;set;}
    public bool? IsHide {get;set;}
    public bool? IsDeleted {get;set;}
    public Guid UserCreated {get;set;}
    public bool? ShowChild {get;set;}
    public int? Sort {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public string? Avatar {get;set;}
    public Guid? IdFile {get;set;}

}