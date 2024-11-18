using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IOnLeaveRepository : IRepository<OnLeaveDto>
{
    Task<OnLeaveDto> GetDataById(Guid id);
    Task<TemplateApi<OnLeaveDto>> GetOnLeaveAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);

    Task<TemplateApi<OnLeaveDto>> GetOnLeaveAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber,
        int pageSize);
    Task<TemplateApi<OnLeaveDto>> FilterOnLeave(FilterOnLeaveModel model, int pageNumber,
        int pageSize);
}