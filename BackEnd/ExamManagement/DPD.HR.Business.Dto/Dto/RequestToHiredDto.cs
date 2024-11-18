using DPD.HumanResources.Entities.Entities;

namespace DPD.HumanResources.Dtos.Dto;

public class RequestToHiredDto
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Reason { get; set; }
    public int? Quantity { get; set; }
    public string? FilePath { get; set; }
    public Guid? IdCategoryVacancies { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid IdUnit { get; set; }
    public Guid? IdFile { get; set; }

    public CategoryVacancies? CategoryVacancies { get; set; }
    public List<WorkflowInstances>? WorkflowInstances { get; set; }
    public int CountWorkFlowStep { get; set; }
    public WorkflowStep? CurrentWorkFlowStep { get; set; }

}