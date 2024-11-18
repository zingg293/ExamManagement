namespace DPD.HR.Infrastructure.Validation.Models.TicketLaborEquipmentDetail;

public class TicketLaborEquipmentDetailModel
{
    public Guid Id {get;set;}
    public Guid IdTicketLaborEquipment {get;set;}
    public Guid IdCategoryLaborEquipment {get;set;}
    public double? Quantity {get;set;}
    public Guid IdEmployee {get;set;}
    public string? EquipmentCode {get;set;}
}