using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IInternRequestRepository : IRepository<InternRequestDto>
{
    Task<TemplateApi<InternRequestDto>> GetInternRequestAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<InternRequestDto>> GetInternRequestAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<InternRequestDto>> FilterInternRequest(FilterInternRequestModel model, int pageNumber, int pageSize);
    Task<InternRequest> GetDataById(Guid idInterRequest);
}