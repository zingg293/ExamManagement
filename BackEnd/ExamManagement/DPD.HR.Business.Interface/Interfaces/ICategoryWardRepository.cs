using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface ICategoryWardRepository : IRepository<CategoryWardDto>
{
    Task<TemplateApi<CategoryWardDto>> GetCategoryWardByDistrictCode(string districtCode, int pageNumber, int pageSize);
}