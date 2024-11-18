namespace DPD.HR.Infrastructure.Validation.Models.RequestToHired;

public class UpdateStepRequestToHiredMode
{
    public Guid IdWorkFlowInstance { get; set; }
    public bool IsTerminated { get; set; }
    public bool IsRequestToChange { get; set; }
    public string Message { get; set; } = String.Empty;
}