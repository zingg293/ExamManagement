using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IEmployeeAllowanceRepository : IRepository<EmployeeAllowanceDto>
{
    Task<TemplateApi<EmployeeAllowanceDto>> GetByIdEmployee(Guid employeeId, int pageNumber,
        int pageSize);

    Task<TemplateApi<EmployeeAllowanceDto>> InsertEmployeeAndAllowance(Guid employeeId, List<Guid> idAllowance,
        Guid idUserCurrent,
        string fullName);
}