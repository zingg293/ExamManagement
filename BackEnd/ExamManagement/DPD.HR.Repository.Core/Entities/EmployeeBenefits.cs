namespace DPD.HumanResources.Entities.Entities;

public class EmployeeBenefits
{
    public Guid Id {get;set;}
    public int? Status {get;set;}
    public DateTime? CreatedDate {get;set;}
    public Guid IdEmployee {get;set;}
    public Guid IdCategoryCompensationBenefits {get;set;}
    public double? Quantity {get;set;}
}