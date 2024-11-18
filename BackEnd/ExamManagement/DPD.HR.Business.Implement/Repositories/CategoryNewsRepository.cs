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

public class CategoryNewsRepository : ICategoryNewsRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryNewsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ CategoryNewsRepository Methods ]==================================================

    public async Task<TemplateApi<CategoryNewsDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<CategoryNews>(CategoryNewsSqlQueries.QueryGetAllCategoryNews))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<CategoryNewsDto>()),
            units.Count);
    }

    public async Task<TemplateApi<CategoryNewsDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<CategoryNews>(
            CategoryNewsSqlQueries.QueryGetByIdCategoryNews, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<CategoryNewsDto>());
    }

    public async Task<TemplateApi<CategoryNewsDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<CategoryNews>(CategoryNewsSqlQueries.QueryGetAllCategoryNewsAvailable,
                new { IsHide = false, IsDeleted = false }))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<CategoryNewsDto>()),
            units.Count);
    }

    public async Task<TemplateApi<CategoryNewsDto>> Update(CategoryNewsDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryNewsById = await connection.QueryFirstOrDefaultAsync<CategoryNews>(
                CategoryNewsSqlQueries.QueryGetByIdCategoryNews, new { Id = model.Id }, tran);

            var categoryNews = model.Adapt<CategoryNews>();
            if (model.IdFile is not null)
            {
                categoryNews.Avatar = categoryNewsById.Avatar;
            }

            await connection.ExecuteAsync(CategoryNewsSqlQueries.QueryUpdateCategoryNews, categoryNews, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryNews",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryNews",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<CategoryNewsDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryNewsDto>> Insert(CategoryNewsDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(CategoryNewsSqlQueries.QueryInsertCategoryNews, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng CategoryNews",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "CategoryNews",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<CategoryNewsDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryNewsDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<CategoryNews>(CategoryNewsSqlQueries.QueryGetCategoryNewsByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(CategoryNewsSqlQueries.QueryInsertCategoryNewsDeleted, categoryCities, tran);

            await connection.ExecuteAsync(CategoryNewsSqlQueries.QueryDeleteCategoryNews, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng CategoryNews",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "CategoryNews",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<CategoryNewsDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryNewsDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(CategoryNewsSqlQueries.QueryUpdateHideCategoryNews,
                new { IsHide = isLock, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryNews",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryNews",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<CategoryNewsDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryNewsDto>> UpdateShowChild(List<Guid> ids, bool isShow, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(CategoryNewsSqlQueries.QueryUpdateShowChildCategoryNews,
                new { ShowChild = isShow, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryNews",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryNews",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<CategoryNewsDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryNewsDto>> GetByIdParent(int pageNumber, int pageSize, Guid idParent)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<CategoryNews>(CategoryNewsSqlQueries.QueryGetAllCategoryNewsByParentId,
                new { ParentId = idParent }))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<CategoryNewsDto>()),
            units.Count);
    }

    #endregion
}