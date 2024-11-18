namespace DPD.HumanResources.Entities.Entities;

public class EmployeeType
{
    public Guid Id { get; set; }
    public string? TypeName { get; set; }
    public bool? IsActive { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
}