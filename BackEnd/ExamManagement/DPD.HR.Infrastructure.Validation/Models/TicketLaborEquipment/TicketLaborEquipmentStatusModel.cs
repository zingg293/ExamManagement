namespace DPD.HR.Infrastructure.Validation.Models.TicketLaborEquipment;

public class TicketLaborEquipmentStatusModel
{
    public Guid Id {get;set;}
    public int Status {get;set;}
    public string? HrNote {get;set;}
    public string? DirectorNote {get;set;}

}