using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface INewsRepository : IRepository<NewsDto>
{
    Task<TemplateApi<NewsDto>> ApproveNews(List<Guid> ids, bool isApprove, Guid idUserCurrent, string fullName);
    Task<TemplateApi<NewsDto>> UpdateNumberView(Guid idNews, Guid idUserCurrent, string fullName);
    Task<TemplateApi<NewsDto>> UpdateNumberLike(Guid idNews, Guid idUserCurrent, string fullName);
    Task<TemplateApi<NewsDto>> GetAllByIdCategoryNews(Guid idCategoryNews, int pageNumber, int pageSize);
    Task<TemplateApi<NewsDto>> SearchNews(string filter, int pageNumber, int pageSize);
    Task<TemplateApi<NewsDto>> FilterSearchNews(FilterSearchNewsModel model, int pageNumber, int pageSize);
}