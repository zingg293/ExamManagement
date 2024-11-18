namespace DPD.HR.Infrastructure.Validation.Models.OnLeave;

public class OnLeaveModel
{
    public Guid? Id { get; set; }
    public string? Description { get; set; }
    public Guid IdEmployee { get; set; }
    public Guid IdUnit { get; set; }
    public string? UnitName { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public bool UnPaidLeave { get; set; }


    public Guid? IdFile { get; set; }
}