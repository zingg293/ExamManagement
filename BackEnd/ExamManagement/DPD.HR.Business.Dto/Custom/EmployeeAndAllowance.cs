using DPD.HumanResources.Entities.Entities;

namespace DPD.HumanResources.Dtos.Custom;

public class EmployeeAndAllowance
{
    public Employee? Employee { get; set; }
    public List<Allowance>? Allowances { get; set; }
}