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

public class AllowancePreviousSalaryInformationRepository : IAllowancePreviousSalaryInformationRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public AllowancePreviousSalaryInformationRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ AllowancePreviousSalaryInformationRepositoryRepository Methods ]==================================================

    public async Task<TemplateApi<AllowancePreviousSalaryInformationDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<AllowancePreviousSalaryInformation>(AllowancePreviousSalaryInformationSqlQueries.QueryGetAllAllowancePreviousSalaryInformationRepository))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<AllowancePreviousSalaryInformationDto>()),
            units.Count);
    }

    public async Task<TemplateApi<AllowancePreviousSalaryInformationDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<AllowancePreviousSalaryInformation>(
            AllowancePreviousSalaryInformationSqlQueries.QueryGetByIdAllowancePreviousSalaryInformation, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<AllowancePreviousSalaryInformationDto>());
    }

    public Task<TemplateApi<AllowancePreviousSalaryInformationDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        //using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        //connection.Open();

        //var units = (await connection.QueryAsync<AllowancePreviousSalaryInformationRepository>(AllowancePreviousSalaryInformationSqlQueries.QueryGetAllAllowancePreviousSalaryInformationAvailable))
        //    .ToList();

        //return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<AllowancePreviousSalaryInformationDto>()),
        //    units.Count);
        throw new NotImplementedException();

    }

    public async Task<TemplateApi<AllowancePreviousSalaryInformationDto>> Update(AllowancePreviousSalaryInformationDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var AllowancePreviousSalaryInformation = model.Adapt<AllowancePreviousSalaryInformation>();
            await connection.ExecuteAsync(AllowancePreviousSalaryInformationSqlQueries.QueryUpdateAllowancePreviousSalaryInformation, AllowancePreviousSalaryInformation, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng AllowancePreviousSalaryInformation",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "AllowancePreviousSalaryInformation",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<AllowancePreviousSalaryInformationDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<AllowancePreviousSalaryInformationDto>> Insert(AllowancePreviousSalaryInformationDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(AllowancePreviousSalaryInformationSqlQueries.QueryInsertAllowancePreviousSalaryInformation, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng AllowancePreviousSalaryInformationRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "AllowancePreviousSalaryInformationRepository",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<AllowancePreviousSalaryInformationDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<AllowancePreviousSalaryInformationDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<AllowancePreviousSalaryInformation>(AllowancePreviousSalaryInformationSqlQueries.QueryGetAllowancePreviousSalaryInformationByIds, new { Ids = ids },
                    tran))
                .ToList();
            
            await connection.ExecuteAsync(AllowancePreviousSalaryInformationSqlQueries.QueryInsertAllowancePreviousSalaryInformationDeleted, categoryCities, tran);
            
            await connection.ExecuteAsync(AllowancePreviousSalaryInformationSqlQueries.QueryDeleteAllowancePreviousSalaryInformation, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng AllowancePreviousSalaryInformationRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "AllowancePreviousSalaryInformationRepository",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<AllowancePreviousSalaryInformationDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<AllowancePreviousSalaryInformationDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();
        try
        {
            await connection.ExecuteAsync(AllowancePreviousSalaryInformationSqlQueries.QueryHideAllowancePreviousSalaryInformation,
                new { IsHide = isLock, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng AllowancePreviousSalaryInformationRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "AllowancePreviousSalaryInformationRepository",
                IsSuccess = true,
                WithId = id
            }).ToList();
            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);
            tran.Commit();
            return new TemplateApi<AllowancePreviousSalaryInformationDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }
    #endregion
}