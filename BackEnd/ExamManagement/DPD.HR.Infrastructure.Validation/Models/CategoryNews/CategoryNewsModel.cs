namespace DPD.HR.Infrastructure.Validation.Models.CategoryNews;

public class CategoryNewsModel
{
    public Guid? Id {get;set;}
    public string? NameCategory {get;set;}
    public int? CategoryGroup {get;set;}
    public Guid? ParentId {get;set;}
    public int? Sort {get;set;}
    public Guid? IdFile {get;set;}

}