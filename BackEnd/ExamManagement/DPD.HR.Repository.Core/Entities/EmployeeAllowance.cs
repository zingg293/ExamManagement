namespace DPD.HumanResources.Entities.Entities;

public class EmployeeAllowance
{
    public Guid Id { get; set; }
    public Guid? IdAllowance { get; set; }
    public Guid? IdEmployee { get; set; }
    public DateTime? CreatedDate { get; set; }
}