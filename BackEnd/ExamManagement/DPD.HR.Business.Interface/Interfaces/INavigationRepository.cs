using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface INavigationRepository: IRepository<NavigationDto>
{
    Task<TemplateApi<NavigationAndChild>> GetNavigationByIdUser(Guid idUser, int pageNumber, int pageSize);
}