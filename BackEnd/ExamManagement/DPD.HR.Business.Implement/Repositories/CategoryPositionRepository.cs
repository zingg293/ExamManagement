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

public class CategoryPositionRepository : ICategoryPositionRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryPositionRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ CategoryPositionRepository Methods ]==================================================

    public async Task<TemplateApi<CategoryPositionDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var categoryPosition = (await connection.QueryAsync<CategoryPosition>(CategoryPositionSqlQueries
                .QueryGetAllCategoryPosition))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            categoryPosition.Select(u => u.Adapt<CategoryPositionDto>()),
            categoryPosition.Count);
    }

    public async Task<TemplateApi<CategoryPositionDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<CategoryPosition>(
            CategoryPositionSqlQueries.QueryGetByIdCategoryPosition, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<CategoryPositionDto>());
    }

    public async Task<TemplateApi<CategoryPositionDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<CategoryPosition>(CategoryPositionSqlQueries
                .QueryGetAllCategoryPositionAvailable))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<CategoryPositionDto>()),
            units.Count);
    }

    public async Task<TemplateApi<CategoryPositionDto>> Update(CategoryPositionDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryPosition = model.Adapt<CategoryPosition>();
            await connection.ExecuteAsync(CategoryPositionSqlQueries.QueryUpdateCategoryPosition, categoryPosition,
                tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryPosition",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryPosition",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<CategoryPositionDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryPositionDto>> Insert(CategoryPositionDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(CategoryPositionSqlQueries.QueryInsertCategoryPosition, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng CategoryPosition",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "CategoryPosition",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<CategoryPositionDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryPositionDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var promotionTransfers =
                (await connection.QueryAsync<PromotionTransfer>(
                    PromotionTransferSqlQueries.QueryGetPromotionTransferByIdCategoryPosition,
                    new { IdCategoryPosition = ids },
                    tran))
                .ToList();

            if (promotionTransfers.Count > 0)
            {
                return new TemplateApi<CategoryPositionDto>(null, null, "Đã có dữ liệu !", false, 0, 0, 0, 0);
            }

            var categoryPositions =
                (await connection.QueryAsync<CategoryPosition>(CategoryPositionSqlQueries.QueryGetCategoryPositionByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(CategoryPositionSqlQueries.QueryInsertCategoryPositionDeleted,
                categoryPositions,
                tran);

            await connection.ExecuteAsync(CategoryPositionSqlQueries.QueryDeleteCategoryPosition, new { Ids = ids },
                tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng CategoryPosition",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "CategoryPosition",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<CategoryPositionDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryPositionDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var promotionTransfers =
                (await connection.QueryAsync<PromotionTransfer>(
                    PromotionTransferSqlQueries.QueryGetPromotionTransferByIdCategoryPosition,
                    new { IdCategoryPosition = ids },
                    tran))
                .ToList();

            if (promotionTransfers.Count > 0)
            {
                return new TemplateApi<CategoryPositionDto>(null, null, "Đã có dữ liệu !", false, 0, 0, 0, 0);
            }
            
            await connection.ExecuteAsync(CategoryPositionSqlQueries.QueryHideCategoryPosition,
                new { IsActive = isLock, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryPosition",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryPosition",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<CategoryPositionDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    #endregion
}