namespace DPD.HumanResources.Entities.Entities;

public class BusinessTrip
{
    public Guid Id {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public string? Description {get;set;}
    public Guid IdUserRequest {get;set;}
    public Guid IdUnit {get;set;}
    public string? UnitName {get;set;}
    public DateTime StartDate {get;set;}
    public DateTime EndDate {get;set;}
    
    public string? Client {get;set;}
    public string? BusinessTripLocation {get;set;}
    public string? Vehicle {get;set;}
    public float? Expense {get;set;}
    public string? Attachments {get;set;}
}