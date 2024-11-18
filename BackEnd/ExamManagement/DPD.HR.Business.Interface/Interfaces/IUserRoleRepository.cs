using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IUserRoleRepository
{
    #region CURD TABLE USER_ROLE
    Task<TemplateApi<UserRoleDto>> InsertListUserRole(List<Guid> idRole, Guid idUser, Guid idUserCurrent, string fullName);

    #endregion
}