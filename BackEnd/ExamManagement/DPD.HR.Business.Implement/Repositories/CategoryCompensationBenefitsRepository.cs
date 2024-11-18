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

public class CategoryCompensationBenefitsRepository : ICategoryCompensationBenefitsRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryCompensationBenefitsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ CategoryCompensationBenefitsRepository Methods ]==================================================

    public async Task<TemplateApi<CategoryCompensationBenefitsDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<CategoryCompensationBenefits>(CategoryCompensationBenefitsSqlQueries
                .QueryGetAllCategoryCompensationBenefits))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<CategoryCompensationBenefitsDto>()),
            units.Count);
    }

    public async Task<TemplateApi<CategoryCompensationBenefitsDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<CategoryCompensationBenefits>(
            CategoryCompensationBenefitsSqlQueries.QueryGetByIdCategoryCompensationBenefits, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<CategoryCompensationBenefitsDto>());
    }

    public Task<TemplateApi<CategoryCompensationBenefitsDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<CategoryCompensationBenefitsDto>> Update(CategoryCompensationBenefitsDto model,
        Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCompensationBenefits = model.Adapt<CategoryCompensationBenefits>();
            await connection.ExecuteAsync(
                CategoryCompensationBenefitsSqlQueries.QueryUpdateCategoryCompensationBenefits,
                categoryCompensationBenefits, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryCompensationBenefits",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryCompensationBenefits",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<CategoryCompensationBenefitsDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0,
                0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryCompensationBenefitsDto>> Insert(CategoryCompensationBenefitsDto model,
        Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(
                CategoryCompensationBenefitsSqlQueries.QueryInsertCategoryCompensationBenefits, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng CategoryCompensationBenefits",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "CategoryCompensationBenefits",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<CategoryCompensationBenefitsDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0,
                0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryCompensationBenefitsDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var employeeBenefits = await connection.QueryFirstOrDefaultAsync<EmployeeBenefits>(
                EmployeeBenefitsSqlQueries.QueryEmployeeBenefitsByIdIdCategoryCompensationBenefits,
                new { IdCategoryCompensationBenefits = ids }, tran);

            if (employeeBenefits != null)
            {
                return new TemplateApi<CategoryCompensationBenefitsDto>(null, null, "Dã có dữ liệu không thể xóa !", true, 0, 0, 0, 0);
            }

            var categoryCompensationBenefits =
                (await connection.QueryAsync<CategoryCompensationBenefits>(
                    CategoryCompensationBenefitsSqlQueries.QueryGetCategoryCompensationBenefitsByIds, new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(
                CategoryCompensationBenefitsSqlQueries.QueryInsertCategoryCompensationBenefitsDeleted,
                categoryCompensationBenefits, tran);

            await connection.ExecuteAsync(
                CategoryCompensationBenefitsSqlQueries.QueryDeleteCategoryCompensationBenefits, new { Ids = ids },
                tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng CategoryCompensationBenefits",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "CategoryCompensationBenefits",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<CategoryCompensationBenefitsDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<CategoryCompensationBenefitsDto>> HideByList(List<Guid> ids, bool isLock,
        Guid idUserCurrent, string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion
}