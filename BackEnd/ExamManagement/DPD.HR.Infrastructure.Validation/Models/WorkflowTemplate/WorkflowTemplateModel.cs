namespace DPD.HR.Infrastructure.Validation.Models.WorkflowTemplate;

public class WorkflowTemplateModel
{
    public Guid? Id {get;set;}
    public string? WorkflowName {get;set;}
    public string WorkflowCode {get;set;}
    public int Order {get;set;}
    public string? StartWorkflowButton {get;set;}
    public string? DefaultCompletedStatus {get;set;}
}