using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IWorkflowStepRepository : IRepository<WorkflowStepDto>
{
    Task<TemplateApi<WorkflowStepDto>> GetAllByIdTemplate(int pageNumber, int pageSize, Guid idTemplate);

    Task<TemplateApi<WorkflowStepDto>> CUD_WorkflowStep(List<WorkflowStepDto> workflowStepDto, Guid idUserCurrent,
        string fullName);
}