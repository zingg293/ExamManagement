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

public class CategoryTypeSalaryScaleRepository : ICategoryTypeSalaryScaleRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryTypeSalaryScaleRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ CategoryTypeSalaryScaleRepositoryRepository Methods ]==================================================

    public async Task<TemplateApi<CategoryTypeSalaryScaleDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<CategoryTypeSalaryScale>(CategoryTypeSalaryScaleSqlQueries.QueryGetAllCategoryTypeSalaryScale))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<CategoryTypeSalaryScaleDto>()),
            units.Count);
    }
    
    public async Task<TemplateApi<CategoryTypeSalaryScaleDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<CategoryTypeSalaryScale>(
            CategoryTypeSalaryScaleSqlQueries.QueryGetByIdCategoryTypeSalaryScale, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<CategoryTypeSalaryScaleDto>());
    }

    public async Task<TemplateApi<CategoryTypeSalaryScaleDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<CategoryTypeSalaryScale>(CategoryTypeSalaryScaleSqlQueries.QueryGetAllCategoryTypeSalaryScaleAvailable))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<CategoryTypeSalaryScaleDto>()),
            units.Count);
    }

    public async Task<TemplateApi<CategoryTypeSalaryScaleDto>> Update(CategoryTypeSalaryScaleDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var CategoryTypeSalaryScaleDto = model.Adapt<CategoryTypeSalaryScaleDto>();
            await connection.ExecuteAsync(CategoryTypeSalaryScaleSqlQueries.QueryUpdateCategoryTypeSalaryScale, CategoryTypeSalaryScaleDto, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryTypeSalaryScale",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryTypeSalaryScale",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<CategoryTypeSalaryScaleDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryTypeSalaryScaleDto>> Insert(CategoryTypeSalaryScaleDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(CategoryTypeSalaryScaleSqlQueries.QueryInsertCategoryTypeSalaryScale, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng CategoryTypeSalaryScale",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "CategoryTypeSalaryScale",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<CategoryTypeSalaryScaleDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryTypeSalaryScaleDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<CategoryTypeSalaryScale>(CategoryTypeSalaryScaleSqlQueries.QueryGetCategoryTypeSalaryScaleByIds, new { Ids = ids },
                    tran))
                .ToList();
            
            await connection.ExecuteAsync(CategoryTypeSalaryScaleSqlQueries.QueryInsertCategoryTypeSalaryScaleDeleted, categoryCities, tran);
            
            await connection.ExecuteAsync(CategoryTypeSalaryScaleSqlQueries.QueryDeleteCategoryTypeSalaryScale, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng CategoryTypeSalaryScaleRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "CategoryTypeSalaryScaleRepository",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<CategoryTypeSalaryScaleDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryTypeSalaryScaleDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(CategoryTypeSalaryScaleSqlQueries.QueryHideCategoryTypeSalaryScale,
                new { IsHide = isLock, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryTypeSalaryScale",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryTypeSalaryScale",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<CategoryTypeSalaryScaleDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    #endregion
}