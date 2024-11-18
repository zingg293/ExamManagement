namespace DPD.HumanResources.Dtos.Dto;

public class WorkflowStepDto
{
    public Guid Id {get;set;}
    public Guid TemplateId {get;set;}
    public string? StepName {get;set;}
    public Guid IdRoleAssign {get;set;}
    public Guid IdUnitAssign {get;set;}
    public bool AllowTerminated {get;set;}
    public string? RejectName {get;set;}
    public int Order {get;set;}
    public string? OutCome {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public string? ProcessWorkflowButton {get;set;}
    public string? StatusColor {get;set;}
    public bool IsDirectUnit {get;set;}
}