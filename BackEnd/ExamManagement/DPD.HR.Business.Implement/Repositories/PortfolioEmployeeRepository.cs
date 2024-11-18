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

public class PortfolioEmployeeRepository : IPortfolioEmployeeRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public PortfolioEmployeeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ PortfolioEmployeeRepository Methods ]==================================================

    public async Task<TemplateApi<PortfolioEmployeeDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<PortfolioEmployee>(PortfolioEmployeeSqlQueries.QueryGetAllPortfolioEmployee))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<PortfolioEmployeeDto>()),
            units.Count);
    }

    public async Task<TemplateApi<PortfolioEmployeeDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<PortfolioEmployee>(
            PortfolioEmployeeSqlQueries.QueryGetByIdPortfolioEmployee, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<PortfolioEmployeeDto>());
    }

    public async Task<TemplateApi<PortfolioEmployeeDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<PortfolioEmployee>(PortfolioEmployeeSqlQueries.QueryGetAllPortfolioEmployeeAvailable))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<PortfolioEmployeeDto>()),
            units.Count);
    }

    public async Task<TemplateApi<PortfolioEmployeeDto>> Update(PortfolioEmployeeDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var PortfolioEmployee = model.Adapt<PortfolioEmployee>();
            await connection.ExecuteAsync(PortfolioEmployeeSqlQueries.QueryUpdatePortfolioEmployee, PortfolioEmployee, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng PortfolioEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "PortfolioEmployee",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<PortfolioEmployeeDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<PortfolioEmployeeDto>> Insert(PortfolioEmployeeDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(PortfolioEmployeeSqlQueries.QueryInsertPortfolioEmployee, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng PortfolioEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "PortfolioEmployee",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<PortfolioEmployeeDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<PortfolioEmployeeDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<PortfolioEmployee>(PortfolioEmployeeSqlQueries.QueryGetPortfolioEmployeeByIds, new { Ids = ids },
                    tran))
                .ToList();
            
            await connection.ExecuteAsync(PortfolioEmployeeSqlQueries.QueryInsertPortfolioEmployeeDeleted, categoryCities, tran);
            
            await connection.ExecuteAsync(PortfolioEmployeeSqlQueries.QueryDeletePortfolioEmployee, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng PortfolioEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "PortfolioEmployee",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<PortfolioEmployeeDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<PortfolioEmployeeDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(PortfolioEmployeeSqlQueries.QueryHidePortfolioEmployee,
                new { IsHide = isLock, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng PortfolioEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "PortfolioEmployee",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<PortfolioEmployeeDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    #endregion
}