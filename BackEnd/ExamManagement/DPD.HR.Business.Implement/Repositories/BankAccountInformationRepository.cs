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

public class BankAccountInformationRepository : IBankAccountInformationRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public BankAccountInformationRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ BankAccountInformationRepositoryRepository Methods ]==================================================

    public async Task<TemplateApi<BankAccountInformationDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<BankAccountInformation>(BankAccountInformationSqlQueries.QueryGetAllBankAccountInformation))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<BankAccountInformationDto>()),
            units.Count);
    }

    public async Task<TemplateApi<BankAccountInformationDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<BankAccountInformation>(
            BankAccountInformationSqlQueries.QueryGetByIdBankAccountInformation, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<BankAccountInformationDto>());
    }

    public async Task<TemplateApi<BankAccountInformationDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<BankAccountInformationRepository>(BankAccountInformationSqlQueries.QueryGetAllBankAccountInformation))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<BankAccountInformationDto>()),
            units.Count);
    }

    public async Task<TemplateApi<BankAccountInformationDto>> Update(BankAccountInformationDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var bankAccountInformation = model.Adapt<BankAccountInformation>();
            await connection.ExecuteAsync(BankAccountInformationSqlQueries.QueryUpdateBankAccountInformation, bankAccountInformation, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng BankAccountInformationRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "BankAccountInformationRepository",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<BankAccountInformationDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<BankAccountInformationDto>> Insert(BankAccountInformationDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(BankAccountInformationSqlQueries.QueryInsertBankAccountInformation, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng BankAccountInformationRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "BankAccountInformationRepository",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<BankAccountInformationDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<BankAccountInformationDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<BankAccountInformation>(BankAccountInformationSqlQueries.QueryGetBankAccountInformationByIds, new { Ids = ids },
                    tran))
                .ToList();
            
            await connection.ExecuteAsync(BankAccountInformationSqlQueries.QueryInsertBankAccountInformationDeleted, categoryCities, tran);
            
            await connection.ExecuteAsync(BankAccountInformationSqlQueries.QueryDeleteBankAccountInformation, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng BankAccountInformationRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "BankAccountInformationRepository",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<BankAccountInformationDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<BankAccountInformationDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(BankAccountInformationSqlQueries.QueryHideBankAccountInformation,
                new { IsHide = isLock, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng BankAccountInformationRepository",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "BankAccountInformationRepository",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<BankAccountInformationDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    #endregion
}