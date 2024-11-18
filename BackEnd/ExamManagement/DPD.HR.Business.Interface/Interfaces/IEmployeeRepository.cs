using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IEmployeeRepository : IRepository<EmployeeDto>
{
    #region ===[ CRUD TABLE Employee ]=============================================================

    Task<TemplateApi<EmployeeAndBenefits>> GetEmployeeAndBenefits(Guid idEmployee);
    Task<TemplateApi<EmployeeAndAllowance>> GetEmployeeAndAllowance(Guid idEmployee);
    Task<TemplateApi<EmployeeDto>> GetEmployeeResigned(int pageNumber, int pageSize);
    Task<TemplateApi<EmployeeDto>> FilterEmployee(FilterEmployeeModel model,int pageNumber, int pageSize);

    Task<TemplateApi<EmployeeDto>> UpdateEmployeeType(Guid idEmployee, Guid typeOfEmployee,
        Guid idUserCurrent, string fullName);

    #endregion
}