namespace DPD.HumanResources.Entities.Entities;

public class OnLeave
{
    public Guid Id {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public string? Description {get;set;}
    public Guid IdUserRequest {get;set;}
    public Guid IdEmployee {get;set;}
    public Guid IdUnit {get;set;}
    public string? UnitName {get;set;}
    public string? Attachments {get;set;}
    public DateTime? FromDate {get;set;}
    public DateTime? ToDate {get;set;}
    public bool UnPaidLeave {get;set;}
}