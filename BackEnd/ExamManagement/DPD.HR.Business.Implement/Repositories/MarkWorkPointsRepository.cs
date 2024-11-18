using System.Data;
using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DPD.HR.Application.Implement.Repositories;

public class MarkWorkPointsRepository : IMarkWorkPointsRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public MarkWorkPointsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ MarkWorkPointsRepository ]=================================================================

    public async Task<TemplateApi<MarkWorkPointsDto>> MarkWorkPointsEmployee(FilterMarkWorkPointsModel model)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var markWorkPointsDto = new MarkWorkPointsDto();

        //getting employee by id
        var employee =
            await connection.QueryFirstOrDefaultAsync<Employee>(EmployeeSqlQueries.QueryGetByIdEmployee,
                new { Id = model.IdEmployee });

        if (employee is null)
        {
            return new Pagination().HandleGetByIdRespond(markWorkPointsDto);
        }

        var onLeaves = await GetOnLeaves(model, connection);

        var overtimes = await GetOvertimes(model, connection);

        markWorkPointsDto.Employee = employee;
        markWorkPointsDto.OnLeaves = onLeaves.Count > 0 ? onLeaves : null;
        markWorkPointsDto.Overtimes = overtimes.Count > 0 ? overtimes : null;

        return new Pagination().HandleGetByIdRespond(markWorkPointsDto);
    }

    private static async Task<List<Overtime>> GetOvertimes(FilterMarkWorkPointsModel model, IDbConnection connection)
    {
        var month = model.FromDate?.Split('/')[0];
        var year = model.FromDate?.Split('/')[1];

        var overtimes = (await connection.QueryAsync<Overtime>(OvertimeSqlQueries.QueryGetOvertimeByEmployeeByCondition,
                new
                {
                    WorkflowCode = AppSettings.Overtime, Month = month, Year = year,
                    IdEmployee = model.IdEmployee
                }))
            .ToList();
        return overtimes;
    }

    private static async Task<List<OnLeave>> GetOnLeaves(FilterMarkWorkPointsModel model, IDbConnection connection)
    {
        var month = model.FromDate?.Split('/')[0];
        var year = model.FromDate?.Split('/')[1];

        var onLeaves = (await connection.QueryAsync<OnLeave>(OnLeaveSqlQueries.QueryGetOnLeaveByEmployeeByCondition,
                new
                {
                    WorkflowCode = AppSettings.OnLeave, Month = month, Year = year,
                    IdEmployee = model.IdEmployee
                }))
            .ToList();
        return onLeaves;
    }

    public async Task<MarkWorkPointsDto> MarkWorkPointsEmployeeForExcel(FilterMarkWorkPointsModel model)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var markWorkPointsDto = new MarkWorkPointsDto();

        //getting employee by id
        var employee =
            await connection.QueryFirstOrDefaultAsync<Employee>(EmployeeSqlQueries.QueryGetByIdEmployee,
                new { Id = model.IdEmployee });

        if (employee is null)
        {
            return markWorkPointsDto;
        }

        var onLeaves = await GetOnLeaves(model, connection);

        var overtimes = await GetOvertimes(model, connection);

        markWorkPointsDto.Employee = employee;
        markWorkPointsDto.OnLeaves = onLeaves.Count > 0 ? onLeaves : null;
        markWorkPointsDto.Overtimes = overtimes.Count > 0 ? overtimes : null;

        return markWorkPointsDto;
    }

    #endregion
}