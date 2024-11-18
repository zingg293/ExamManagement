namespace DPD.HR.Infrastructure.Validation.Models.BusinessTripEmployee;

public class BusinessTripEmployeeModel
{
    public Guid Id {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public Guid IdBusinessTrip {get;set;}
    public Guid IdEmployee {get;set;}
    public bool? Captain {get;set;}
}