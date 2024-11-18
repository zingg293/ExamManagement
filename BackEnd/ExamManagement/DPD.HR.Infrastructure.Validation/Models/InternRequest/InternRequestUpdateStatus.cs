namespace DPD.HR.Infrastructure.Validation.Models.InternRequest;

public class InternRequestUpdateStatus
{
    public Guid IdInternRequest {get;set;}
    public int Status {get;set;}
    public string? HrNote {get;set;}
    public string? DirectorNote {get;set;}
}