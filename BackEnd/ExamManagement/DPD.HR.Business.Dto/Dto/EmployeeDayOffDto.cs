namespace DPD.HumanResources.Dtos.Dto;

public class EmployeeDayOffDto
{
    public Guid Id {get;set;}
    public Guid? IdEmployee {get;set;}
    public DateTime? DayOff {get;set;}
    public int? TypeOfDayOff {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public int? OnLeave {get;set;}
}