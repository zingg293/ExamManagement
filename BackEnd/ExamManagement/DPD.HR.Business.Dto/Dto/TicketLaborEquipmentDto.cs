using DPD.HumanResources.Entities.Entities;

namespace DPD.HumanResources.Dtos.Dto;

public class TicketLaborEquipmentDto
{
    public Guid Id { get; set; }
    public Guid IdUserRequest { get; set; }
    public int? Type { get; set; }
    public string? Reason { get; set; }
    public string? FileAttachment { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? Status { get; set; }
    public Guid IdUnit {get;set;}
    public Guid? IdFile {get;set;}


    public List<TicketLaborEquipmentDetailDto>? TicketLaborEquipmentDetail { get; set; }
    public List<WorkflowInstances>? WorkflowInstances { get; set; }
    public int CountWorkFlowStep { get; set; }
    public WorkflowStep? CurrentWorkFlowStep { get; set; }
}