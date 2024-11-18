using System.Data;
using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DPD.HR.Application.Implement.Repositories;

public class WorkFlowRepository : IWorkFlowRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public WorkFlowRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ WorkFlowRepository Methods ]==========================================

    /// <summary>
    /// In the work flows, using this service to update status, name status, current step ... in work flow instances 
    /// </summary>
    /// <param name="idWorkFlowInstance"></param>
    /// <param name="isTerminated"></param>
    /// <param name="isRequestToChange"></param>
    /// <param name="message"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <returns></returns>
    public async Task<TemplateApi<WorkflowTemplateDto>> UpdateStepWorkFlow(Guid idWorkFlowInstance,
        bool isTerminated,
        bool isRequestToChange, string message, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            //getting information of user by id
            var user = await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.UserById,
                new { Id = idUserCurrent }, tran);

            //getting workFlowInstances by id
            var workflowInstances = await connection.QueryFirstOrDefaultAsync<WorkflowInstances>(
                WorkflowInstancesSqlQueries.QueryGetByIdWorkflowInstances, new { Id = idWorkFlowInstance }, tran);

            if (workflowInstances == null)
            {
                return new TemplateApi<WorkflowTemplateDto>(null, null,
                    "Không tồn tại dữ liệu !", false, 0, 0, 0,
                    0);
            }

            //checking CurrentStep to make sure it won't be larger than the number of workflowStep
            if (workflowInstances.IsCompleted)
            {
                return new TemplateApi<WorkflowTemplateDto>(null, null,
                    "Yêu cầu đã hoàn thành không thể chỉnh sửa !", false, 0, 0, 0,
                    0);
            }

            //initializing list guid id template
            var idsTemplateId = new List<Guid>() { workflowInstances.TemplateId };
            //getting workFlowStep by list id TemplateId
            var workflowStep = (await connection.QueryAsync<WorkflowStep>(
                    WorkflowStepSqlQueries.QueryGetWorkflowStepByTemplateId, new { TemplateId = idsTemplateId },
                    tran))
                .OrderBy(e => e.Order).ToList();

            //getting workflowTemplate by id
            var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
                WorkflowTemplateSqlQueries.QueryGetByIdWorkflowTemplate, new { Id = workflowInstances.TemplateId },
                tran);

            if (!workflowStep.Any() || workflowTemplate is null)
            {
                return new TemplateApi<WorkflowTemplateDto>(null, null,
                    "Không tồn tại dữ liệu !", false, 0, 0, 0,
                    0);
            }

            //when user created this record is requesting and CurrentStep of workflowInstances table is equal length of workflowStep,
            //then user can complete work flow
            if (workflowInstances.CreatedBy == idUserCurrent &&
                workflowInstances.CurrentStep == workflowStep.Count && workflowInstances.IsApproved)
            {
                return await CompleteItemInWorkFlow(idWorkFlowInstance, idUserCurrent, fullName, connection, tran,
                    workflowInstances, user, workflowTemplate);
            }

            //when user requests terminating work flow 
            if (isTerminated)
            {
                return await TerminatingItemInWorkFlow(idWorkFlowInstance, message, idUserCurrent, fullName, connection,
                    tran, workflowStep, workflowInstances, user);
            }

            //when user requests changing information of work flow
            if (isRequestToChange)
            {
                return await RequestToChangeItemInWorkFlow(idWorkFlowInstance, message, idUserCurrent, fullName,
                    connection, tran, workflowInstances, workflowStep, user);
            }

            //when the last user in work flow request to approve 
            if (workflowInstances.CurrentStep == workflowStep.Count)
            {
                return await ApproveItemInWorkFlow(idWorkFlowInstance, message, idUserCurrent, fullName, connection,
                    tran, workflowStep, workflowInstances, user);
            }

            return await MoveOnItemInWorkFlow(idWorkFlowInstance, message, idUserCurrent, fullName, workflowInstances,
                workflowStep, connection, tran, user);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    /// <summary>
    /// using this function to move on, increasing current step of item in work flow
    /// </summary>
    /// <param name="idWorkFlowInstance">id in table work flow instance</param>
    /// <param name="message">text that user note in each step</param>
    /// <param name="idUserCurrent">id user is logging in app</param>
    /// <param name="fullName">name user is logging in app</param>
    /// <param name="workflowInstances">data work flow instance table by id</param>
    /// <param name="workflowStep">list data work flow step table by id</param>
    /// <param name="connection">connection to database</param>
    /// <param name="tran">beginning transaction when dealing with database</param>
    /// <param name="user">information user is logging in the app</param>
    /// <returns></returns>
    private static async Task<TemplateApi<WorkflowTemplateDto>> MoveOnItemInWorkFlow(Guid idWorkFlowInstance,
        string message, Guid idUserCurrent,
        string fullName, WorkflowInstances workflowInstances, IReadOnlyCollection<WorkflowStep> workflowStep,
        IDbConnection connection,
        IDbTransaction tran, User user)
    {
        Diary diary;

        #region WorkflowInstances

        //increasing current step of workFlowInstances retrieved by ID
        workflowInstances.CurrentStep++;
        //changing UnitId to IdUnitAssign of WorkflowStep table
        workflowInstances.UnitId = workflowStep.ElementAt(workflowInstances.CurrentStep - 1).IsDirectUnit
            ? user.UnitId
            : workflowStep.ElementAt(workflowInstances.CurrentStep - 1).IdUnitAssign;
        //changing NameStatus to StepName of WorkflowStep table
        workflowInstances.NameStatus = workflowStep.ElementAt(workflowInstances.CurrentStep - 1).StepName;
        workflowInstances.Message = message;
        workflowInstances.IsDraft = false;

        await connection.ExecuteAsync(WorkflowInstancesSqlQueries.QueryUpdateWorkflowInstances,
            workflowInstances, tran);

        diary = new Diary()
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã cập nhật bảng WorkflowInstances",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Cập nhật CSDL",
            Operation = "Update",
            Table = "WorkflowInstances",
            IsSuccess = true,
            WithId = workflowInstances.Id
        };

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

        #endregion

        #region WorkflowHistory

        var workflowHistory = new WorkflowHistory()
        {
            Id = Guid.NewGuid(),
            Status = 0,
            CreatedDate = DateTime.Now,
            IdWorkFlowInstance = idWorkFlowInstance,
            IdUser = idUserCurrent,
            Action = workflowStep.ElementAt(workflowInstances.CurrentStep - 1).StepName,
            IdUnit = user.UnitId,
            IsStepCompleted = false,
            Comment = null,
            Message = message,
            IsCancelled = false,
            IsRequestToChanged = false
        };

        await connection.ExecuteAsync(WorkflowHistorySqlQueries.QueryInsertWorkflowHistory,
            workflowHistory,
            tran);

        diary = new Diary()
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã thêm mới bảng WorkflowHistory",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Thêm mới CSDL",
            Operation = "Create",
            Table = "WorkflowHistory",
            IsSuccess = true,
            WithId = workflowHistory.Id
        };

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

        #endregion

        tran.Commit();

        return new TemplateApi<WorkflowTemplateDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
    }

    /// <summary>
    /// using this function to approve item that current step is equal the lenght of work flow step in the same work flow template
    /// this function is using the params like a above function
    /// </summary>
    /// <param name="idWorkFlowInstance"></param>
    /// <param name="message"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <param name="connection"></param>
    /// <param name="tran"></param>
    /// <param name="workflowStep"></param>
    /// <param name="workflowInstances"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    private static async Task<TemplateApi<WorkflowTemplateDto>> ApproveItemInWorkFlow(Guid idWorkFlowInstance,
        string message, Guid idUserCurrent,
        string fullName, IDbConnection connection, IDbTransaction tran, IReadOnlyCollection<WorkflowStep> workflowStep,
        WorkflowInstances workflowInstances, User user)
    {
        Diary diary;

        #region WorkflowInstances

        //getting workFlowInstance by id
        var workFlowInstance = await connection.QueryFirstOrDefaultAsync<WorkflowInstances>(
            WorkflowInstancesSqlQueries.QueryGetByIdWorkflowInstances, new { Id = idWorkFlowInstance },
            tran);

        //getting outCome field in workflowStep table
        var outCome = workflowStep.ElementAt(workFlowInstance!
            .CurrentStep - 1).OutCome;

        //initializing common data and then update work flow instance
        var updateCommonInformationWorkflowInstances = new WorkflowInstances()
        {
            Id = idWorkFlowInstance,
            IsCompleted = false,
            IsDraft = false,
            IsTerminated = false,
            NameStatus = outCome,
            IsApproved = true,
            Message = message,
            UnitId = null
        };
        //update message to workflow instance
        await connection.ExecuteAsync(
            WorkflowInstancesSqlQueries.QueryUpdateCommonInformationWorkflowInstances,
            updateCommonInformationWorkflowInstances,
            tran);

        diary = new Diary()
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã cập nhật bảng WorkflowInstances",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Cập nhật CSDL",
            Operation = "Update",
            Table = "WorkflowInstances",
            IsSuccess = true,
            WithId = workflowInstances.Id
        };

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

        #endregion

        #region WorkflowHistory

        var workflowHistoryComplete = new WorkflowHistory()
        {
            Id = Guid.NewGuid(),
            Status = 0,
            CreatedDate = DateTime.Now,
            IdWorkFlowInstance = idWorkFlowInstance,
            IdUser = idUserCurrent,
            Action = workflowStep.ElementAt(workflowInstances.CurrentStep - 1).OutCome,
            IdUnit = user.UnitId,
            IsStepCompleted = false,
            Comment = null,
            Message = message,
            IsCancelled = false,
            IsRequestToChanged = false
        };

        await connection.ExecuteAsync(WorkflowHistorySqlQueries.QueryInsertWorkflowHistory,
            workflowHistoryComplete,
            tran);

        diary = new Diary()
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã thêm mới bảng WorkflowHistory",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Thêm mới CSDL",
            Operation = "Create",
            Table = "WorkflowHistory",
            IsSuccess = true,
            WithId = workflowHistoryComplete.Id
        };

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

        #endregion

        tran.Commit();

        return new TemplateApi<WorkflowTemplateDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0,
            0);
    }

    /// <summary>
    /// using this function to complete item that in final step
    /// this function is using the params like a above function
    /// </summary>
    /// <param name="idWorkFlowInstance"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <param name="connection"></param>
    /// <param name="tran"></param>
    /// <param name="workflowInstances"></param>
    /// <param name="user"></param>
    /// <param name="workflowTemplate"></param>
    /// <returns></returns>
    private static async Task<TemplateApi<WorkflowTemplateDto>> CompleteItemInWorkFlow(Guid idWorkFlowInstance,
        Guid idUserCurrent, string fullName,
        IDbConnection connection, IDbTransaction tran, WorkflowInstances workflowInstances, User user,
        WorkflowTemplate workflowTemplate)
    {
        Diary diary;

        #region WorkflowInstance

        await connection.ExecuteAsync(
            WorkflowInstancesSqlQueries.QueryUpdateWorkflowInstancesToComplete,
            new
            {
                IsCompleted = true, IsDraft = false, IsTerminated = false, NameStatus = "Đã hoàn thành",
                Message = "",
                Id = idWorkFlowInstance
            },
            tran);

        diary = new Diary()
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã cập nhật bảng WorkflowInstances",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Cập nhật CSDL",
            Operation = "Update",
            Table = "WorkflowInstances",
            IsSuccess = true,
            WithId = workflowInstances.Id
        };

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

        #endregion

        #region WorkflowHistory

        var workflowHistoryComplete = new WorkflowHistory()
        {
            Id = Guid.NewGuid(),
            Status = 0,
            CreatedDate = DateTime.Now,
            IdWorkFlowInstance = idWorkFlowInstance,
            IdUser = idUserCurrent,
            Action = "Hoàn thành phiếu",
            IdUnit = user.UnitId,
            IsStepCompleted = true,
            Comment = null,
            Message = null,
            IsCancelled = false,
            IsRequestToChanged = false
        };

        await connection.ExecuteAsync(WorkflowHistorySqlQueries.QueryInsertWorkflowHistory,
            workflowHistoryComplete,
            tran);

        diary = new Diary()
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã thêm mới bảng WorkflowHistory",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Thêm mới CSDL",
            Operation = "Create",
            Table = "WorkflowHistory",
            IsSuccess = true,
            WithId = workflowHistoryComplete.Id
        };

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

        #endregion

        #region LaborEquipmentUnit

        await DealWithLaborEquipmentUnitWhenFinishing(idUserCurrent, fullName, connection, tran, workflowInstances,
            workflowTemplate);

        #endregion

        #region PositionEmployee

        await DealWithPositionEmployeeWhenFinishing(idUserCurrent, fullName, connection, tran, workflowInstances,
            workflowTemplate);

        #endregion

        tran.Commit();

        return new TemplateApi<WorkflowTemplateDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0,
            0);
    }

    /// <summary>
    /// this is exception in case user finishes process item in work flow but item is ticket labor equipment
    /// then dealing with labor equipment unit table
    /// including 4 case
    /// case type = 0 that's mean create record to table
    /// case type = 1 that's mean update status to table 
    /// case type = 2 that's mean update status to table 
    /// case type = 3 that's mean update status, idUnit, idEmployee to table 
    /// </summary>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <param name="connection"></param>
    /// <param name="tran"></param>
    /// <param name="workflowInstances"></param>
    /// <param name="workflowTemplate"></param>
    private static async Task DealWithLaborEquipmentUnitWhenFinishing(Guid idUserCurrent, string fullName,
        IDbConnection connection, IDbTransaction tran, WorkflowInstances workflowInstances,
        WorkflowTemplate workflowTemplate)
    {
        List<Diary> diaries;
        // in case complete work flow if it is ticket labor equipment then insert data to laborEquipmentUnit table
        if (workflowTemplate.WorkflowCode == AppSettings.TicketLaborEquipment)
        {
            //getting data from ticketLaborEquipments by id
            var ticketLaborEquipments = await connection.QueryFirstOrDefaultAsync<TicketLaborEquipment>(
                TicketLaborEquipmentSqlQueries.QueryGetByIdTicketLaborEquipment,
                new { Id = workflowInstances.ItemId },
                tran);

            // getting data from TicketLaborEquipmentDetail by IdTicketLaborEquipment
            var ticketLaborEquipmentDetails = (await connection.QueryAsync<TicketLaborEquipmentDetail>(
                    TicketLaborEquipmentDetailSqlQueries
                        .QueryTicketLaborEquipmentDetailByIdTicketLaborEquipment,
                    new { IdTicketLaborEquipment = ticketLaborEquipments.Id }, tran))
                .ToList();

            switch (ticketLaborEquipments.Type)
            {
                // if type of ticketLaborEquipments is equal 0 it's mean create case then insert to laborEquipmentUnit table by quantity in TicketLaborEquipmentDetail table
                case 0:
                    //initializing list class to hold data before insert to database
                    var laborEquipmentUnits = new List<LaborEquipmentUnit>();

                    foreach (var item in ticketLaborEquipmentDetails)
                    {
                        for (var i = 0; i < item.Quantity; i++)
                        {
                            var uniqueCode = DateTimeOffset.Now.ToUnixTimeMilliseconds() + "_" +
                                             Guid.NewGuid().ToString("N");

                            laborEquipmentUnits.Add(new LaborEquipmentUnit()
                            {
                                Id = Guid.NewGuid(),
                                CreatedDate = DateTime.Now,
                                Status = 0,
                                EquipmentCode = uniqueCode,
                                IdTicketLaborEquipment = ticketLaborEquipments.Id,
                                IdUnit = ticketLaborEquipments.IdUnit,
                                IdEmployee = item.IdEmployee,
                                IdCategoryLaborEquipment = item.IdCategoryLaborEquipment
                            });
                        }
                    }

                    await connection.ExecuteAsync(LaborEquipmentUnitSqlQueries.QueryInsertLaborEquipmentUnit,
                        laborEquipmentUnits,
                        tran);

                    diaries = laborEquipmentUnits.Select(item => new Diary
                    {
                        Id = Guid.NewGuid(),
                        Content = $"{fullName} đã thêm mới bảng LaborEquipmentUnit",
                        UserId = idUserCurrent,
                        UserName = fullName,
                        DateCreate = DateTime.Now,
                        Title = "Thêm mới CSDL",
                        Operation = "Create",
                        Table = "LaborEquipmentUnit",
                        IsSuccess = true,
                        WithId = item.Id
                    }).ToList();

                    await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);
                    break;
                case 2:

                    #region LaborEquipmentUnit

                    // getting data from LaborEquipmentUnit by EquipmentCode
                    var laborEquipmentUnit = (await connection.QueryAsync<LaborEquipmentUnit>(
                            LaborEquipmentUnitSqlQueries
                                .QueryGetLaborEquipmentUnitByEquipmentCodes,
                            new { EquipmentCode = ticketLaborEquipmentDetails.Select(e => e.EquipmentCode) },
                            tran))
                        .ToList();

                    //updating all of status = 2 in LaborEquipmentUnit table by list id
                    await connection.ExecuteAsync(
                        LaborEquipmentUnitSqlQueries.QueryUpdateStatusLaborEquipmentUnitByIds,
                        new { Status = 2, Id = laborEquipmentUnit.Select(e => e.Id) },
                        tran);

                    diaries = laborEquipmentUnit.Select(item => new Diary
                    {
                        Id = Guid.NewGuid(),
                        Content = $"{fullName} đã cập nhật bảng LaborEquipmentUnit",
                        UserId = idUserCurrent,
                        UserName = fullName,
                        DateCreate = DateTime.Now,
                        Title = "Cập nhật CSDL",
                        Operation = "Update",
                        Table = "LaborEquipmentUnit",
                        IsSuccess = true,
                        WithId = item.Id
                    }).ToList();
                    await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

                    #endregion

                    #region TicketLaborEquipmentDetail

                    await connection.ExecuteAsync(
                        TicketLaborEquipmentDetailSqlQueries.QueryUpdateIsCheckTicketLaborEquipmentDetail,
                        new { IsCheck = 1, Id = ticketLaborEquipmentDetails.Select(e => e.Id) }, tran);

                    diaries = ticketLaborEquipmentDetails.Select(item => new Diary
                    {
                        Id = Guid.NewGuid(),
                        Content = $"{fullName} đã cập nhật bảng TicketLaborEquipmentDetail",
                        UserId = idUserCurrent,
                        UserName = fullName,
                        DateCreate = DateTime.Now,
                        Title = "Cập nhật CSDL",
                        Operation = "Update",
                        Table = "TicketLaborEquipmentDetail",
                        IsSuccess = true,
                        WithId = item.Id
                    }).ToList();
                    await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

                    #endregion

                    break;
            }
        }
    }

    /// <summary>
    /// this is exception in case user finishes process item in work flow but item is promotion transfer
    /// then dealing with potion employee table
    /// including 2 case
    /// case IsTransfer = true that's mean delete the record that exits by id employee and insert the new position for the new transfer
    /// case IsPromotion = true that's mean create a new record in the position employee
    /// </summary>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <param name="connection"></param>
    /// <param name="tran"></param>
    /// <param name="workflowInstances"></param>
    /// <param name="workflowTemplate"></param>
    private static async Task DealWithPositionEmployeeWhenFinishing(Guid idUserCurrent, string fullName,
        IDbConnection connection, IDbTransaction tran, WorkflowInstances workflowInstances,
        WorkflowTemplate workflowTemplate)
    {
        // in case complete work flow if it is promotion transfer then insert data to PositionEmployee table
        if (workflowTemplate.WorkflowCode == AppSettings.PromotionTransfer)
        {
            //getting data from promotionTransfer by id
            var promotionTransfer = await connection.QueryFirstOrDefaultAsync<PromotionTransfer>(
                PromotionTransferSqlQueries.QueryGetByIdPromotionTransfer, new { Id = workflowInstances.ItemId }, tran);

            if (promotionTransfer.IsPromotion)
            {
                await InsertToPositionEmployee(idUserCurrent, fullName, connection, tran, promotionTransfer);
            }

            if (promotionTransfer.IsTransfer)
            {
                await DeletePositionEmployee(idUserCurrent, fullName, connection, tran, promotionTransfer);

                await InsertToPositionEmployee(idUserCurrent, fullName, connection, tran, promotionTransfer);
            }
        }
    }

    /// <summary>
    /// this service uses to delete record in the position employee by id IdPosition that get by IdPositionEmployeeCurrent in the PromotionTransfer
    /// </summary>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <param name="connection"></param>
    /// <param name="tran"></param>
    /// <param name="promotionTransfer"></param>
    private static async Task DeletePositionEmployee(Guid idUserCurrent, string fullName, IDbConnection connection,
        IDbTransaction tran, PromotionTransfer promotionTransfer)
    {
        List<Diary> diaries;
        var positionEmployees =
            (await connection.QueryAsync<PositionEmployee>(
                PositionEmployeeSqlQueries.QueryDeletePositionEmployeeByIdPosition,
                new { IdPosition = promotionTransfer.IdPositionEmployeeCurrent },
                tran))
            .ToList();

        await connection.ExecuteAsync(PositionEmployeeSqlQueries.QueryInsertPositionEmployeeDeleted,
            positionEmployees,
            tran);

        await connection.ExecuteAsync(PositionEmployeeSqlQueries.QueryDeletePositionEmployee,
            new { Ids = positionEmployees.Select(e => e.Id) },
            tran);

        diaries = positionEmployees.Select(item => new Diary
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã xóa từ bảng PositionEmployee",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Xóa từ CSDL",
            Operation = "Delete",
            Table = "PositionEmployee",
            IsSuccess = true,
            WithId = item.Id
        }).ToList();

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);
    }

    /// <summary>
    /// this service uses to insert data to PositionEmployee table
    /// </summary>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <param name="connection"></param>
    /// <param name="tran"></param>
    /// <param name="promotionTransfer"></param>
    private static async Task InsertToPositionEmployee(Guid idUserCurrent, string fullName, IDbConnection connection,
        IDbTransaction tran, PromotionTransfer promotionTransfer)
    {
        var positionEmployee = new PositionEmployee()
        {
            Id = Guid.NewGuid(),
            IdEmployee = promotionTransfer.IdEmployee,
            IdPosition = promotionTransfer.IdCategoryPosition,
            IsHeadcount = promotionTransfer.IsHeadCount,
            IdUnit = promotionTransfer.IdUnit,
            CreatedDate = DateTime.Now,
            Status = 0
        };

        await connection.ExecuteAsync(PositionEmployeeSqlQueries.QueryInsertPositionEmployee, positionEmployee,
            tran);

        var diary = new Diary()
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã thêm mới bảng PositionEmployee",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Thêm mới CSDL",
            Operation = "Create",
            Table = "PositionEmployee",
            IsSuccess = true,
            WithId = positionEmployee.Id
        };

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);
    }

    /// <summary>
    /// using this function to terminate item in work flow
    /// this function is using the params like a above function
    /// </summary>
    /// <param name="idWorkFlowInstance"></param>
    /// <param name="message"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <param name="connection"></param>
    /// <param name="tran"></param>
    /// <param name="workflowStep"></param>
    /// <param name="workflowInstances"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    private static async Task<TemplateApi<WorkflowTemplateDto>> TerminatingItemInWorkFlow(Guid idWorkFlowInstance,
        string message, Guid idUserCurrent,
        string fullName, IDbConnection connection, IDbTransaction tran, IReadOnlyCollection<WorkflowStep> workflowStep,
        WorkflowInstances workflowInstances, User user)
    {
        Diary diary;

        #region WorkflowInstances

        //getting workFlowInstance by id
        var workFlowInstance = await connection.QueryFirstOrDefaultAsync<WorkflowInstances>(
            WorkflowInstancesSqlQueries.QueryGetByIdWorkflowInstances, new { Id = idWorkFlowInstance },
            tran);

        //getting rejectName field in workflowStep table
        var rejectName = workflowStep.ElementAt(workFlowInstance!
            .CurrentStep - 1).RejectName;

        //updating instance of work flow become terminated
        await connection.ExecuteAsync(
            WorkflowInstancesSqlQueries.QueryUpdateWorkflowInstancesToComplete,
            new
            {
                IsCompleted = false, IsDraft = false, IsTerminated = true, NameStatus = rejectName,
                Message = message,
                Id = idWorkFlowInstance
            },
            tran);

        diary = new Diary()
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã cập nhật bảng WorkflowInstances",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Cập nhật CSDL",
            Operation = "Update",
            Table = "WorkflowInstances",
            IsSuccess = true,
            WithId = workflowInstances.Id
        };

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

        #endregion

        #region WorkflowHistory

        var workflowHistoryComplete = new WorkflowHistory()
        {
            Id = Guid.NewGuid(),
            Status = 0,
            CreatedDate = DateTime.Now,
            IdWorkFlowInstance = idWorkFlowInstance,
            IdUser = idUserCurrent,
            Action = workflowStep.ElementAt(workflowInstances.CurrentStep - 1).StepName,
            IdUnit = user.UnitId,
            IsStepCompleted = false,
            Comment = null,
            Message = message,
            IsCancelled = true,
            IsRequestToChanged = false
        };

        await connection.ExecuteAsync(WorkflowHistorySqlQueries.QueryInsertWorkflowHistory,
            workflowHistoryComplete,
            tran);

        diary = new Diary()
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã thêm mới bảng WorkflowHistory",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Thêm mới CSDL",
            Operation = "Create",
            Table = "WorkflowHistory",
            IsSuccess = true,
            WithId = workflowHistoryComplete.Id
        };

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

        #endregion

        tran.Commit();

        return new TemplateApi<WorkflowTemplateDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0,
            0);
    }

    /// <summary>
    /// using this function to request to change information item in work flow
    /// this function is using the params like a above function
    /// </summary>
    /// <param name="idWorkFlowInstance"></param>
    /// <param name="message"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <param name="connection"></param>
    /// <param name="tran"></param>
    /// <param name="workflowInstances"></param>
    /// <param name="workflowStep"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    private static async Task<TemplateApi<WorkflowTemplateDto>> RequestToChangeItemInWorkFlow(Guid idWorkFlowInstance,
        string message, Guid idUserCurrent, string fullName,
        IDbConnection connection, IDbTransaction tran, WorkflowInstances workflowInstances,
        IEnumerable<WorkflowStep> workflowStep, User user)
    {
        Diary diary;

        #region WorkflowInstances

        var data = new WorkflowInstances()
        {
            Id = idWorkFlowInstance,
            CurrentStep = 0,
            UnitId = null,
            IsDraft = true,
            Message = message,
            NameStatus = "Yêu cầu chỉnh sửa lại"
        };
        await connection.ExecuteAsync(WorkflowInstancesSqlQueries.QueryUpdateWorkflowInstances,
            data, tran);

        diary = new Diary()
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã cập nhật bảng WorkflowInstances",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Cập nhật CSDL",
            Operation = "Update",
            Table = "WorkflowInstances",
            IsSuccess = true,
            WithId = workflowInstances.Id
        };

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

        #endregion

        #region WorkflowHistory

        var workflowHistoryComplete = new WorkflowHistory()
        {
            Id = Guid.NewGuid(),
            Status = 0,
            CreatedDate = DateTime.Now,
            IdWorkFlowInstance = idWorkFlowInstance,
            IdUser = idUserCurrent,
            Action = workflowStep.ElementAt(workflowInstances.CurrentStep - 1).StepName,
            IdUnit = user.UnitId,
            IsStepCompleted = false,
            Comment = null,
            Message = message,
            IsCancelled = false,
            IsRequestToChanged = true
        };

        await connection.ExecuteAsync(WorkflowHistorySqlQueries.QueryInsertWorkflowHistory,
            workflowHistoryComplete,
            tran);

        diary = new Diary()
        {
            Id = Guid.NewGuid(),
            Content = $"{fullName} đã thêm mới bảng WorkflowHistory",
            UserId = idUserCurrent,
            UserName = fullName,
            DateCreate = DateTime.Now,
            Title = "Thêm mới CSDL",
            Operation = "Create",
            Table = "WorkflowHistory",
            IsSuccess = true,
            WithId = workflowHistoryComplete.Id
        };

        await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

        #endregion

        tran.Commit();

        return new TemplateApi<WorkflowTemplateDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0,
            0);
    }

    /// <summary>
    /// When initializing item data to work flow, using this service to add instances of item, one item one instance
    /// </summary>
    /// <param name="codeWorkFlow"></param>
    /// <param name="itemId"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="fullName"></param>
    /// <returns></returns>
    public async Task<TemplateApi<WorkflowTemplateDto>> InsertStepWorkFlow(string codeWorkFlow, Guid itemId,
        Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            //getting user information by id which is logging
            var user = await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.UserById,
                new { Id = idUserCurrent }, tran);

            #region WorkflowInstances

            //getting work flow template information by work flow code
            var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
                WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
                new { WorkflowCode = codeWorkFlow }, tran);

            if (workflowTemplate is null)
            {
                return new TemplateApi<WorkflowTemplateDto>(null, null, "Mã code WorkFlow không tồn tại !", false, 0, 0,
                    0, 0);
            }

            //getting max value current in database of work flow instance table
            var maxNumber = await connection.QueryFirstOrDefaultAsync<int>(
                WorkflowInstancesSqlQueries.QueryMaxNumberForCodeWorkflowInstances,
                new { TemplateId = workflowTemplate.Id }, tran);

            //init string number is a million then plus it with max current number which increased by one in database  
            string inputNumber = "0000000";
            int currentNumber = int.Parse(inputNumber);
            currentNumber += maxNumber + 1;
            string incrementedNumber = currentNumber.ToString().PadLeft(7, '0');

            var workflowInstance = new WorkflowInstances()
            {
                Id = Guid.NewGuid(),
                Status = 0,
                CreatedDate = DateTime.Now,
                WorkflowCode = workflowTemplate?.WorkflowCode + "-" + incrementedNumber + "-" + DateTime.Now.Year,
                WorkflowName = workflowTemplate?.WorkflowName,
                IsCompleted = false,
                IsTerminated = false,
                IsDraft = true,
                NameStatus = "Dự thảo bản nháp",
                TemplateId = workflowTemplate!.Id,
                CurrentStep = 0,
                UnitId = null,
                ItemId = itemId,
                CreatedBy = idUserCurrent,
                RequestToChange = false,
                Message = null,
                IsApproved = false,
                NumberForCode = maxNumber + 1
            };

            await connection.ExecuteAsync(WorkflowInstancesSqlQueries.QueryInsertWorkflowInstances, workflowInstance,
                tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng WorkflowInstances",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "WorkflowInstances",
                IsSuccess = true,
                WithId = workflowInstance.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            #endregion

            #region WorkflowHistory

            var workflowHistory = new WorkflowHistory()
            {
                Id = Guid.NewGuid(),
                Status = 0,
                CreatedDate = DateTime.Now,
                IdWorkFlowInstance = workflowInstance.Id,
                IdUser = idUserCurrent,
                Action = "Dự thảo bản nháp",
                IdUnit = user.UnitId,
                IsStepCompleted = false,
                Comment = null,
                Message = null,
                IsCancelled = false,
                IsRequestToChanged = false
            };

            await connection.ExecuteAsync(WorkflowHistorySqlQueries.QueryInsertWorkflowHistory, workflowHistory,
                tran);

            diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng WorkflowHistory",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "WorkflowHistory",
                IsSuccess = true,
                WithId = workflowInstance.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<WorkflowTemplateDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    #endregion

    #region ===[ Common Get WorkFlow Methods ]==================================================

    /// <summary>
    /// filtering proper data to each item
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="data"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="isAdminOrUserRequest"></param>
    /// <param name="idTemplate"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<List<T>> FilterAndSortItemData<T>(IDbConnection connection, List<T> data, Guid idUserCurrent,
        bool isAdminOrUserRequest, Guid idTemplate)
    {
        //get all data in WorkflowInstances table by idTemplate
        var workflowInstances =
            (await connection.QueryAsync<WorkflowInstances>(
                WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByTemplateId,
                new { TemplateId = new List<Guid>() { idTemplate } }))
            .ToList();
        //get all data in WorkflowStep table by idTemplate
        var workflowSteps = (await connection.QueryAsync<WorkflowStep>(
                WorkflowStepSqlQueries.QueryGetWorkflowStepByTemplateId,
                new { TemplateId = new List<Guid>() { idTemplate } }))
            .ToList();
        //get data by id in user table
        var user = await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.UserById, new { Id = idUserCurrent });
        //get all data by id user in role table
        var roles = (await connection.QueryAsync<Role>(RoleSqlQueries.GetRoleByIdUser, new { IdUser = idUserCurrent }))
            .ToList();

        //getting all data if requesting user is admin or filtering data by createdBy
        //default filtering data is depend on unitID and roleID of use is requesting whose data match with UnitID and roleID
        return AssignValueToIListItem(data, workflowInstances, workflowSteps, isAdminOrUserRequest, user, roles);
    }

    /// <summary>
    /// filtering proper histories data to each item
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="data"></param>
    /// <param name="idUserCurrent"></param>
    /// <param name="idTemplate"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<List<T>> FilterAndSortItemDataHistories<T>(IDbConnection connection, List<T> data,
        Guid idUserCurrent, Guid idTemplate)
    {
        //get all data in WorkflowInstances table by idTemplate
        var workflowInstances =
            (await connection.QueryAsync<WorkflowInstances>(
                WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByTemplateId,
                new { TemplateId = new List<Guid>() { idTemplate } }))
            .ToList();
        //get all data in WorkflowStep table by idTemplate
        var workflowSteps = (await connection.QueryAsync<WorkflowStep>(
                WorkflowStepSqlQueries.QueryGetWorkflowStepByTemplateId,
                new { TemplateId = new List<Guid>() { idTemplate } }))
            .ToList();

        //get data by id in user table
        var user = await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.UserById, new { Id = idUserCurrent });
        //get all data by id user in role table
        var roles = (await connection.QueryAsync<Role>(RoleSqlQueries.GetRoleByIdUser, new { IdUser = idUserCurrent }))
            .ToList();

        //get data by IdWorkFlowInstance in WorkflowHistory table
        var workflowHistories = (await connection.QueryAsync<WorkflowHistory>(
            WorkflowHistorySqlQueries.QueryGetAllWorkflowHistoriesByIdWorkFlowInstance,
            new { IdWorkFlowInstance = workflowInstances.Select(e => e.Id) })).ToList();

        var filteredData = new List<T>();

        foreach (var item in data)
        {
            var id = GetIdValue(item);

            //get instance of item requestToHired
            var itemWorkflowInstances = workflowInstances.FirstOrDefault(e =>
                e.ItemId == id);

            //if itemWorkflowInstances don't have any data then continue loop
            if (itemWorkflowInstances is null) continue;

            //get all WorkflowHistory by id user and id IdWorkFlowInstance
            //if admin then getting all data
            if (roles.Any(e => e.IsAdmin))
            {
                if (!workflowHistories.Exists(e => e.IdWorkFlowInstance == itemWorkflowInstances.Id))
                {
                    continue;
                }
            }
            else
            {
                if (!workflowHistories.Exists(e =>
                        e.IdWorkFlowInstance == itemWorkflowInstances.Id && e.IdUser == idUserCurrent))
                {
                    continue;
                }
            }

            //if histories don't have any data then continue loop
            //if (!workflowHistories.Any()) continue;

            var result = SetValueToItem(item, workflowSteps, workflowInstances, true, user, roles);
            if (result is not null)
            {
                filteredData.Add(result);
            }
        }

        return filteredData;
    }

    /// <summary>
    /// assigning data to each element of list data
    /// </summary>
    /// <param name="data">list data of item </param>
    /// <param name="workflowSteps">list data of workflowSteps table by specific IdTemplate</param>
    /// <param name="workflowInstances">list data of workflowInstances table by specific IdTemplate</param>
    /// <param name="isAdminOrUserRequest">to check to get proper data</param>
    /// <param name="user">get user by id</param>
    /// <param name="roles">get list role by id user</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private List<T> AssignValueToIListItem<T>(List<T> data, IReadOnlyCollection<WorkflowInstances> workflowInstances,
        IReadOnlyCollection<WorkflowStep> workflowSteps, bool isAdminOrUserRequest, User user,
        IReadOnlyCollection<Role> roles)
    {
        //initializing list class to hold data in it
        var filteredData = new List<T>();

        //looping through list data and set additional information for each of element, if value is null then not set value to the array 
        foreach (var item in data)
        {
            var result = SetValueToItem(item, workflowSteps, workflowInstances, isAdminOrUserRequest, user, roles);

            if (result is not null)
            {
                filteredData.Add(result);
            }
        }

        return filteredData;
    }

    /// <summary>
    /// setting additional value to item object 
    /// </summary>
    /// <param name="item">object data like requestToHire, resign ...</param>
    /// <param name="workflowSteps">list data of workflowSteps table by specific IdTemplate</param>
    /// <param name="workflowInstances">list data of workflowInstances table by specific IdTemplate</param>
    /// <param name="isAdminOrUserRequest">to check to get proper data</param>
    /// <param name="user">get user by id</param>
    /// <param name="roles">get list role by id user</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private T? SetValueToItem<T>(T item, IEnumerable<WorkflowStep> workflowSteps,
        IReadOnlyCollection<WorkflowInstances> workflowInstances,
        bool isAdminOrUserRequest, User user, IEnumerable<Role> roles)
    {
        var objectType = item!.GetType();
        var id = GetIdValue(item);

        // Getting the instance of item data from workflowInstances
        var itemWorkflowInstance = isAdminOrUserRequest
            ? workflowInstances.FirstOrDefault(e => e.ItemId == id)
            : workflowInstances.FirstOrDefault(e =>
                e.ItemId == id && e.IsTerminated == false && e.IsCompleted == false && e.UnitId is not null);

        if (itemWorkflowInstance is null)
        {
            return default;
        }

        // Get all workflow steps for the template and order them by ascending order
        var itemWorkflowSteps = workflowSteps
            .Where(e => e.TemplateId == itemWorkflowInstance!.TemplateId)
            .OrderBy(e => e.Order)
            .ToList();

        // Get workflowInstances of the data item
        var workflowInstancesItem =
            workflowInstances.Where(e => e.ItemId == id).ToList();
        // Count all steps belonging to the TemplateID in workflowSteps table
        var countWorkFlowStepItem = itemWorkflowSteps.Count;
        // Get the row data in workflowSteps table that matches CurrentStep in workflowInstances table
        var currentWorkFlowStepItem = itemWorkflowInstance!.CurrentStep != 0
            ? itemWorkflowSteps.ElementAtOrDefault(itemWorkflowInstance.CurrentStep - 1)
            : null;

        // Set properties using reflection
        objectType.GetProperty("WorkflowInstances")!.SetValue(item, workflowInstancesItem);
        objectType.GetProperty("CountWorkFlowStep")!.SetValue(item, countWorkFlowStepItem);
        objectType.GetProperty("CurrentWorkFlowStep")!.SetValue(item, currentWorkFlowStepItem);

        if (!isAdminOrUserRequest)
        {
            //get IdUnitAssign of WorkFlowStep table at index = currentStep of workflowInstances
            var idUnitInWorkFlowStep =
                itemWorkflowSteps.ElementAt(itemWorkflowInstance.CurrentStep - 1).IsDirectUnit
                    ? itemWorkflowInstance.UnitId
                    : itemWorkflowSteps.ElementAt(itemWorkflowInstance.CurrentStep - 1).IdUnitAssign;

            //get IdRoleAssign of WorkFlowStep table at index = currentStep of workflowInstances
            var idRoleInWorkFlowStep =
                itemWorkflowSteps.ElementAt(itemWorkflowInstance.CurrentStep - 1).IdRoleAssign;

            // if user is requesting having UnitId and list RoleId, and both of them have in WorkFlowStep at specific element then add it to the list data
            // otherwise continue the next index in loop
            if (idUnitInWorkFlowStep == user.UnitId && roles.Any(role => role.Id == idRoleInWorkFlowStep))
            {
                return item;
            }

            return default;
        }

        return item;
    }

    /// <summary>
    /// Getting guid id from item 
    /// </summary>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static Guid GetIdValue<T>(T item)
    {
        var objectType = item!.GetType();
        var idProperty = objectType.GetProperty("Id")?.GetValue(item)!;
        var id = new Guid(idProperty.ToString()!);

        return id;
    }

    #endregion
}