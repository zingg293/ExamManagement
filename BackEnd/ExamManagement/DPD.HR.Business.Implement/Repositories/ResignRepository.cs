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

public class ResignRepository : IResignRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public ResignRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ ResignRepository Methods ]==================================================

    public async Task<TemplateApi<ResignDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<Resign>(ResignSqlQueries.QueryGetAllResign))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<ResignDto>()),
            units.Count);
    }

    public async Task<TemplateApi<ResignDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<Resign>(
            ResignSqlQueries.QueryGetByIdResign, new { Id = id });

        var result = unit.Adapt<ResignDto>();
        return new Pagination().HandleGetByIdRespond(result);
    }

    public Task<TemplateApi<ResignDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<ResignDto>> Update(ResignDto model, Guid idUserCurrent, string fullName)
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
                return new TemplateApi<ResignDto>(null, null, "Phiếu đã đi vào quy trình không thể chỉnh sửa !",
                    false, 0,
                    0, 0, 0);
            }
            
            var resign = model.Adapt<Resign>();
            
            var resignById = await connection.QueryFirstOrDefaultAsync<Resign>(
                ResignSqlQueries.QueryGetByIdResign, new { Id = model.Id }, tran);

            if (model.IdFile is not null)
            {
                resign.ResignForm = resignById.ResignForm;
            }
            await connection.ExecuteAsync(ResignSqlQueries.QueryUpdateResign, resign, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng Resign",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "Resign",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<ResignDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<ResignDto>> Insert(ResignDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(ResignSqlQueries.QueryInsertResign, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng Resign",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "Resign",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<ResignDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<ResignDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<Resign>(ResignSqlQueries.QueryGetResignByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(ResignSqlQueries.QueryInsertResignDeleted, categoryCities,
                tran);

            await connection.ExecuteAsync(ResignSqlQueries.QueryDeleteResign, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng Resign",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "Resign",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<ResignDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<ResignDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion

    public async Task<ResignDto> GetDataById(Guid idResign)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var resign = await connection.QueryFirstOrDefaultAsync<Resign>(
            ResignSqlQueries.QueryGetByIdResign, new { Id = idResign });

        return resign.Adapt<ResignDto>();
    }

    public async Task<TemplateApi<ResignDto>> GetResignAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all data in Resign table
        var resign =
            (await connection.QueryAsync<Resign>(ResignSqlQueries.QueryGetAllResign))
            .ToList().Adapt<List<ResignDto>>();

        //get all data by id user in role table
        var roles = (await connection.QueryAsync<Role>(RoleSqlQueries.GetRoleByIdUser, new { IdUser = idUserCurrent }))
            .ToList();

        //checking for the records created by user is requesting if have any data then filtering it by id requesting user  
        //otherwise getting default data by UnitId and RoleId
        var checkHaveAnyByCreateBy =
            resign.Exists(e => e.IdUserRequest == idUserCurrent) && !roles.Exists(e => e.IsAdmin);
        if (checkHaveAnyByCreateBy)
        {
            resign = resign.Where(e => e.IdUserRequest == idUserCurrent).ToList();
        }

        //getting all data if requesting user is admin
        bool isAdminOrUserRequest = roles.Exists(e => e.IsAdmin) || checkHaveAnyByCreateBy;
        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.Resign });

        var filteredResign = await new WorkFlowRepository(_configuration).FilterAndSortItemData(connection,
            resign,
            idUserCurrent, isAdminOrUserRequest, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredResign,
            filteredResign.Count);
    }

    public async Task<TemplateApi<ResignDto>> GetResignAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber,
        int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all Resign
        var resign =
            (await connection.QueryAsync<Resign>(ResignSqlQueries.QueryGetAllResign))
            .ToList().Adapt<List<ResignDto>>();

        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.Resign });

        var filteredResign = await new WorkFlowRepository(_configuration).FilterAndSortItemDataHistories(
            connection,
            resign,
            idUserCurrent, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredResign,
            filteredResign.Count);
    }
}