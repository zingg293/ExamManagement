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

public class CategoryLaborEquipmentRepository : ICategoryLaborEquipmentRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryLaborEquipmentRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ CategoryLaborEquipmentRepository Methods ]==================================================

    public async Task<TemplateApi<CategoryLaborEquipmentDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<CategoryLaborEquipment>(CategoryLaborEquipmentSqlQueries
                .QueryGetAllCategoryLaborEquipment))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<CategoryLaborEquipmentDto>()),
            units.Count);
    }

    public async Task<TemplateApi<CategoryLaborEquipmentDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<CategoryLaborEquipment>(
            CategoryLaborEquipmentSqlQueries.QueryGetByIdCategoryLaborEquipment, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<CategoryLaborEquipmentDto>());
    }

    public Task<TemplateApi<CategoryLaborEquipmentDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<CategoryLaborEquipmentDto>> Update(CategoryLaborEquipmentDto model,
        Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryLaborEquipment = model.Adapt<CategoryLaborEquipment>();
            await connection.ExecuteAsync(CategoryLaborEquipmentSqlQueries.QueryUpdateCategoryLaborEquipment,
                categoryLaborEquipment,
                tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryLaborEquipment",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryLaborEquipment",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<CategoryLaborEquipmentDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryLaborEquipmentDto>> Insert(CategoryLaborEquipmentDto model,
        Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(CategoryLaborEquipmentSqlQueries.QueryInsertCategoryLaborEquipment, model,
                tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng CategoryLaborEquipment",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "CategoryLaborEquipment",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<CategoryLaborEquipmentDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryLaborEquipmentDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var ticketLaborEquipmentDetail =
                await connection.QueryFirstOrDefaultAsync<TicketLaborEquipmentDetail>(
                    TicketLaborEquipmentDetailSqlQueries.QueryTicketLaborEquipmentDetailByIdCategoryLaborEquipment,
                    new { IdCategoryLaborEquipment = ids }, tran);

            if (ticketLaborEquipmentDetail != null)
            {
                return new TemplateApi<CategoryLaborEquipmentDto>(null, null, "Danh mục này đã có dữ liệu không thể xóa !",
                    false, 0, 0, 0, 0);
            }
            
            var categoryLaborEquipments =
                (await connection.QueryAsync<CategoryLaborEquipment>(
                    CategoryLaborEquipmentSqlQueries.QueryCategoryLaborEquipmentByIds, new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(CategoryLaborEquipmentSqlQueries.QueryInsertCategoryLaborEquipmentDeleted,
                categoryLaborEquipments, tran);

            await connection.ExecuteAsync(CategoryLaborEquipmentSqlQueries.QueryDeleteCategoryLaborEquipment,
                new { Ids = ids },
                tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng CategoryLaborEquipment",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "CategoryLaborEquipment",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<CategoryLaborEquipmentDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<CategoryLaborEquipmentDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion
}