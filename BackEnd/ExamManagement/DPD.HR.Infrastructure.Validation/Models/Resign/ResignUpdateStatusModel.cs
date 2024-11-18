namespace DPD.HR.Infrastructure.Validation.Models.Resign;

public class ResignUpdateStatusModel
{
    public Guid IdResign {get;set;}
    public int Status {get;set;}
    public string? HrNote {get;set;}
    public string? DirectorNote {get;set;}
}