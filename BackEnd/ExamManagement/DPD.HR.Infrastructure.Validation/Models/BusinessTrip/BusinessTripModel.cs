namespace DPD.HR.Infrastructure.Validation.Models.BusinessTrip;

public class BusinessTripModel
{
    public Guid? Id {get;set;}
    public string? Description {get;set;}
    public Guid IdUnit {get;set;}
    public string? UnitName {get;set;}
    public string StartDate {get;set;}
    public string EndDate {get;set;}    
    
    public string? Client {get;set;}
    public string? BusinessTripLocation {get;set;}
    public string? Vehicle {get;set;}
    public float? Expense {get;set;}
    public Guid? IdFile {get;set;}
}