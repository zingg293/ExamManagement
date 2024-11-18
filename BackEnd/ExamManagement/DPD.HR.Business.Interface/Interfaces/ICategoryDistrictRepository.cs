using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface ICategoryDistrictRepository : IRepository<CategoryDistrictDto>
{
    Task<TemplateApi<CategoryDistrictDto>> GetByCityCode(string cityCode, int pageNumber, int pageSize);
}