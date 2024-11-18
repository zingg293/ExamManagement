using System.Data;
using System.Globalization;
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

public class OnLeaveRepository : IOnLeaveRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public OnLeaveRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ OnLeaveRepository Methods ]==================================================

    public async Task<TemplateApi<OnLeaveDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var onLeave = (await connection.QueryAsync<OnLeave>(OnLeaveSqlQueries.QueryGetAllOnLeave))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            onLeave.Select(u => u.Adapt<OnLeaveDto>()),
            onLeave.Count);
    }

    public async Task<TemplateApi<OnLeaveDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<OnLeave>(
            OnLeaveSqlQueries.QueryGetByIdOnLeave, new { Id = id });

        var result = unit.Adapt<OnLeaveDto>();

        return new Pagination().HandleGetByIdRespond(result);
    }

    public Task<TemplateApi<OnLeaveDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<OnLeaveDto>> Update(OnLeaveDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var idsItemId = new List<Guid>() { model.Id };
            var workflowInstances =
                await connection.QueryFirstOrDefaultAsync<WorkflowInstances>(
                    WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByItemId, new { ItemId = idsItemId }, tran);

            if (workflowInstances != null && workflowInstances.CurrentStep != 0)
            {
                return new TemplateApi<OnLeaveDto>(null, null, "Phiếu đã đi vào quy trình không thể chỉnh sửa !",
                    false, 0,
                    0, 0, 0);
            }

            var onLeave = model.Adapt<OnLeave>();

            //getting data by id
            var onLeaveById = await connection.QueryFirstOrDefaultAsync<OnLeave>(
                OnLeaveSqlQueries.QueryGetByIdOnLeave, new { Id = model.Id }, tran);

            if (model.IdFile is not null)
            {
                onLeave.Attachments = onLeaveById.Attachments;
            }

            await connection.ExecuteAsync(OnLeaveSqlQueries.QueryUpdateOnLeave, onLeave, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng OnLeave",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "OnLeave",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<OnLeaveDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<OnLeaveDto>> Insert(OnLeaveDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(OnLeaveSqlQueries.QueryInsertOnLeave, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng OnLeave",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "OnLeave",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<OnLeaveDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<OnLeaveDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var workflowInstances =
                (await connection.QueryAsync<WorkflowInstances>(
                    WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByItemId, new { ItemId = ids }, tran))
                .ToList();

            var workflowHistories =
                (await connection.QueryAsync<WorkflowHistory>(
                    WorkflowHistorySqlQueries.QueryGetWorkflowHistoriesByIdWorkFlowInstance,
                    new { IdWorkFlowInstance = workflowInstances.Select(e => e.Id) },
                    tran))
                .ToList();

            if (workflowHistories.Count > 1)
            {
                return new TemplateApi<OnLeaveDto>(null, null, "Phiếu đã đi vào qui trình không thể xóa !",
                    false, 0,
                    0, 0, 0);
            }

            #region WorkflowHistory

            await connection.ExecuteAsync(WorkflowHistorySqlQueries.QueryInsertWorkflowHistoryDeleted,
                workflowHistories,
                tran);

            await connection.ExecuteAsync(WorkflowHistorySqlQueries.QueryDeleteWorkflowHistory,
                new { Ids = workflowHistories.Select(e => e.Id) }, tran);

            var diaries = workflowHistories.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng WorkflowHistory",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "WorkflowHistory",
                IsSuccess = true,
                WithId = id.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            #region WorkflowInstances

            await connection.ExecuteAsync(WorkflowInstancesSqlQueries.QueryInsertWorkflowInstancesDeleted,
                workflowInstances,
                tran);

            await connection.ExecuteAsync(WorkflowInstancesSqlQueries.QueryDeleteWorkflowInstances,
                new { Ids = workflowInstances.Select(e => e.Id) }, tran);

            diaries = workflowHistories.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng WorkflowInstances",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "WorkflowInstances",
                IsSuccess = true,
                WithId = id.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            #region OnLeave

            var onLeaves =
                (await connection.QueryAsync<OnLeave>(OnLeaveSqlQueries.QueryGetOnLeaveByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(OnLeaveSqlQueries.QueryInsertOnLeaveDeleted, onLeaves,
                tran);

            await connection.ExecuteAsync(OnLeaveSqlQueries.QueryDeleteOnLeave, new { Ids = ids }, tran);

            diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng OnLeave",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "OnLeave",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<OnLeaveDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<OnLeaveDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<OnLeaveDto> GetDataById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<OnLeave>(
            OnLeaveSqlQueries.QueryGetByIdOnLeave, new { Id = id });

        return unit.Adapt<OnLeaveDto>();
    }

    public async Task<TemplateApi<OnLeaveDto>> GetOnLeaveAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all data in OnLeave table
        var onLeave =
            (await connection.QueryAsync<OnLeave>(OnLeaveSqlQueries.QueryGetAllOnLeave))
            .ToList().Adapt<List<OnLeaveDto>>();

        //get all data by id user in role table
        var roles = (await connection.QueryAsync<Role>(RoleSqlQueries.GetRoleByIdUser, new { IdUser = idUserCurrent }))
            .ToList();

        //checking for the records created by user is requesting if have any data then filtering it by id requesting user  
        //otherwise getting default data by UnitId and RoleId
        var checkHaveAnyByCreateBy =
            onLeave.Exists(e => e.IdUserRequest == idUserCurrent) && !roles.Exists(e => e.IsAdmin);
        if (checkHaveAnyByCreateBy)
        {
            onLeave = onLeave.Where(e => e.IdUserRequest == idUserCurrent).ToList();
        }

        //getting all data if requesting user is admin
        bool isAdminOrUserRequest = roles.Exists(e => e.IsAdmin) || checkHaveAnyByCreateBy;
        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.OnLeave });

        var filteredOnLeave = await new WorkFlowRepository(_configuration).FilterAndSortItemData(connection,
            onLeave,
            idUserCurrent, isAdminOrUserRequest, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredOnLeave,
            filteredOnLeave.Count);
    }

    public async Task<TemplateApi<OnLeaveDto>> GetOnLeaveAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber,
        int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all OnLeave
        var onLeave =
            (await connection.QueryAsync<OnLeave>(OnLeaveSqlQueries.QueryGetAllOnLeave))
            .ToList().Adapt<List<OnLeaveDto>>();

        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.OnLeave });

        var filteredOnLeave = await new WorkFlowRepository(_configuration).FilterAndSortItemDataHistories(
            connection,
            onLeave,
            idUserCurrent, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredOnLeave,
            filteredOnLeave.Count);
    }

    public async Task<TemplateApi<OnLeaveDto>> FilterOnLeave(FilterOnLeaveModel model, int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var onLeave = (await connection.QueryAsync<OnLeave>(OnLeaveSqlQueries.QueryGetAllOnLeave))
            .ToList();

        if (model.IdEmployee is not null)
        {
            onLeave = onLeave.Where(e => e.IdEmployee == model.IdEmployee).ToList();
        }

        if (model.FromDate is not null && model.ToDate is not null)
        {
            DateTime startDay = DateTime.ParseExact(model.FromDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime endDay = DateTime.ParseExact(model.ToDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            onLeave = onLeave.Where(e => e.FromDate >= startDay && e.ToDate <= endDay).ToList();
        }

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            onLeave.Select(u => u.Adapt<OnLeaveDto>()),
            onLeave.Count);
    }
    #endregion

}