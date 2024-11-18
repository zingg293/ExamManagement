using DPD.HumanResources.Entities.Entities;

namespace DPD.HumanResources.Dtos.Dto;

public class LaborEquipmentUnitAndRelevantInformation
{
    public Guid Id {get;set;}
    public int? Status {get;set;}
    public DateTime? CreatedDate {get;set;}
    public Guid IdEmployee {get;set;}
    public Guid IdTicketLaborEquipment {get;set;}
    public Guid IdCategoryLaborEquipment {get;set;}
    public Guid IdUnit {get;set;}
    public string EquipmentCode {get;set;}
    public int Type {get;set;}
    
    public TicketLaborEquipment? TicketLaborEquipment { get; set; }
    public CategoryLaborEquipment? CategoryLaborEquipment { get; set; }
}