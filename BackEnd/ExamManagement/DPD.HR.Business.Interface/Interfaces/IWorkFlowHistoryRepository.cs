using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IWorkFlowHistoryRepository
{
    Task<TemplateApi<WorkflowHistoryDto>> GetWorkFlowHistoryByIdInstance(Guid idWorkFLowInstance, int pageNumber,
        int pageSize);
}