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

public class PreviousSalaryInformationRepository : IPreviousSalaryInformationRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public PreviousSalaryInformationRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ PreviousSalaryInformationRepository Methods ]==================================================

    public async Task<TemplateApi<PreviousSalaryInformationDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<PreviousSalaryInformation>(PreviousSalaryInformationSqlQueries.QueryGetAllPreviousSalaryInformation))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<PreviousSalaryInformationDto>()),
            units.Count);
    }

    public async Task<TemplateApi<PreviousSalaryInformationDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<PreviousSalaryInformation>(
            PreviousSalaryInformationSqlQueries.QueryGetByIdPreviousSalaryInformation, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<PreviousSalaryInformationDto>());
    }

    public async Task<TemplateApi<PreviousSalaryInformationDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<PreviousSalaryInformation>(PreviousSalaryInformationSqlQueries.QueryGetAllPreviousSalaryInformationAvailable))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<PreviousSalaryInformationDto>()),
            units.Count);
    }

    public async Task<TemplateApi<PreviousSalaryInformationDto>> Update(PreviousSalaryInformationDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var PreviousSalaryInformation = model.Adapt<PreviousSalaryInformation>();
            await connection.ExecuteAsync(PreviousSalaryInformationSqlQueries.QueryUpdatePreviousSalaryInformation, PreviousSalaryInformation, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng PreviousSalaryInformation",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "PreviousSalaryInformation",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<PreviousSalaryInformationDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<PreviousSalaryInformationDto>> Insert(PreviousSalaryInformationDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(PreviousSalaryInformationSqlQueries.QueryInsertPreviousSalaryInformation, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng PreviousSalaryInformation",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "PreviousSalaryInformation",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<PreviousSalaryInformationDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<PreviousSalaryInformationDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<PreviousSalaryInformation>(PreviousSalaryInformationSqlQueries.QueryGetPreviousSalaryInformationByIds, new { Ids = ids },
                    tran))
                .ToList();
            
            await connection.ExecuteAsync(PreviousSalaryInformationSqlQueries.QueryInsertPreviousSalaryInformationDeleted, categoryCities, tran);
            
            await connection.ExecuteAsync(PreviousSalaryInformationSqlQueries.QueryDeletePreviousSalaryInformation, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng PreviousSalaryInformation",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "PreviousSalaryInformation",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<PreviousSalaryInformationDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<PreviousSalaryInformationDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(PreviousSalaryInformationSqlQueries.QueryHidePreviousSalaryInformation,
                new { IsHide = isLock, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng PreviousSalaryInformation",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "PreviousSalaryInformation",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<PreviousSalaryInformationDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    #endregion
}