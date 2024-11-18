namespace DPD.HumanResources.Dtos.Dto;

public class CategoryCompensationBenefitsDto
{
    public Guid Id {get;set;}
    public string? Name {get;set;}
    public int? Status {get;set;}
    public DateTime? CreatedDate {get;set;}
    public double? AmountMoney {get;set;}
}