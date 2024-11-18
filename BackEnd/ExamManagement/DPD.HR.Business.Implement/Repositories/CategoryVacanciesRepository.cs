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

public class CategoryVacanciesRepository : ICategoryVacanciesRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryVacanciesRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ CategoryVacanciesRepository Methods ]==================================================

    public async Task<TemplateApi<CategoryVacanciesDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<CategoryVacancies>(CategoryVacanciesSqlQueries
                .QueryGetAllCategoryVacancies))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<CategoryVacanciesDto>()),
            units.Count);
    }

    public async Task<TemplateApi<CategoryVacanciesDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<CategoryVacancies>(
            CategoryVacanciesSqlQueries.QueryGetByIdCategoryVacancies, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<CategoryVacanciesDto>());
    }

    public Task<TemplateApi<CategoryVacanciesDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<CategoryVacanciesDto>> Update(CategoryVacanciesDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryVacancies = model.Adapt<CategoryVacancies>();
            await connection.ExecuteAsync(CategoryVacanciesSqlQueries.QueryUpdateCategoryVacancies, categoryVacancies,
                tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryVacancies",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryVacancies",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<CategoryVacanciesDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryVacanciesDto>> Insert(CategoryVacanciesDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(CategoryVacanciesSqlQueries.QueryInsertCategoryVacancies, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng CategoryVacancies",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "CategoryVacancies",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<CategoryVacanciesDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryVacanciesDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var requestToHiredByIdVacancy =
                await connection.QueryFirstOrDefaultAsync<RequestToHired>(
                    RequestToHiredSqlQueries.QueryRequestToHiredByIdCategoryVacancies,
                    new { IdCategoryVacancies = ids }, tran);

            if (requestToHiredByIdVacancy != null)
            {
                return new TemplateApi<CategoryVacanciesDto>(null, null, "Danh mục này đã có dữ liệu không thể xóa !",
                    false, 0, 0, 0, 0);
            }

            var categoryVacancies =
                (await connection.QueryAsync<CategoryVacancies>(CategoryVacanciesSqlQueries.QueryCategoryVacanciesByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(CategoryVacanciesSqlQueries.QueryInsertCategoryVacanciesDeleted,
                categoryVacancies, tran);

            await connection.ExecuteAsync(CategoryVacanciesSqlQueries.QueryDeleteCategoryVacancies, new { Ids = ids },
                tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng CategoryVacancies",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "CategoryVacancies",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<CategoryVacanciesDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<CategoryVacanciesDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<CategoryVacanciesDto>> UpdateStatusVacancy(int status, Guid idCategoryVacancy,
        Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(CategoryVacanciesSqlQueries.QueryUpdateStatusCategoryVacancy,
                new { Status = status, Id = idCategoryVacancy },
                tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng CategoryVacancies",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "CategoryVacancies",
                IsSuccess = true,
                WithId = idCategoryVacancy
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<CategoryVacanciesDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<CategoryVacanciesDto>> GetVacancyApproved(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var vacanciesList = (await connection.QueryAsync<CategoryVacancies>(CategoryVacanciesSqlQueries
                .QueryGetAllCategoryVacancies))
            .ToList();

        vacanciesList = vacanciesList.Where(e => e.Status == 1).ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            vacanciesList.Select(u => u.Adapt<CategoryVacanciesDto>()),
            vacanciesList.Count);
    }

    #endregion
}