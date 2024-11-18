namespace DPD.HumanResources.Entities.Entities;

public class TicketLaborEquipmentDetail
{
    public Guid Id { get; set; }
    public Guid IdTicketLaborEquipment { get; set; }
    public Guid IdCategoryLaborEquipment { get; set; }
    public double? Quantity { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? Status { get; set; }
    public Guid IdEmployee { get; set; }
    public string? EquipmentCode { get; set; }
    public bool IsCheck { get; set; }
}