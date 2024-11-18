namespace DPD.HumanResources.Entities.Entities;

public class BusinessTripEmployee
{
    public Guid Id {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public Guid IdBusinessTrip {get;set;}
    public Guid IdEmployee {get;set;}
    public bool? Captain {get;set;}
}