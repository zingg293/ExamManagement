namespace DPD.HR.Infrastructure.Validation.Models.EmployeeDayOff;

public class EmployeeDayOffModel
{
    public Guid? Id {get;set;}
    public Guid? IdEmployee {get;set;}
    public DateTime? DayOff {get;set;}
    public int? TypeOfDayOff {get;set;}
    public int? OnLeave {get;set;}
}