
namespace DPD.HumanResources.Dtos.Dto;

public class BusinessTripEmployeeDto
{
    public Guid Id {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public Guid IdBusinessTrip {get;set;}
    public Guid IdEmployee {get;set;}
    public bool? Captain {get;set;}
}