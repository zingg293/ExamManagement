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

public class BusinessTripEmployeeRepository : IBusinessTripEmployeeRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public BusinessTripEmployeeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ BusinessTripEmployeeRepository Methods ]==================================================

    public async Task<TemplateApi<BusinessTripEmployeeDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<BusinessTripEmployee>(BusinessTripEmployeeSqlQueries
                .QueryGetAllBusinessTripEmployee))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<BusinessTripEmployeeDto>()),
            units.Count);
    }

    public async Task<TemplateApi<BusinessTripEmployeeDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<BusinessTripEmployee>(
            BusinessTripEmployeeSqlQueries.QueryGetByIdBusinessTripEmployee, new { Id = id });
        var result = unit.Adapt<BusinessTripEmployeeDto>();

        return new Pagination().HandleGetByIdRespond(result);
    }

    public Task<TemplateApi<BusinessTripEmployeeDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<BusinessTripEmployeeDto>> Update(BusinessTripEmployeeDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var businessTripEmployee = model.Adapt<BusinessTripEmployee>();
            await connection.ExecuteAsync(BusinessTripEmployeeSqlQueries.QueryUpdateBusinessTripEmployee,
                businessTripEmployee, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng BusinessTripEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "BusinessTripEmployee",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<BusinessTripEmployeeDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<BusinessTripEmployeeDto>> Insert(BusinessTripEmployeeDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(BusinessTripEmployeeSqlQueries.QueryInsertBusinessTripEmployee, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng BusinessTripEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "BusinessTripEmployee",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<BusinessTripEmployeeDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<BusinessTripEmployeeDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<BusinessTripEmployee>(
                    BusinessTripEmployeeSqlQueries.QueryGetBusinessTripEmployeeByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(BusinessTripEmployeeSqlQueries.QueryInsertBusinessTripEmployeeDeleted,
                categoryCities,
                tran);

            await connection.ExecuteAsync(BusinessTripEmployeeSqlQueries.QueryDeleteBusinessTripEmployee,
                new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng BusinessTripEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "BusinessTripEmployee",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<BusinessTripEmployeeDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<BusinessTripEmployeeDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<BusinessTripEmployeeDto>> InsertBusinessTripEmployeeByList(
        List<BusinessTripEmployeeDto> businessTripEmployee, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var businessTripEmployeesNotIn =
                (await connection.QueryAsync<BusinessTripEmployee>(
                    BusinessTripEmployeeSqlQueries.QueryGetAllBusinessTripEmployeeNotInIds,
                    new
                    {
                        Ids = businessTripEmployee.Select(e => e.Id).ToList(),
                        IdBusinessTrip = businessTripEmployee?.FirstOrDefault()?.IdBusinessTrip
                    },
                    tran))
                .ToList();

            if (businessTripEmployeesNotIn.Count > 0)
            {
                await connection.ExecuteAsync(
                    BusinessTripEmployeeSqlQueries.QueryInsertBusinessTripEmployeeDeleted,
                    businessTripEmployeesNotIn, tran);

                await connection.ExecuteAsync(
                    BusinessTripEmployeeSqlQueries.QueryDeleteBusinessTripEmployee,
                    new { Ids = businessTripEmployeesNotIn.Select(e => e.Id).ToList() },
                    tran);

                var diariesNotIn = businessTripEmployeesNotIn.Select(item => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảng BusinessTripEmployee",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "BusinessTripEmployee",
                    IsSuccess = true,
                    WithId = item.Id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diariesNotIn, tran);
            }

            var dataToInsert = businessTripEmployee?.Where(e => e.Id == Guid.Empty).ToList();
            var dataToUpdate = businessTripEmployee?.Where(e => e.Id != Guid.Empty).ToList();

            var businessTripEmployeesToSave = dataToInsert?.Select(item => new BusinessTripEmployee()
                {
                    Id = Guid.NewGuid(),
                    Status = 0,
                    CreatedDate = DateTime.Now,
                    IdBusinessTrip = item.IdBusinessTrip,
                    Captain = item.Captain,
                    IdEmployee = item.IdEmployee,
                })
                .ToList();

            await connection.ExecuteAsync(BusinessTripEmployeeSqlQueries.QueryInsertBusinessTripEmployee,
                businessTripEmployeesToSave,
                tran);

            var diaries = businessTripEmployeesToSave?.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng BusinessTripEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "BusinessTripEmployee",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            //update data business trip empployee table
            await connection.ExecuteAsync(BusinessTripEmployeeSqlQueries.QueryUpdateBusinessTripEmployee,
                dataToUpdate,
                tran);

            diaries = dataToUpdate?.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng BusinessTripEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "BusinessTripEmployee",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<BusinessTripEmployeeDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0,
                0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<BusinessTripEmployeeDto>> GetListByIBusinessTrip(Guid idBusinessTrip, int pageNumber,
        int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<BusinessTripEmployee>(BusinessTripEmployeeSqlQueries
                .QueryGetBusinessTripEmployeeByIdBusinessTrip, new { IdBusinessTrip = idBusinessTrip }))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<BusinessTripEmployeeDto>()),
            units.Count);
    }

    #endregion
}