namespace DPD.HumanResources.Dtos.Dto;

public class EmployeeAllowanceDto
{
    public Guid Id { get; set; }
    public Guid? IdAllowance { get; set; }
    public Guid? IdEmployee { get; set; }
    public DateTime? CreatedDate { get; set; }
}