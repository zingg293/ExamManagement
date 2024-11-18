using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IRequestToHiredRepository : IRepository<RequestToHiredDto>
{
    Task<RequestToHired> GetDataById(Guid idRequestToHire);
    Task<TemplateApi<RequestToHiredDto>> GetRequestToHireAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<RequestToHiredDto>> GetRequestToHireAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);

    Task<TemplateApi<RequestToHiredDto>> GetListRequestToHired(int pageNumber, int pageSize, Guid? idUnit,
        Guid? idCategoryVacancies);
}