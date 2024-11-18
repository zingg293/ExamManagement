using DPD.HumanResources.Entities.Entities;

namespace DPD.HumanResources.Dtos.Dto;

public class OvertimeDto
{
    public Guid Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? Status { get; set; }
    public string? Description { get; set; }
    public Guid IdUserRequest { get; set; }
    public Guid IdEmployee { get; set; }
    public Guid IdUnit { get; set; }
    public string? UnitName { get; set; }
    public DateTime? FromDate {get;set;}
    public DateTime? ToDate {get;set;}
    
    public List<WorkflowInstances>? WorkflowInstances { get; set; }
    public int CountWorkFlowStep { get; set; }
    public WorkflowStep? CurrentWorkFlowStep { get; set; }

}