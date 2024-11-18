namespace DPD.HR.Infrastructure.Validation.Models.EmployeeBenefits;

public class EmployeeBenefitsModel
{
    public Guid? Id {get;set;}
    public Guid IdEmployee {get;set;}
    public Guid IdCategoryCompensationBenefits {get;set;}
    public double? Quantity {get;set;}
}