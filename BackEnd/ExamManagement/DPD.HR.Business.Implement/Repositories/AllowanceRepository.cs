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

public class AllowanceRepository : IAllowanceRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public AllowanceRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ AllowanceRepository Methods ]==================================================

    public async Task<TemplateApi<AllowanceDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var allowances = (await connection.QueryAsync<Allowance>(AllowanceSqlQueries.QueryGetAllAllowance))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            allowances.Select(u => u.Adapt<AllowanceDto>()),
            allowances.Count);
    }

    public async Task<TemplateApi<AllowanceDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<Allowance>(
            AllowanceSqlQueries.QueryGetByIdAllowance, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<AllowanceDto>());
    }

    public Task<TemplateApi<AllowanceDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<AllowanceDto>> Update(AllowanceDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var allowance = model.Adapt<Allowance>();
            await connection.ExecuteAsync(AllowanceSqlQueries.QueryUpdateAllowance, allowance, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng Allowance",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "Allowance",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<AllowanceDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<AllowanceDto>> Insert(AllowanceDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(AllowanceSqlQueries.QueryInsertAllowance, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng Allowance",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "Allowance",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<AllowanceDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<AllowanceDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var employeeAllowance = await connection.QueryFirstOrDefaultAsync<EmployeeAllowance>(
                EmployeeAllowanceSqlQueries.QueryEmployeeAllowanceByIdAllowance,
                new { IdAllowance = ids }, tran);

            if (employeeAllowance != null)
            {
                return new TemplateApi<AllowanceDto>(null, null, "Dã có dữ liệu không thể xóa !", true, 0, 0, 0, 0);
            }

            var allowances =
                (await connection.QueryAsync<Allowance>(AllowanceSqlQueries.QueryGetAllAllowanceByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(AllowanceSqlQueries.InsertAllowanceDelete, allowances, tran);

            await connection.ExecuteAsync(AllowanceSqlQueries.QueryDeleteListAllowance, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng Allowance",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "Allowance",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<AllowanceDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<AllowanceDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion
}