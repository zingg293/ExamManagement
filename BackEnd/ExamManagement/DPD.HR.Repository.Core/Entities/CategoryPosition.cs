namespace DPD.HumanResources.Entities.Entities;

public class CategoryPosition
{
    public Guid Id {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public string? PositionName {get;set;}
    public string? Description {get;set;}
    public bool? IsActive {get;set;}
}