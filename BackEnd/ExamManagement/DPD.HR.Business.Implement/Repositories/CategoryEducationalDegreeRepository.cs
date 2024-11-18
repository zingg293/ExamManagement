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

public class CategoryEducationalDegreeRepository : ICategoryEducationalDegreeRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryEducationalDegreeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ CategoryEducationalDegreeRepositoryRepository Methods ]==================================================

    public async Task<TemplateApi<CategoryEducationalDegreeDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<CategoryEducationalDegree>(CategoryEducationalDegreeSqlQueries.QueryGetAllCategoryEducationalDegree))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<CategoryEducationalDegreeDto>()),
            units.Count);
    }

    public async Task<TemplateApi<CategoryEducationalDegreeDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<CategoryEducationalDegree>(
            CategoryEducationalDegreeSqlQueries.QueryGetByIdCategoryEducationalDegree, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<CategoryEducationalDegreeDto>());
    }

    public async Task<TemplateApi<CategoryEducationalDegreeDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<CategoryEducationalDegreeRepository>(CategoryEducationalDegreeSqlQueries.QueryGetAllCategoryEducationalDegreeAvailable))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<CategoryEducationalDegreeDto>()),
            units.Count);
    }

    public async Task<TemplateApi<CategoryEducationalDegreeDto>> Update(CategoryEducationalDegreeDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var CategoryEducationalDegreeRepository = model.Adapt<CategoryEducationalDegree>();
            await connection.ExecuteAsync(CategoryEducationalDegreeSqlQueries.QueryUpdateCategoryEducationalDegree, CategoryEducationalDegreeRepository, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryEducationalDegreeRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryEducationalDegreeRepository",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<CategoryEducationalDegreeDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryEducationalDegreeDto>> Insert(CategoryEducationalDegreeDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(CategoryEducationalDegreeSqlQueries.QueryInsertCategoryEducationalDegree, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng CategoryEducationalDegreeRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "CategoryEducationalDegreeRepository",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<CategoryEducationalDegreeDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryEducationalDegreeDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<CategoryEducationalDegree>(CategoryEducationalDegreeSqlQueries.QueryGetCategoryEducationalDegreeByIds, new { Ids = ids },
                    tran))
                .ToList();
            
            await connection.ExecuteAsync(CategoryEducationalDegreeSqlQueries.QueryInsertCategoryEducationalDegreeDeleted, categoryCities, tran);
            
            await connection.ExecuteAsync(CategoryEducationalDegreeSqlQueries.QueryDeleteCategoryEducationalDegree, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng CategoryEducationalDegreeRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "CategoryEducationalDegreeRepository",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<CategoryEducationalDegreeDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryEducationalDegreeDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(CategoryEducationalDegreeSqlQueries.QueryHideCategoryEducationalDegree, new { IsHide = isLock, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryEducationalDegreeRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryEducationalDegreeRepository",
                IsSuccess = true,
                WithId = id
            }).ToList();
            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);
            tran.Commit();
            return new TemplateApi<CategoryEducationalDegreeDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    #endregion
}