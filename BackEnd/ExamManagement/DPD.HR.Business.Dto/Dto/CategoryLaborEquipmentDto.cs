namespace DPD.HumanResources.Dtos.Dto;

public class CategoryLaborEquipmentDto
{
    public Guid Id {get;set;}
    public string? Name {get;set;}
    public string? Unit {get;set;}
    public string? Code {get;set;}
    public string? Description {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
}