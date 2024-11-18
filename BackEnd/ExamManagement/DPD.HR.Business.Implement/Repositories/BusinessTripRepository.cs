using System.Data;
using System.Globalization;
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

public class BusinessTripRepository : IBusinessTripRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public BusinessTripRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ BusinessTripRepository Methods ]==================================================

    public async Task<TemplateApi<BusinessTripDto>> GetListBusinessTrip(int pageNumber, int pageSize, Guid? idUnit,
        string? startDate, string? endDate)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<BusinessTrip>(BusinessTripSqlQueries.QueryGetAllBusinessTrip))
            .ToList();

        if (idUnit is not null)
        {
            units = units.Where(e => e.IdUnit == idUnit).ToList();
        }

        if (startDate is not null)
        {
            var startDay =
                DateTime.ParseExact(startDate, "dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("vi-VN"));
            units = units.Where(e => e.StartDate >= startDay).ToList();
        }

        if (endDate is not null)
        {
            var endDay = DateTime.ParseExact(endDate, "dd/MM/yyyy HH:mm",
                CultureInfo.GetCultureInfo("vi-VN"));
            units = units.Where(e => e.EndDate <= endDay).ToList();
        }

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<BusinessTripDto>()),
            units.Count);
    }

    public async Task<BusinessTrip> GetDataById(Guid idBusinessTrip)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<BusinessTrip>(
            BusinessTripSqlQueries.QueryGetByIdBusinessTrip, new { Id = idBusinessTrip });

        return unit;
    }

    public async Task<TemplateApi<BusinessTripDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<BusinessTrip>(BusinessTripSqlQueries.QueryGetAllBusinessTrip))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<BusinessTripDto>()),
            units.Count);
    }

    public async Task<TemplateApi<BusinessTripDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<BusinessTrip>(
            BusinessTripSqlQueries.QueryGetByIdBusinessTrip, new { Id = id });

        var businessTripEmployees = (await connection.QueryAsync<BusinessTripEmployee>(BusinessTripEmployeeSqlQueries
                .QueryGetBusinessTripEmployeeByIdBusinessTrip, new { IdBusinessTrip = id }))
            .ToList();

        var result = unit.Adapt<BusinessTripDto>();
        result.BusinessTripEmployees = businessTripEmployees;

        return new Pagination().HandleGetByIdRespond(result);
    }

    public Task<TemplateApi<BusinessTripDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<BusinessTripDto>> Update(BusinessTripDto model, Guid idUserCurrent, string fullName)
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
                return new TemplateApi<BusinessTripDto>(null, null, "Phiếu đã đi vào quy trình không thể chỉnh sửa !",
                    false, 0,
                    0, 0, 0);
            }

            var businessTrip = model.Adapt<BusinessTrip>();
            
            //getting data by id
            var businessTripById = await connection.QueryFirstOrDefaultAsync<BusinessTrip>(
                BusinessTripSqlQueries.QueryGetByIdBusinessTrip, new { Id = model.Id }, tran);

            if (model.IdFile is not null)
            {
                businessTrip.Attachments = businessTripById.Attachments;
            }
            
            await connection.ExecuteAsync(BusinessTripSqlQueries.QueryUpdateBusinessTrip, businessTrip, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng BusinessTrip",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "BusinessTrip",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<BusinessTripDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<BusinessTripDto>> Insert(BusinessTripDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(BusinessTripSqlQueries.QueryInsertBusinessTrip, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng BusinessTrip",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "BusinessTrip",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<BusinessTripDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<BusinessTripDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
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
                return new TemplateApi<BusinessTripDto>(null, null, "Phiếu đã đi vào qui trình không thể xóa !",
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

            #region BusinessTrip

            var businessTrips =
                (await connection.QueryAsync<BusinessTrip>(BusinessTripSqlQueries.QueryGetBusinessTripByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(BusinessTripSqlQueries.QueryInsertBusinessTripDeleted, businessTrips,
                tran);

            await connection.ExecuteAsync(BusinessTripSqlQueries.QueryDeleteBusinessTrip, new { Ids = ids }, tran);

            diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng BusinessTrip",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "BusinessTrip",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<BusinessTripDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<BusinessTripDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion

    public async Task<TemplateApi<BusinessTripDto>> GetBusinessTripAndWorkFlow(Guid idUserCurrent, int pageNumber,
        int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all data in BusinessTrip table
        var businessTrip =
            (await connection.QueryAsync<BusinessTrip>(BusinessTripSqlQueries.QueryGetAllBusinessTrip))
            .ToList().Adapt<List<BusinessTripDto>>();

        //get all data by id user in role table
        var roles = (await connection.QueryAsync<Role>(RoleSqlQueries.GetRoleByIdUser, new { IdUser = idUserCurrent }))
            .ToList();

        //checking for the records created by user is requesting if have any data then filtering it by id requesting user  
        //otherwise getting default data by UnitId and RoleId
        var checkHaveAnyByCreateBy =
            businessTrip.Exists(e => e.IdUserRequest == idUserCurrent) && !roles.Exists(e => e.IsAdmin);
        if (checkHaveAnyByCreateBy)
        {
            businessTrip = businessTrip.Where(e => e.IdUserRequest == idUserCurrent).ToList();
        }

        //getting all data if requesting user is admin
        bool isAdminOrUserRequest = roles.Exists(e => e.IsAdmin) || checkHaveAnyByCreateBy;
        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.BusinessTrip });

        var filteredBusinessTrip = await new WorkFlowRepository(_configuration).FilterAndSortItemData(connection,
            businessTrip,
            idUserCurrent, isAdminOrUserRequest, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredBusinessTrip,
            filteredBusinessTrip.Count);
    }

    public async Task<TemplateApi<BusinessTripDto>> GetBusinessTripAndWorkFlowByIdUserApproved(Guid idUserCurrent,
        int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all BusinessTrip
        var businessTrip =
            (await connection.QueryAsync<BusinessTrip>(BusinessTripSqlQueries.QueryGetAllBusinessTrip))
            .ToList().Adapt<List<BusinessTripDto>>();

        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.BusinessTrip });

        var filteredBusinessTrip = await new WorkFlowRepository(_configuration).FilterAndSortItemDataHistories(
            connection,
            businessTrip,
            idUserCurrent, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredBusinessTrip,
            filteredBusinessTrip.Count);
    }
}