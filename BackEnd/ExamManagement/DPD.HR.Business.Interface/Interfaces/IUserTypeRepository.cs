using DPD.HumanResources.Dtos.Dto;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IUserTypeRepository : IRepository<UserTypeDto>
{
    #region CRUD TABLE USER_TYPE
    Task<UserTypeDto> GetTypeUser(string typeCode);
    Task<UserTypeDto> GetUserType(Guid id);
    #endregion
}