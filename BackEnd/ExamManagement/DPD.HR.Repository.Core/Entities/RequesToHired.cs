namespace DPD.HumanResources.Entities.Entities;

public class RequestToHired
{
    public Guid Id {get;set;}
    public int? Status {get;set;}
    public DateTime? CreatedDate {get;set;}
    public string? Reason {get;set;}
    public int? Quantity {get;set;}
    public string? FilePath {get;set;}
    public Guid? IdCategoryVacancies {get;set;}
    public Guid CreatedBy {get;set;}
    public Guid IdUnit {get;set;}
}