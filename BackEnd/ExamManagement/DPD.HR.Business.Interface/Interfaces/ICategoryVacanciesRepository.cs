using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface ICategoryVacanciesRepository : IRepository<CategoryVacanciesDto>
{
    Task<TemplateApi<CategoryVacanciesDto>> UpdateStatusVacancy(int status, Guid idCategoryVacancy, Guid idUserCurrent,
        string fullName);
    Task<TemplateApi<CategoryVacanciesDto>> GetVacancyApproved(int pageNumber, int pageSize);
}