using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IOvertimeRepository: IRepository<OvertimeDto>
{
    Task<TemplateApi<OvertimeDto>> GetOvertimeAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<OvertimeDto>> GetOvertimeDAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<OvertimeDto>> FilterOverTime(FilterOverTimeModel model, int pageNumber, int pageSize);
}