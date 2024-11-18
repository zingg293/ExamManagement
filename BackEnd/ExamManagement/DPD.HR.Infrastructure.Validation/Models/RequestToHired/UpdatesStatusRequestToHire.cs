namespace DPD.HR.Infrastructure.Validation.Models.RequestToHired;

public class UpdatesStatusRequestToHire
{
    public Guid IdRequestToHire {get;set;}
    public int Status {get;set;}
    public string? HrNote {get;set;}
    public string? DirectorNote {get;set;}
}