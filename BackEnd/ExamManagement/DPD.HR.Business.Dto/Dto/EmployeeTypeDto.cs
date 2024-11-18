namespace DPD.HumanResources.Dtos.Dto;

public class EmployeeTypeDto
{
    public Guid Id { get; set; }
    public string? TypeName { get; set; }
    public bool? IsActive { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
}