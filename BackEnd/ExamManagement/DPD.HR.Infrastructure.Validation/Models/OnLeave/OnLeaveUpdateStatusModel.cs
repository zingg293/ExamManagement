namespace DPD.HR.Infrastructure.Validation.Models.OnLeave;

public class OnLeaveUpdateStatusModel
{
    public Guid IdOnLeave {get;set;}
    public int Status {get;set;}
    public string? HrNote {get;set;}
    public string? DirectorNote {get;set;}
}