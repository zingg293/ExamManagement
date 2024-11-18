namespace DPD.HumanResources.Dtos.Dto;

public class WorkflowHistoryDto
{
    public Guid Id {get;set;}
    public Guid IdWorkFlowInstance {get;set;}
    public Guid IdUser {get;set;}
    public string? Action {get;set;}
    public Guid IdUnit {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public bool IsStepCompleted {get;set;}
    public bool IsCancelled {get;set;}
    public bool IsRequestToChanged {get;set;}
    public string? Comment {get;set;}
    public string? Message {get;set;}
}