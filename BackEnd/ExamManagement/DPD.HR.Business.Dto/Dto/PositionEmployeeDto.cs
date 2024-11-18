namespace DPD.HumanResources.Dtos.Dto;

public class PositionEmployeeDto
{
    public Guid Id {get;set;}
    public Guid IdEmployee {get;set;}
    public Guid IdPosition {get;set;}
    public bool IsHeadcount {get;set;}
    public Guid IdUnit {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
}