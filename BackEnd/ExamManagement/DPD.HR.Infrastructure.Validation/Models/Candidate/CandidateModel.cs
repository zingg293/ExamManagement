namespace DPD.HR.Infrastructure.Validation.Models.Candidate;

public class CandidateModel
{
    public Guid? Id {get;set;}
    public string? Name {get;set;}
    public bool? Sex {get;set;}
    public DateTime? Birthday {get;set;}
    public string? Phone {get;set;}
    public string? Email {get;set;}
    public string? Address {get;set;}
    public Guid IdCity {get;set;}
    public Guid IdDistrict {get;set;}
    public Guid IdWard {get;set;}
    public string? Note {get;set;}
}