using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IBusinessTripRepository : IRepository<BusinessTripDto>
{
    Task<TemplateApi<BusinessTripDto>> GetBusinessTripAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);

    Task<TemplateApi<BusinessTripDto>> GetBusinessTripAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber,
        int pageSize);

    Task<TemplateApi<BusinessTripDto>> GetListBusinessTrip(int pageNumber, int pageSize, Guid? idUnit,
        string? startDate, string? endDate);

    Task<BusinessTrip> GetDataById(Guid idBusinessTrip);
}