using System.Data;
using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DPD.HR.Application.Implement.Repositories;

public class UserTypeRepository : IUserTypeRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public UserTypeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ UserTypeRepository ]=================================================================

    public async Task<UserTypeDto> GetTypeUser(string typeCode)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var userType = await connection.QueryFirstOrDefaultAsync<UserType>(UserTypeSqlQueries.GetAllUserTypeByTypeCode,
            new { TypeCode = typeCode });

        return userType.Adapt<UserTypeDto>();
    }

    public async Task<UserTypeDto> GetUserType(Guid idUserType)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var userType = await
            connection.QueryFirstOrDefaultAsync<UserType>(UserTypeSqlQueries.GetUserTypeById,
                new { Id = idUserType });
        return userType.Adapt<UserTypeDto>();
    }

    public async Task<TemplateApi<UserTypeDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        var userTypes = (await connection.QueryAsync<UserType>(UserTypeSqlQueries.GetAllUserType)).ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, userTypes.Select(u => u.Adapt<UserTypeDto>()),
            userTypes.Count);
    }

    public async Task<TemplateApi<UserTypeDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<UserType>(
            UserTypeSqlQueries.GetUserTypeById, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<UserTypeDto>());
    }

    public Task<TemplateApi<UserTypeDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<UserTypeDto>> Update(UserTypeDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var userType = model.Adapt<UserType>();
            await connection.ExecuteAsync(UserTypeSqlQueries.QueryUpdateUserType, userType, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng UserType",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "UserType",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<UserTypeDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<UserTypeDto>> Insert(UserTypeDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(UserTypeSqlQueries.QueryInsertUserType, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng UserType",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "UserType",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<UserTypeDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<UserTypeDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var userByType =
                (await connection.QueryAsync<User>(UserSqlQueries.UserByIdsUserType, new { UserTypeId = ids },
                    tran))
                .ToList();

            if (userByType.Any())
            {
                return new TemplateApi<UserTypeDto>(null, null, "Đã có dữ liệu không thể xóa !", false, 0, 0, 0, 0);
            }
            
            var userTypes =
                (await connection.QueryAsync<UserType>(UserTypeSqlQueries.GetUserTypeByIds, new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(UserTypeSqlQueries.QueryInsertUserTypeDelete, userTypes, tran);

            await connection.ExecuteAsync(UserTypeSqlQueries.QueryDeleteUserType, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng UserType",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "UserType",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<UserTypeDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<UserTypeDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion
}