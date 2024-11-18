namespace DPD.HR.Infrastructure.Validation.Models.EmployeeAllowance;

public class EmployeeAllowanceModel
{
    public Guid Employee { get; set; }
    public List<Guid>? IdEmployeeAllowance { get; set; }
}