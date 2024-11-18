namespace DPD.HR.Infrastructure.Validation.Models.TicketLaborEquipment;

public class TicketLaborEquipmentModel
{
    public Guid? Id {get;set;}
    public int? Type {get;set;}
    public string? Reason {get;set;}
    public string? Description {get;set;}
    
    public Guid? IdFile {get;set;}
}