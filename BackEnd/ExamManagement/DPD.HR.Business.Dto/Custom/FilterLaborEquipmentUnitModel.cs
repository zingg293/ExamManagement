namespace DPD.HumanResources.Dtos.Custom;

public class FilterLaborEquipmentUnitModel
{
    public Guid? IdEmployee {get;set;}
    public Guid? IdUnit {get;set;}
    public int PageNumber {get;set;}
    public int? Status {get;set;}
    public int PageSize {get;set;}
}