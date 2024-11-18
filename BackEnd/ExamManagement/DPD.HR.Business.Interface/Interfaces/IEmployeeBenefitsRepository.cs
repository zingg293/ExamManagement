using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IEmployeeBenefitsRepository : IRepository<EmployeeBenefitsDto>
{
    Task<TemplateApi<EmployeeBenefitsDto>> UpdateEmployeeBenefits(List<EmployeeBenefitsDto> listIdBenefits,
        Guid idUserCurrent, string fullName);
}