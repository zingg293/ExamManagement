using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces
{
    public interface IUserRepository : IRepository<UserDto>
    {
        #region ===[ CRUD TABLE USER ]=============================================================

        Task<TemplateApi<UserAndRole>> GetUserAndRole(Guid idUser);

        Task<UserDto> UserByIdInternal(Guid id);

        Task<TemplateApi<UserDto>> InsertUser(UserDto newUser, Guid idUserCurrent, string fullName, string password,
            string salt);

        #endregion

        #region ===[ LOGIN AND REGISTER ACCOUNT ]=============================================================

        Task<User> UserByEmail(string email);
        Task<TemplateApi<UserDto>> ActiveUserByCode(string email, string code);
        Task<TemplateApi<UserDto>> UpdateActiveCode(string code, string email);
        Task<TemplateApi<UserDto>> UpdatePassword(string email, string newPassword, string salt);

        #endregion
    }
}