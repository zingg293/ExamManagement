using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IWorkFlowRepository
{
    Task<TemplateApi<WorkflowTemplateDto>> UpdateStepWorkFlow(Guid idWorkFlowInstance, bool isTerminated,
        bool isRequestToChange, string message, Guid idUserCurrent,
        string fullName);

    Task<TemplateApi<WorkflowTemplateDto>> InsertStepWorkFlow(string codeWorkFlow, Guid itemId, Guid idUserCurrent,
        string fullName);
}