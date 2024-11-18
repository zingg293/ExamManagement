namespace DPD.HumanResources.Entities.Entities;

public class TicketLaborEquipment
{
    public Guid Id {get;set;}
    public Guid IdUserRequest {get;set;}
    public int? Type {get;set;}
    public string? Reason {get;set;}
    public string? FileAttachment {get;set;}
    public string? Description {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public Guid IdUnit {get;set;}
}