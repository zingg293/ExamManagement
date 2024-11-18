using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IResignRepository: IRepository<ResignDto>
{
    Task<ResignDto> GetDataById(Guid idResign);
    Task<TemplateApi<ResignDto>> GetResignAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<ResignDto>> GetResignAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
}