using System.Data;
using System.Globalization;
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

public class EmployeeDayOffRepository : IEmployeeDayOffRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public EmployeeDayOffRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ EmployeeDayOffRepository Methods ]==================================================

    public async Task<TemplateApi<EmployeeDayOffDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<EmployeeDayOff>(EmployeeDayOffSqlQueries.QueryGetAllEmployeeDayOff))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<EmployeeDayOffDto>()),
            units.Count);
    }

    public async Task<TemplateApi<EmployeeDayOffDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<EmployeeDayOff>(
            EmployeeDayOffSqlQueries.QueryGetByIdEmployeeDayOff, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<EmployeeDayOffDto>());
    }

    public Task<TemplateApi<EmployeeDayOffDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<EmployeeDayOffDto>> Update(EmployeeDayOffDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var employeeDayOff = model.Adapt<EmployeeDayOff>();
            await connection.ExecuteAsync(EmployeeDayOffSqlQueries.QueryUpdateEmployeeDayOff, employeeDayOff, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng EmployeeDayOff",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "EmployeeDayOff",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<EmployeeDayOffDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<EmployeeDayOffDto>> Insert(EmployeeDayOffDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var employeeDayOffByDate =
                (await connection.QueryAsync<EmployeeDayOff>(
                    EmployeeDayOffSqlQueries.QueryEmployeeDayOffByDate,
                    new
                    {
                        DayOff = model.DayOff?.ToString("dd/MM/yyyy"),
                        IdEmployee = model.IdEmployee
                    }, tran)).ToList();

            if (employeeDayOffByDate.Any())
            {
                var check = false;

                check = employeeDayOffByDate.Any(e => e.TypeOfDayOff == model.TypeOfDayOff);

                if (check)
                {
                    return new TemplateApi<EmployeeDayOffDto>(null, null,
                        "Nhân viên này đã đăng kí nghỉ hoặc tăng ca vào thời gian này !", false, 0, 0, 0, 0);
                }
            }

            await connection.ExecuteAsync(EmployeeDayOffSqlQueries.QueryInsertEmployeeDayOff, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng EmployeeDayOff",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "EmployeeDayOff",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<EmployeeDayOffDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<EmployeeDayOffDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var employeeDayOffs =
                (await connection.QueryAsync<EmployeeDayOff>(EmployeeDayOffSqlQueries.QueryEmployeeDayOffByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(EmployeeDayOffSqlQueries.QueryInsertEmployeeDayOffDeleted, employeeDayOffs,
                tran);

            await connection.ExecuteAsync(EmployeeDayOffSqlQueries.QueryDeleteEmployeeDayOff, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng EmployeeDayOff",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "EmployeeDayOff",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<EmployeeDayOffDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<EmployeeDayOffDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion
}