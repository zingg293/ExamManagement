using System.Data;
using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DPD.HR.Application.Implement.Repositories;

public class RoleRepository : IRoleRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public RoleRepository(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    #endregion

    #region ===[ IRoleRepository Methods ]=====================================================

    public async Task<RoleDto> GetUserRole(string roleType)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var result = await connection.QueryAsync<Role>(RoleSqlQueries.GetByRoleCode);
        return result.Adapt<RoleDto>();
    }

    public async Task<RoleDto> GetUserRoleById(Guid idRole)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var result = await connection.QuerySingleOrDefaultAsync<Role>(RoleSqlQueries.GetByIdRole, new { Id = idRole });
        return result.Adapt<RoleDto>();
    }

    public async Task<List<RoleDto>> GetAllRole()
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var roles = await connection.QueryAsync<Role>(RoleSqlQueries.GetAllRole);

        return roles.Adapt<List<RoleDto>>();
    }

    public async Task<TemplateApi<RoleDto>> InsertRoleAndNavigationRole(RoleDto roleDto, List<Guid> idNavigationRole,
        Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            #region Role

            await connection.ExecuteAsync(RoleSqlQueries.QueryInsertRole, roleDto, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng Role",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "Role",
                IsSuccess = true,
                WithId = roleDto.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            #endregion

            #region NavigationRole

            //init list data before insert into table navigationRole
            var navigationRoles = idNavigationRole.Select(item => new NavigationRole()
                { Id = Guid.NewGuid(), IdNavigation = item, IdRole = roleDto.Id }).ToList();

            await connection.ExecuteAsync(NavigationRoleSqlQueries.QueryInsertNavigationRole, navigationRoles, tran);

            var diaries = navigationRoles.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng NavigationRole",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "NavigationRole",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<RoleDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<RoleDto>> UpdateRoleAndNavigationRole(RoleDto roleDto, List<Guid> idNavigationRole,
        Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            #region Role

            var role = roleDto.Adapt<Role>();
            await connection.ExecuteAsync(RoleSqlQueries.QueryUpdateRole, role, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng Role",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "Role",
                IsSuccess = true,
                WithId = role.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            #endregion

            #region NavigationRole

            var idsRole = new List<Guid>() { role.Id };

            //get all navigation role by id role 
            var navigationRoleByIdRole = (
                    await connection.QueryAsync<NavigationRole>(NavigationRoleSqlQueries.QueryGetNavigationRoleByIdRole,
                        new { IdRole = idsRole }, tran))
                .ToList();

            //delete above data 
            await connection.ExecuteAsync(NavigationRoleSqlQueries.QueryDeleteNavigationRole,
                new {Ids = navigationRoleByIdRole.Select(e => e.Id).ToList()}, tran);

            //init list data before insert into table navigationRole
            var navigationRoles = idNavigationRole.Select(item => new NavigationRole()
                { Id = Guid.NewGuid(), IdNavigation = item, IdRole = roleDto.Id }).ToList();

            await connection.ExecuteAsync(NavigationRoleSqlQueries.QueryInsertNavigationRole, navigationRoles, tran);

            var diaries = navigationRoles.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng NavigationRole",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "NavigationRole",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<RoleDto>(null, null, "Cập nật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<RoleDto>> DeleteRoleAndNavigationRole(List<Guid> roleIds, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            #region NavigationRole

            //get all navigation role by id role 
            var navigationRoleByIdRole = (
                    await connection.QueryAsync<NavigationRole>(NavigationRoleSqlQueries.QueryGetNavigationRoleByIdRole,
                        new { IdRole = roleIds }, tran))
                .ToList();

            //delete above data 
            await connection.ExecuteAsync(NavigationRoleSqlQueries.QueryDeleteNavigationRole,
                new {Ids = navigationRoleByIdRole.Select(e => e.Id).ToList()}, tran);

            var diaries = navigationRoleByIdRole.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng NavigationRole",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "NavigationRole",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            #region Role

            await connection.ExecuteAsync(RoleSqlQueries.QueryDeleteRole, new {Ids = roleIds}, tran);

            diaries = roleIds.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng Role",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "Role",
                IsSuccess = true,
                WithId = item
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<RoleDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<RoleAndNavigation>> GetRoleAndNavigation(Guid roleId)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var role = await connection.QueryFirstOrDefaultAsync<Role>(RoleSqlQueries.GetByIdRole, new { Id = roleId });
        var navigationByRole = (await connection.QueryAsync<Navigation>(NavigationSqlQueries.QueryGetAllNavigationByIdRole,
            new { IdRole = roleId })).ToList();

        var result = new RoleAndNavigation
        {
            Role = role,
            Navigation = navigationByRole
        };

        return new Pagination().HandleGetByIdRespond(result);
    }

    public async Task<IEnumerable<InformationRoleOfUser>> GetRoleByIdUser(Guid idUser)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var results = (await connection.QueryAsync<InformationRoleOfUser>(RoleSqlQueries.GetAllInformRoleOfUser,
            new { Id = idUser })).ToList();

        return results;
    }

    public async Task<TemplateApi<RoleDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var result = (await connection.QueryAsync<Role>(RoleSqlQueries.GetAllRole)).ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, result.Select(u => u.Adapt<RoleDto>()),
            result.Count);
    }

    public Task<TemplateApi<RoleDto>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<RoleDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<RoleDto>> Update(RoleDto model, Guid idUserCurrent, string fullName)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<RoleDto>> Insert(RoleDto model, Guid idUserCurrent, string fullName)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<RoleDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<RoleDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion
}