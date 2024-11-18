using System.Data;
using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DPD.HR.Application.Implement.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public UserRoleRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ UserRoleRepository ]=================================================================

    public async Task<TemplateApi<UserRoleDto>> InsertListUserRole(List<Guid> idRole, Guid idUser, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var getListRoleByUser =
                await connection.QueryAsync<UserRole>(UserRoleSqlQueries.QueryGetAllUserRoleByIdUser,
                    new { IdUser = idUser }, tran);

            if (getListRoleByUser.Any())
            {
                await connection.ExecuteAsync(UserRoleSqlQueries.QueryDeleteUserRoleByIdUser, new { IdUser = idUser },
                    tran);
            }

            var userRoles = idRole.Select(roleId => new UserRole
            {
                Id = Guid.NewGuid(),
                IdUser = idUser,
                IdRole = roleId
            }).ToList();

            await connection.ExecuteAsync(UserRoleSqlQueries.InsertUserRole, userRoles, tran);

            var diaries = idRole.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng UserRole",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "UserRole",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<UserRoleDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    #endregion
}