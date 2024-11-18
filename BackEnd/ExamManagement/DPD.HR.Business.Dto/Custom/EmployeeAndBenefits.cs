using DPD.HumanResources.Entities.Entities;

namespace DPD.HumanResources.Dtos.Custom;

public class EmployeeAndBenefits
{
    public Employee? Employee { get; set; }
    public List<EmployeeBenefits>? EmployeeBenefits { get; set; }
}