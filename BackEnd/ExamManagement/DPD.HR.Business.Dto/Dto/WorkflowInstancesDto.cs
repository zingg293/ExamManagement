namespace DPD.HumanResources.Dtos.Dto;

public class WorkflowInstancesDto
{
    public Guid Id {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public string WorkflowCode {get;set;}
    public string? WorkflowName {get;set;}
    public bool IsCompleted {get;set;}
    public bool IsTerminated {get;set;}
    public bool IsDraft {get;set;}
    public string? NameStatus {get;set;}
    public Guid TemplateId {get;set;}
    public int CurrentStep {get;set;}
    public Guid UnitId {get;set;}
    public Guid ItemId {get;set;}
    public Guid CreatedBy {get;set;}
    public bool? RequestToChange {get;set;}
    public string? Message {get;set;}
    public int? NumberForCode {get;set;}
    public bool IsApproved {get;set;}
}