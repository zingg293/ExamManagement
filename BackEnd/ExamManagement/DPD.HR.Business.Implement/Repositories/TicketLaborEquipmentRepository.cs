using System.Data;
using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DPD.HR.Application.Implement.Repositories;

public class TicketLaborEquipmentRepository : ITicketLaborEquipmentRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public TicketLaborEquipmentRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ TicketLaborEquipmentRepository Methods ]==============================

    public async Task<TemplateApi<TicketLaborEquipmentDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var ticketLaborEquipments = (await connection.QueryAsync<TicketLaborEquipment>(TicketLaborEquipmentSqlQueries
                .QueryGetAllTicketLaborEquipment))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            ticketLaborEquipments.Select(u => u.Adapt<TicketLaborEquipmentDto>()),
            ticketLaborEquipments.Count);
    }

    public async Task<TemplateApi<TicketLaborEquipmentDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var ticketLaborEquipments = await connection.QueryFirstOrDefaultAsync<TicketLaborEquipment>(
            TicketLaborEquipmentSqlQueries.QueryGetByIdTicketLaborEquipment, new { Id = id });

        var workflowInstance = (await connection.QueryAsync<WorkflowInstances>(
                WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByItemId,
                new { ItemId = new List<Guid>() { id } }))
            .ToList();

        var ticketLaborEquipmentDetails = (await connection.QueryAsync<TicketLaborEquipmentDetail>(
                TicketLaborEquipmentDetailSqlQueries
                    .QueryTicketLaborEquipmentDetailByIdTicketLaborEquipment,
                new { IdTicketLaborEquipment = id }))
            .ToList();

        var data = ticketLaborEquipments.Adapt<TicketLaborEquipmentDto>();
        data.TicketLaborEquipmentDetail = ticketLaborEquipmentDetails.Adapt<List<TicketLaborEquipmentDetailDto>>();
        data.WorkflowInstances = workflowInstance;

        return new Pagination().HandleGetByIdRespond(data);
    }

    public Task<TemplateApi<TicketLaborEquipmentDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<TicketLaborEquipmentDto>> Update(TicketLaborEquipmentDto model, Guid idUserCurrent,
        string fullName)
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
                return new TemplateApi<TicketLaborEquipmentDto>(null, null,
                    "Phiếu đã đi vào quy trình không thể chỉnh sửa !",
                    false, 0,
                    0, 0, 0);
            }

            var ticketLaborEquipment = model.Adapt<TicketLaborEquipment>();

            var ticketLaborEquipmentById = await connection.QueryFirstOrDefaultAsync<TicketLaborEquipment>(
                TicketLaborEquipmentSqlQueries.QueryGetByIdTicketLaborEquipment, new { Id = model.Id }, tran);

            if (model.IdFile is not null)
            {
                ticketLaborEquipment.FileAttachment = ticketLaborEquipmentById.FileAttachment;
            }

            await connection.ExecuteAsync(TicketLaborEquipmentSqlQueries.QueryUpdateTicketLaborEquipment,
                ticketLaborEquipment, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng TicketLaborEquipment",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "TicketLaborEquipment",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<TicketLaborEquipmentDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<TicketLaborEquipmentDto>> Insert(TicketLaborEquipmentDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var user = await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.UserById,
                new { Id = idUserCurrent }, tran);
            model.IdUnit = user.UnitId;
            model.IdUserRequest = user.Id;

            await connection.ExecuteAsync(TicketLaborEquipmentSqlQueries.QueryInsertTicketLaborEquipment, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng TicketLaborEquipment",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "TicketLaborEquipment",
                IsSuccess = true,
                WithId = model.Id
            };
            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<TicketLaborEquipmentDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<TicketLaborEquipmentDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
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

            if (workflowInstances.Any(e => e.CurrentStep != 0))
            {
                return new TemplateApi<TicketLaborEquipmentDto>(null, null, "Phiếu đã đi vào qui trình không thể xóa !",
                    false, 0,
                    0, 0, 0);
            }

            #region WorkflowHistory

            var workflowHistories =
                (await connection.QueryAsync<WorkflowHistory>(
                    WorkflowHistorySqlQueries.QueryGetWorkflowHistoriesByIdWorkFlowInstance,
                    new { IdWorkFlowInstance = workflowInstances.Select(e => e.Id) },
                    tran))
                .ToList();

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

            #region TicketLaborEquipment

            var ticketLaborEquipments =
                (await connection.QueryAsync<TicketLaborEquipment>(
                    TicketLaborEquipmentSqlQueries.QueryGetTicketLaborEquipmentByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(TicketLaborEquipmentSqlQueries.QueryInsertTicketLaborEquipmentDeleted,
                ticketLaborEquipments, tran);

            await connection.ExecuteAsync(TicketLaborEquipmentSqlQueries.QueryDeleteTicketLaborEquipment,
                new { Ids = ids }, tran);

            diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng TicketLaborEquipment",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "TicketLaborEquipment",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<TicketLaborEquipmentDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<TicketLaborEquipmentDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TicketLaborEquipment> GetDataById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var ticketLaborEquipments = await connection.QueryFirstOrDefaultAsync<TicketLaborEquipment>(
            TicketLaborEquipmentSqlQueries.QueryGetByIdTicketLaborEquipment, new { Id = id });

        return ticketLaborEquipments;
    }

    public async Task<TemplateApi<TicketLaborEquipmentDto>> GetTicketLaborEquipmentAndWorkFlow(Guid idUserCurrent,
        int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all data in TicketLaborEquipment table
        var ticketLaborEquipment =
            (await connection.QueryAsync<TicketLaborEquipment>(TicketLaborEquipmentSqlQueries
                .QueryGetAllTicketLaborEquipment))
            .ToList().Adapt<List<TicketLaborEquipmentDto>>();

        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.TicketLaborEquipment });

        //get all data by id user in role table
        var roles = (await connection.QueryAsync<Role>(RoleSqlQueries.GetRoleByIdUser, new { IdUser = idUserCurrent }))
            .ToList();

        //checking for the records created by user is requesting if have any data then filtering it by id requesting user  
        //otherwise getting default data by UnitId and RoleId
        var checkHaveAnyByCreateBy =
            ticketLaborEquipment.Exists(e => e.IdUserRequest == idUserCurrent) && !roles.Exists(e => e.IsAdmin);
        if (checkHaveAnyByCreateBy)
        {
            ticketLaborEquipment = ticketLaborEquipment.Where(e => e.IdUserRequest == idUserCurrent).ToList();
        }

        //getting all data if requesting user is admin
        bool isAdminOrUserRequest = roles.Exists(e => e.IsAdmin) || checkHaveAnyByCreateBy;

        var filteredTicketLaborEquipment = await new WorkFlowRepository(_configuration).FilterAndSortItemData(
            connection,
            ticketLaborEquipment,
            idUserCurrent, isAdminOrUserRequest, workflowTemplate.Id);

        await AssignDetailToTicket(connection, filteredTicketLaborEquipment);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredTicketLaborEquipment,
            filteredTicketLaborEquipment.Count);
    }

    private static async Task AssignDetailToTicket(IDbConnection connection,
        List<TicketLaborEquipmentDto> filteredTicketLaborEquipment)
    {
        // getting all data in TicketLaborEquipmentDetail table
        var ticketLaborEquipmentDetails = (await connection.QueryAsync<TicketLaborEquipmentDetail>(
                TicketLaborEquipmentDetailSqlQueries
                    .QueryGetAllTicketLaborEquipmentDetail))
            .ToList().Adapt<List<TicketLaborEquipmentDetailDto>>();

        foreach (var item in filteredTicketLaborEquipment)
        {
            item.TicketLaborEquipmentDetail =
                ticketLaborEquipmentDetails.Where(e => e.IdTicketLaborEquipment == item.Id).ToList();
        }
    }

    public async Task<TemplateApi<TicketLaborEquipmentDto>> GetTicketLaborEquipmentAndWorkFlowByIdUserApproved(
        Guid idUserCurrent, int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all ticketLaborEquipment
        var ticketLaborEquipment =
            (await connection.QueryAsync<TicketLaborEquipment>(TicketLaborEquipmentSqlQueries
                .QueryGetAllTicketLaborEquipment))
            .ToList().Adapt<List<TicketLaborEquipmentDto>>();

        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.TicketLaborEquipment });

        var filteredRequestToHired = await new WorkFlowRepository(_configuration).FilterAndSortItemDataHistories(
            connection,
            ticketLaborEquipment,
            idUserCurrent, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredRequestToHired,
            filteredRequestToHired.Count);
    }

    #endregion
}