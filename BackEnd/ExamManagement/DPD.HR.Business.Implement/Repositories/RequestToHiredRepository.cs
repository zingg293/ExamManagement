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

public class RequestToHiredRepository : IRequestToHiredRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public RequestToHiredRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ RequestToHiredRepository Methods ]==================================================

    /// <summary>
    /// this function to get the list data from RequestToHired table in the database
    /// </summary>
    /// <param name="pageNumber">this param that received from the controller</param>
    /// <param name="pageSize">this param that received from the controller</param>
    /// <param name="idUnit">this param that received from the controller</param>
    /// <param name="idCategoryVacancies">this param that received from the controller</param>
    /// <returns></returns>
    public async Task<TemplateApi<RequestToHiredDto>> GetListRequestToHired(int pageNumber, int pageSize, Guid? idUnit,
        Guid? idCategoryVacancies)
    {
        //initializing the connection to the database 
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //using Dapper library to call the query and map it to class
        var requestToHired =
            (await connection.QueryAsync<RequestToHired>(RequestToHiredSqlQueries.QueryGetAllRequestToHired))
            .ToList();

        if (idUnit is not null)
        {
            requestToHired = requestToHired.Where(e => e.IdUnit == idUnit).ToList();
        }

        if (idCategoryVacancies is not null)
        {
            requestToHired = requestToHired.Where(e => e.IdCategoryVacancies == idCategoryVacancies).ToList();
        }

        //calling function pagination to handle request
        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            requestToHired.Select(u => u.Adapt<RequestToHiredDto>()),
            requestToHired.Count);
    }

    /// <summary>
    /// this function to get the list data from RequestToHired table in the database
    /// </summary>
    /// <param name="pageNumber">this param that received from the controller</param>
    /// <param name="pageSize">this param that received from the controller</param>
    /// <returns></returns>
    public async Task<TemplateApi<RequestToHiredDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        //initializing the connection to the database 
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //using Dapper library to call the query and map it to class
        var requestToHired =
            (await connection.QueryAsync<RequestToHired>(RequestToHiredSqlQueries.QueryGetAllRequestToHired))
            .ToList();

        //calling function pagination to handle request
        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            requestToHired.Select(u => u.Adapt<RequestToHiredDto>()),
            requestToHired.Count);
    }

    /// <summary>
    /// this service to handle the request its purpose to get data by id 
    /// </summary>
    /// <param name="idRequestToHire"></param>
    /// <returns></returns>
    public async Task<RequestToHired> GetDataById(Guid idRequestToHire)
    {
        //initializing the connection to the database 
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //using Dapper library to call the query and map it to class
        var requestToHired = await connection.QueryFirstOrDefaultAsync<RequestToHired>(
            RequestToHiredSqlQueries.QueryGetByIdRequestToHired, new { Id = idRequestToHire });

        return requestToHired;
    }

    /// <summary>
    /// this service to handle and get RequestToHired and WorkFlowInstance, WorkFlowStep, the relevant information about RequestToHired
    /// </summary>
    /// <param name="idUserCurrent"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<TemplateApi<RequestToHiredDto>> GetRequestToHireAndWorkFlow(Guid idUserCurrent, int pageNumber,
        int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all data in requestToHired table
        var requestToHired =
            (await connection.QueryAsync<RequestToHired>(RequestToHiredSqlQueries.QueryGetAllRequestToHired))
            .ToList().Adapt<List<RequestToHiredDto>>();

        //get all data by id user in role table
        var roles = (await connection.QueryAsync<Role>(RoleSqlQueries.GetRoleByIdUser, new { IdUser = idUserCurrent }))
            .ToList();

        //checking for the records created by user is requesting if have any data then filtering it by id requesting user  
        //otherwise getting default data by UnitId and RoleId
        var checkHaveAnyByCreateBy =
            requestToHired.Exists(e => e.CreatedBy == idUserCurrent) && !roles.Exists(e => e.IsAdmin);
        if (checkHaveAnyByCreateBy)
        {
            requestToHired = requestToHired.Where(e => e.CreatedBy == idUserCurrent).ToList();
        }

        //getting all data if requesting user is admin
        bool isAdminOrUserRequest = roles.Exists(e => e.IsAdmin) || checkHaveAnyByCreateBy;
        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.RequestToHired });

        var filteredRequestToHired = await new WorkFlowRepository(_configuration).FilterAndSortItemData(connection,
            requestToHired,
            idUserCurrent, isAdminOrUserRequest, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredRequestToHired,
            filteredRequestToHired.Count);
    }

    /// <summary>
    /// getting RequestToHired information and its histories step when dealing in work flow
    /// </summary>
    /// <param name="idUserCurrent"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<TemplateApi<RequestToHiredDto>> GetRequestToHireAndWorkFlowByIdUserApproved(Guid idUserCurrent,
        int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all requestToHired
        var requestToHired =
            (await connection.QueryAsync<RequestToHired>(RequestToHiredSqlQueries.QueryGetAllRequestToHired))
            .ToList().Adapt<List<RequestToHiredDto>>();

        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.RequestToHired });

        var filteredRequestToHired = await new WorkFlowRepository(_configuration).FilterAndSortItemDataHistories(
            connection,
            requestToHired,
            idUserCurrent, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredRequestToHired,
            filteredRequestToHired.Count);
    }

    /// <summary>
    /// getting data from the RequestToHired table in the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TemplateApi<RequestToHiredDto>> GetById(Guid id)
    {
        //initializing the connection to the database 
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //getting requestToHired by id
        var requestToHired = await connection.QueryFirstOrDefaultAsync<RequestToHired>(
            RequestToHiredSqlQueries.QueryGetByIdRequestToHired, new { Id = id });

        //getting categoryVacancies information by the id of the requestToHired table
        var categoryVacancies =
            await connection.QueryFirstOrDefaultAsync<CategoryVacancies>(
                CategoryVacanciesSqlQueries.QueryCategoryVacanciesByIdRequestToHire, new { Id = id });

        //initialing list id 
        var idsItemId = new List<Guid>() { id };

        //getting workFlowInstance by itemId
        var workflowInstances =
            (await connection.QueryAsync<WorkflowInstances>(
                WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByItemId, new { ItemId = idsItemId }))
            .ToList();

        //setting above data that received from the database to the properties in the RequestToHiredDto class
        var requestToHiredDto = requestToHired.Adapt<RequestToHiredDto>();
        requestToHiredDto.CategoryVacancies = categoryVacancies;
        requestToHiredDto.WorkflowInstances = workflowInstances;

        return new Pagination().HandleGetByIdRespond(requestToHiredDto);
    }

    /// <summary>
    /// getting the value that's match by conditon
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<TemplateApi<RequestToHiredDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// this service to update information in the requestToHired table
    /// </summary>
    /// <param name="model"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <returns></returns>
    public async Task<TemplateApi<RequestToHiredDto>> Update(RequestToHiredDto model, Guid idUserCurrent,
        string fullName)
    {
        //initializing the connection to the database 
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        //beginning the transaction when dealing with the data in the database
        using var tran = connection.BeginTransaction();

        try
        {
            if (model.Quantity < 0)
            {
                return new TemplateApi<RequestToHiredDto>(null, null, "Số lượng không là số âm !", false, 0, 0, 0, 0);
            }

            //initialing list id 
            var idsItemId = new List<Guid>() { model.Id };
            //getting workFlowInstance by itemId
            var workflowInstances =
                await connection.QueryFirstOrDefaultAsync<WorkflowInstances>(
                    WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByItemId, new { ItemId = idsItemId }, tran);

            //if the workflowInstances is null and requestToHired information that contained in the work flow 
            //and then it can not be updated
            if (workflowInstances != null && workflowInstances.CurrentStep != 0)
            {
                return new TemplateApi<RequestToHiredDto>(null, null, "Phiếu đã đi vào quy trình không thể chỉnh sửa !",
                    false, 0,
                    0, 0, 0);
            }

            //mapping data from dto to entity using the Mapster library
            var requestToHired = model.Adapt<RequestToHired>();

            //getting data by id
            var requestToHiredById = await connection.QueryFirstOrDefaultAsync<RequestToHired>(
                RequestToHiredSqlQueries.QueryGetByIdRequestToHired, new { Id = model.Id }, tran);

            if (model.IdFile is not null)
            {
                requestToHired.FilePath = requestToHiredById.FilePath;
            }

            //executing the command to update the information of requestToHired table
            await connection.ExecuteAsync(RequestToHiredSqlQueries.QueryUpdateRequestToHired, requestToHired, tran);

            //initialing the data of diary table
            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng RequestToHired",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "RequestToHired",
                IsSuccess = true,
                WithId = model.Id
            };

            //executing the command to save the information of diary table
            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<RequestToHiredDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    /// <summary>
    /// using to save the information to the requestToHired table
    /// </summary>
    /// <param name="model"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <returns></returns>
    public async Task<TemplateApi<RequestToHiredDto>> Insert(RequestToHiredDto model, Guid idUserCurrent,
        string fullName)
    {
        //initializing the connection to the database 
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        //beginning the transaction when dealing with the data in the database
        using var tran = connection.BeginTransaction();

        try
        {
            if (model.Quantity < 0)
            {
                return new TemplateApi<RequestToHiredDto>(null, null, "Số lượng không là số âm !", false, 0, 0, 0, 0);
            }

            // getting user by id
            var user = await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.UserById,
                new { Id = idUserCurrent }, tran);
            model.IdUnit = user.UnitId;

            #region RequestToHired

            //executing the command to save the information of requestToHired table
            await connection.ExecuteAsync(RequestToHiredSqlQueries.QueryInsertRequestToHired, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng RequestToHired",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "RequestToHired",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<RequestToHiredDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    /// <summary>
    /// using to delete by id with the requestToHired table
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <returns></returns>
    public async Task<TemplateApi<RequestToHiredDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        //initializing the connection to the database 
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        //beginning the transaction when dealing with the data in the database
        using var tran = connection.BeginTransaction();

        try
        {
            //getting data by id
            var workflowInstances =
                (await connection.QueryAsync<WorkflowInstances>(
                    WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByItemId, new { ItemId = ids }, tran))
                .ToList();

            //getting value from the WorkFlowHistory table by IdWorkFlowInstance
            var workflowHistories =
                (await connection.QueryAsync<WorkflowHistory>(
                    WorkflowHistorySqlQueries.QueryGetWorkflowHistoriesByIdWorkFlowInstance,
                    new { IdWorkFlowInstance = workflowInstances.Select(e => e.Id) },
                    tran))
                .ToList();

            //if the information of requestToHired that contained in the work flow then it can not be deleted
            if (workflowHistories.Count > 1)
            {
                return new TemplateApi<RequestToHiredDto>(null, null, "Phiếu đã đi vào qui trình không thể xóa !",
                    false, 0,
                    0, 0, 0);
            }

            //if the data can be deleted and then we will delete data in the workFlowHistory, workFlowInstance, requestToHired
            //before we delete data from its table, we will insert its data to the deleted_Table

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

            #region RequestToHired

            var requestToHired =
                (await connection.QueryAsync<RequestToHired>(RequestToHiredSqlQueries.QueryGetRequestToHiredByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(RequestToHiredSqlQueries.QueryInsertRequestToHiredDeleted, requestToHired,
                tran);

            await connection.ExecuteAsync(RequestToHiredSqlQueries.QueryDeleteRequestToHired, new { Ids = ids }, tran);

            diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng RequestToHired",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "RequestToHired",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<RequestToHiredDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    /// <summary>
    /// using to hide the information in the requestToHIRED TABLE
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="isLock"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<TemplateApi<RequestToHiredDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion
}