using System.Data;
using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities;
using DPD.HumanResources.Utilities.Utils;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DPD.HR.Application.Implement.Repositories;

public class UserRepository : IUserRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ CRUD TABLE USER ]=================================================================

    public async Task<TemplateApi<UserDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var users = (await connection.QueryAsync<User>(UserSqlQueries.AllUser)).ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, users.Select(u => u.Adapt<UserDto>()),
            users.Count);
    }

    public async Task<TemplateApi<UserDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var user = await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.UserById, new { Id = id });

        return new Pagination().HandleGetByIdRespond(user.Adapt<UserDto>());
    }

    public async Task<TemplateApi<UserDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var users = (await connection.QueryAsync<User>(UserSqlQueries.AllUserAvailable)).ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, users.Select(u => u.Adapt<UserDto>()),
            users.Count);
    }

    public async Task<TemplateApi<UserDto>> Update(UserDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var user = model.Adapt<User>();
            await connection.ExecuteAsync(UserSqlQueries.UpdateUser, user, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng User",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "User",
                IsSuccess = true,
                WithId = user.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);
            tran.Commit();
            return new TemplateApi<UserDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<UserDto>> Insert(UserDto model, Guid idUserCurrent, string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<UserDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {

            foreach (var id in ids)
            {
                await connection.ExecuteAsync(UserSqlQueries.DeleteUser, new { Id = id }, tran);
            }

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng User",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "User",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<UserDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<UserDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(UserSqlQueries.HideUser, new { Ids = ids, IsLocked = isLock }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = isLock ? "Khóa tài khoản" : "Mở khóa tài khoản",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "User",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);
            tran.Commit();

            return new TemplateApi<UserDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<UserAndRole>> GetUserAndRole(Guid idUser)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var user = await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.UserById, new { Id = idUser });

        var roleOfUser = (await connection.QueryAsync<Role>(RoleSqlQueries.GetRoleByIdUser, new { IdUser = idUser }))
            .ToList();

        var userAndRole = new UserAndRole()
        {
            User = user.Adapt<UserDto>(),
            Roles = roleOfUser
        };

        return new Pagination().HandleGetByIdRespond(userAndRole.Adapt<UserAndRole>());
    }

    public async Task<UserDto> UserByIdInternal(Guid idUser)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var user = await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.UserById, new { Id = idUser });

        return user.Adapt<UserDto>();
    }

    public async Task<TemplateApi<UserDto>> InsertUser(UserDto newUser, Guid idUserCurrent, string fullName,
        string password, string salt)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        using var tran = connection.BeginTransaction();

        try
        {

            var user = newUser.Adapt<User>();
            user.Password = password;
            user.Salt = salt;
            await connection.ExecuteAsync(UserSqlQueries.InsertUser, user, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng User",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "User",
                IsSuccess = true,
                WithId = user.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<UserDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    #endregion

    #region ===[ LOGIN AND REGISTER ACCOUNT ]=================================================================

    public async Task<User> UserByEmail(string email)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var user = await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.UserByEmail, new { Email = email });
        return user;
    }

    public async Task<TemplateApi<UserDto>> ActiveUserByCode(string email, string code)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var user = connection.QueryFirstOrDefault<User>(UserSqlQueries.UserByEmail, new { Email = email }, tran);

            if (user is null)
            {
                return new TemplateApi<UserDto>(null, null, "Email không tồn tại !", true, 0, 0, 0, 0);
            }

            await connection.ExecuteAsync(UserSqlQueries.ActiveUser, new { user.Id }, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{user.Fullname} đã cập nhật bảng User",
                UserId = user.Id,
                UserName = user.Fullname,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "User",
                IsSuccess = true,
                WithId = user.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<UserDto>(null, null, "Kích hoạt thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<UserDto>> UpdateActiveCode(string code, string email)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var user = connection.QueryFirstOrDefault<User>(UserSqlQueries.UserByEmail, new { Email = email }, tran);

            if (user is null)
            {
                return new TemplateApi<UserDto>(null, null, "Email không tồn tại !", false, 0, 0, 0, 0);
            }

            await connection.ExecuteAsync(UserSqlQueries.UpdateCodeUser, new { ActiveCode = code, user.Id }, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{user.Fullname} đã cập nhật bảng User",
                UserId = user.Id,
                UserName = user.Fullname,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "User",
                IsSuccess = true,
                WithId = user.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<UserDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<UserDto>> UpdatePassword(string email, string newPassword, string salt)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var user = connection.QueryFirstOrDefault<User>(UserSqlQueries.UserByEmail, new { Email = email }, tran);

            if (user is null)
                return new TemplateApi<UserDto>(null, null, "Email không tồn tại !", false, 0, 0, 0, 0);


            await connection.ExecuteAsync(UserSqlQueries.UpdatePasswordUser, new { Password = newPassword, user.Id },
                tran);
            await connection.ExecuteAsync(UserSqlQueries.UpdateSaltUser, new { Salt = salt, user.Id }, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{user.Fullname} đã cập nhật bảng User",
                UserId = user.Id,
                UserName = user.Fullname,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "User",
                IsSuccess = true,
                WithId = user.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<UserDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    #endregion
}