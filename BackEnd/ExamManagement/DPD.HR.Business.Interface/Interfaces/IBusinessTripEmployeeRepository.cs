using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IBusinessTripEmployeeRepository : IRepository<BusinessTripEmployeeDto>
{
    Task<TemplateApi<BusinessTripEmployeeDto>> InsertBusinessTripEmployeeByList(
        List<BusinessTripEmployeeDto> businessTripEmployee, Guid idUserCurrent, string fullName);

    Task<TemplateApi<BusinessTripEmployeeDto>> GetListByIBusinessTrip(
        Guid idBusinessTrip, int pageNumber,
        int pageSize);
}