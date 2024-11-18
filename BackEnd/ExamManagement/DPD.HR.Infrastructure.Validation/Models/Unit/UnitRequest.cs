namespace DPD.HR.Infrastructure.Validation.Models.Unit;

public class UnitRequest
{
    public Guid? Id { get; set; }
    public string UnitName { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public string? UnitCode { get; set; }
}