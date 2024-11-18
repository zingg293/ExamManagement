namespace DPD.HR.Infrastructure.Validation.Models.RequestToHired;

public class RequestToHiredModel
{
    public Guid? Id {get;set;}
    public string? Reason {get;set;}
    public int? Quantity {get;set;}
    public Guid? IdCategoryVacancies {get;set;}
    public Guid? IdUnit {get;set;}
    public Guid? IdFile {get;set;}
}