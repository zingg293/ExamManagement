using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface ICategoryNewsRepository : IRepository<CategoryNewsDto>
{
    Task<TemplateApi<CategoryNewsDto>> UpdateShowChild(List<Guid> ids, bool isShow, Guid idUserCurrent,
        string fullName);

    Task<TemplateApi<CategoryNewsDto>> GetByIdParent(int pageNumber, int pageSize, Guid idParent);
}