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

public class EmployeeAllowanceRepository : IEmployeeAllowanceRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public EmployeeAllowanceRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ EmployeeAllowanceRepository ]=================================================================

    public Task<TemplateApi<EmployeeAllowanceDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<EmployeeAllowanceDto>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<EmployeeAllowanceDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<EmployeeAllowanceDto>> Update(EmployeeAllowanceDto model, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<EmployeeAllowanceDto>> Insert(EmployeeAllowanceDto model, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<EmployeeAllowanceDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<EmployeeAllowanceDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<EmployeeAllowanceDto>> GetByIdEmployee(Guid employeeId, int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var employeeAllowances = (await connection.QueryAsync<EmployeeAllowance>(EmployeeAllowanceSqlQueries
                .QueryGetAllEmployeeAllowanceByIdEmployee, new { IdEmployee = employeeId }))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            employeeAllowances.Select(u => u.Adapt<EmployeeAllowanceDto>()),
            employeeAllowances.Count);
    }

    public async Task<TemplateApi<EmployeeAllowanceDto>> InsertEmployeeAndAllowance(Guid employeeId,
        List<Guid> idAllowance,
        Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var employeeAllowances = (await connection.QueryAsync<EmployeeAllowance>(
                    EmployeeAllowanceSqlQueries
                        .QueryGetAllEmployeeAllowanceByIdEmployee, new { IdEmployee = employeeId }, tran))
                .ToList();

            if (employeeAllowances.Any())
            {
                await connection.ExecuteAsync(EmployeeAllowanceSqlQueries.QueryDeleteEmployeeAllowance,
                    new { Ids = employeeAllowances.Select(e => e.Id) },
                    tran);

                var diaries = employeeAllowances.Select(item => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảng EmployeeAllowance",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "EmployeeAllowance",
                    IsSuccess = true,
                    WithId = item.Id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);
            }

            var allowances = idAllowance.Select(item => new EmployeeAllowance()
                    { Id = Guid.NewGuid(), IdEmployee = employeeId, IdAllowance = item, CreatedDate = DateTime.Now })
                .ToList();

            await connection.ExecuteAsync(EmployeeAllowanceSqlQueries.QueryInsertEmployeeAllowance, allowances,
                tran);

            var diariesAdd = allowances.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng EmployeeAllowance",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "EmployeeAllowance",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diariesAdd, tran);

            tran.Commit();
            return new TemplateApi<EmployeeAllowanceDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    #endregion
}