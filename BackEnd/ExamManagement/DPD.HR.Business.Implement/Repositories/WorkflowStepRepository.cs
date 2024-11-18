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

public class WorkflowStepRepository : IWorkflowStepRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public WorkflowStepRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ WorkflowStepRepository Methods ]==================================================

    public async Task<TemplateApi<WorkflowStepDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<WorkflowStep>(WorkflowStepSqlQueries.QueryGetAllWorkflowStep))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<WorkflowStepDto>()),
            units.Count);
    }

    public async Task<TemplateApi<WorkflowStepDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<WorkflowStep>(
            WorkflowStepSqlQueries.QueryGetByIdWorkflowStep, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<WorkflowStepDto>());
    }

    public Task<TemplateApi<WorkflowStepDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<WorkflowStepDto>> Update(WorkflowStepDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var workflowStepById = await connection.QueryFirstOrDefaultAsync<WorkflowStep>(
                WorkflowStepSqlQueries.QueryGetByIdWorkflowStep, new { Id = model.Id }, tran);

            if (workflowStepById != null)
            {
                var idsTemplateId = new List<Guid>() { workflowStepById.TemplateId };
                var workflowInstances = (await connection.QueryAsync<WorkflowInstances>(
                    WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByTemplateId,
                    new { TemplateId = idsTemplateId },
                    tran)).ToList();

                if (workflowInstances.Exists(e => e.CurrentStep != 0 && e.IsCompleted == false))
                {
                    return new TemplateApi<WorkflowStepDto>(null, null,
                        "Mẫu này đã đi vào hoạt động không thể chỉnh sửa !", false,
                        0,
                        0, 0, 0);
                }
            }

            var workflowStep = model.Adapt<WorkflowStep>();
            await connection.ExecuteAsync(WorkflowStepSqlQueries.QueryUpdateWorkflowStep, workflowStep, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng WorkflowStep",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "WorkflowStep",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<WorkflowStepDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<WorkflowStepDto>> Insert(WorkflowStepDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var idsTemplateId = new List<Guid>() { model.TemplateId };
            var workflowStep = await connection.QueryFirstOrDefaultAsync<WorkflowStep>(
                WorkflowStepSqlQueries.QueryGetWorkflowStepByTemplateId, new { TemplateId = idsTemplateId }, tran);

            if (workflowStep != null && workflowStep.Order == model.Order)
            {
                return new TemplateApi<WorkflowStepDto>(null, null, "Không thể thêm trùng vị trí xắp xếp !", false, 0,
                    0, 0, 0);
            }

            var workflowInstances = (await connection.QueryAsync<WorkflowInstances>(
                WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByTemplateId, new { TemplateId = idsTemplateId },
                tran)).ToList();

            if (workflowInstances.Exists(e => e.CurrentStep != 0 && e.IsCompleted == false))
            {
                return new TemplateApi<WorkflowStepDto>(null, null, "Mẫu này đã đi vào hoạt động không thể thêm !",
                    false, 0, 0, 0, 0);
            }

            await connection.ExecuteAsync(WorkflowStepSqlQueries.QueryInsertWorkflowStep, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng WorkflowStep",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "WorkflowStep",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<WorkflowStepDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<WorkflowStepDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var workflowStepById = (await connection.QueryAsync<WorkflowStep>(
                WorkflowStepSqlQueries.QueryGetWorkflowStepByIds, new { Ids = ids }, tran)).ToList();

            if (workflowStepById.Count > 0)
            {
                var workflowInstances = (await connection.QueryAsync<WorkflowInstances>(
                    WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByTemplateId,
                    new { TemplateId = workflowStepById.Select(e => e.TemplateId) }, tran)).ToList();

                if (workflowInstances.Count != 0 && workflowInstances.Exists(e => e.IsCompleted == false))
                {
                        return new TemplateApi<WorkflowStepDto>(null, null,
                        "Mẫu này đã có dữ liệu không thể xóa !", false,
                        0,
                        0, 0, 0);
                }
            }

            var workflowSteps =
                (await connection.QueryAsync<WorkflowStep>(WorkflowStepSqlQueries.QueryGetWorkflowStepByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(WorkflowStepSqlQueries.QueryInsertWorkflowStepDeleted, workflowSteps, tran);

            await connection.ExecuteAsync(WorkflowStepSqlQueries.QueryDeleteWorkflowStep, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng WorkflowStep",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "WorkflowStep",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<WorkflowStepDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<WorkflowStepDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<WorkflowStepDto>> GetAllByIdTemplate(int pageNumber, int pageSize, Guid idTemplate)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var ids = new List<Guid>() { idTemplate };
        var units = (await connection.QueryAsync<WorkflowStep>(WorkflowStepSqlQueries.QueryGetWorkflowStepByTemplateId,
                new { TemplateId = ids })).OrderBy(e => e.Order)
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<WorkflowStepDto>()),
            units.Count);
    }

    public async Task<TemplateApi<WorkflowStepDto>> CUD_WorkflowStep(List<WorkflowStepDto> workflowStepDto,
        Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var workflowStepsNotIn =
                (await connection.QueryAsync<WorkflowStep>(
                    WorkflowStepSqlQueries.QueryGetWorkflowStepNotInIds,
                    new
                    {
                        Ids = workflowStepDto.Select(e => e.Id).ToList()
                    },
                    tran))
                .ToList();

            if (workflowStepsNotIn.Any())
            {
                await connection.ExecuteAsync(WorkflowStepSqlQueries.QueryInsertWorkflowStepDeleted,
                    workflowStepsNotIn, tran);

                await connection.ExecuteAsync(WorkflowStepSqlQueries.QueryDeleteWorkflowStep,
                    new { Ids = workflowStepsNotIn.Select(e => e.Id).ToList() },
                    tran);

                var diariesNotIn = workflowStepsNotIn.Select(item => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảng WorkflowStep",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "WorkflowStep",
                    IsSuccess = true,
                    WithId = item.Id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diariesNotIn, tran);
            }

            var dataToInsert = workflowStepDto?.Where(e => e.Id == Guid.Empty).ToList();
            var dataToUpdate = workflowStepDto?.Where(e => e.Id != Guid.Empty).ToList();

            var workflowSteps = dataToInsert?.Select(item => new WorkflowStep()
                {
                    Id = Guid.NewGuid(),
                    TemplateId = item.TemplateId,
                    StepName = item.StepName,
                    IdRoleAssign = item.IdRoleAssign,
                    IdUnitAssign = item.IdUnitAssign,
                    AllowTerminated = false,
                    RejectName = item.RejectName,
                    Order = item.Order,
                    OutCome = item.OutCome,
                    Status = 0,
                    CreatedDate = DateTime.Now
                })
                .ToList();

            await connection.ExecuteAsync(WorkflowStepSqlQueries.QueryInsertWorkflowStep, workflowSteps,
                tran);

            var diaries = workflowSteps?.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng WorkflowStep",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "WorkflowStep",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            await connection.ExecuteAsync(WorkflowStepSqlQueries.QueryUpdateWorkflowStep,
                dataToUpdate,
                tran);

            diaries = dataToUpdate?.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng WorkflowStep",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "UPDATE",
                Table = "WorkflowStep",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<WorkflowStepDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
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