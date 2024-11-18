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

public class WorkFlowHistoryRepository : IWorkFlowHistoryRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public WorkFlowHistoryRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ WorkflowStepRepository Methods ]==================================================

    public async Task<TemplateApi<WorkflowHistoryDto>> GetWorkFlowHistoryByIdInstance(Guid idWorkFLowInstance,
        int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var idsWorkflowInstance = new List<Guid>() { idWorkFLowInstance };
        var workflowHistories = (await connection.QueryAsync<WorkflowHistory>(
                WorkflowHistorySqlQueries.QueryGetWorkflowHistoriesByIdWorkFlowInstance,
                new { IdWorkFlowInstance = idsWorkflowInstance }))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            workflowHistories.Select(u => u.Adapt<WorkflowHistoryDto>()),
            workflowHistories.Count);
    }

    #endregion
}