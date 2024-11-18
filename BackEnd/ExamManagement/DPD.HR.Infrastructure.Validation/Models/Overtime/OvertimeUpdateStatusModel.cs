namespace DPD.HR.Infrastructure.Validation.Models.Overtime;

public class OvertimeUpdateStatusModel
{
    public Guid IdOvertime {get;set;}
    public int Status {get;set;}
    public string? HrNote {get;set;}
    public string? DirectorNote {get;set;}
}