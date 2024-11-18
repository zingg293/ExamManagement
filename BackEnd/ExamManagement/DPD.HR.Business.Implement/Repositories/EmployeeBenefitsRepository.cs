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

public class EmployeeBenefitsRepository : IEmployeeBenefitsRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public EmployeeBenefitsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ EmployeeBenefitsRepository Methods ]==================================================

    public async Task<TemplateApi<EmployeeBenefitsDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var employeeBenefits = (await connection.QueryAsync<EmployeeBenefits>(EmployeeBenefitsSqlQueries
                .QueryGetAllEmployeeBenefits))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            employeeBenefits.Select(u => u.Adapt<EmployeeBenefitsDto>()),
            employeeBenefits.Count);
    }

    public async Task<TemplateApi<EmployeeBenefitsDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<EmployeeBenefits>(
            EmployeeBenefitsSqlQueries.QueryGetByIdEmployeeBenefits, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<EmployeeBenefitsDto>());
    }

    public Task<TemplateApi<EmployeeBenefitsDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<EmployeeBenefitsDto>> Update(EmployeeBenefitsDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var employeeBenefits = model.Adapt<EmployeeBenefits>();
            await connection.ExecuteAsync(EmployeeBenefitsSqlQueries.QueryUpdateEmployeeBenefits, employeeBenefits,
                tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng EmployeeBenefits",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "EmployeeBenefits",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<EmployeeBenefitsDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<EmployeeBenefitsDto>> Insert(EmployeeBenefitsDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(EmployeeBenefitsSqlQueries.QueryInsertEmployeeBenefits, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng EmployeeBenefits",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "EmployeeBenefits",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<EmployeeBenefitsDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<EmployeeBenefitsDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var employeeBenefits =
                (await connection.QueryAsync<EmployeeBenefits>(EmployeeBenefitsSqlQueries.QueryEmployeeBenefitsByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(EmployeeBenefitsSqlQueries.QueryInsertEmployeeBenefitsDeleted,
                employeeBenefits, tran);

            await connection.ExecuteAsync(EmployeeBenefitsSqlQueries.QueryDeleteEmployeeBenefits, new { Ids = ids },
                tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng EmployeeBenefits",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "EmployeeBenefits",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<EmployeeBenefitsDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<EmployeeBenefitsDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<EmployeeBenefitsDto>> UpdateEmployeeBenefits(
        List<EmployeeBenefitsDto> employeeBenefitsDto, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var employeeBenefitsNotIn =
                (await connection.QueryAsync<EmployeeBenefits>(
                    EmployeeBenefitsSqlQueries.QueryEmployeeBenefitsByNotInIds,
                    new
                    {
                        Ids = employeeBenefitsDto.Select(e => e.Id).ToList(),
                        IdEmployee = employeeBenefitsDto?.FirstOrDefault()?.IdEmployee
                    },
                    tran))
                .ToList();

            if (employeeBenefitsNotIn.Any())
            {
                await connection.ExecuteAsync(EmployeeBenefitsSqlQueries.QueryInsertEmployeeBenefitsDeleted,
                    employeeBenefitsNotIn, tran);

                await connection.ExecuteAsync(EmployeeBenefitsSqlQueries.QueryDeleteEmployeeBenefits,
                    new { Ids = employeeBenefitsNotIn.Select(e => e.Id).ToList() },
                    tran);

                var diariesNotIn = employeeBenefitsNotIn.Select(item => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảng EmployeeBenefits",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "EmployeeBenefits",
                    IsSuccess = true,
                    WithId = item.Id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diariesNotIn, tran);
            }

            var dataToInsert = employeeBenefitsDto?.Where(e => e.Id == Guid.Empty).ToList();
            var dataToUpdate = employeeBenefitsDto?.Where(e => e.Id != Guid.Empty).ToList();

            var employeeBenefits = dataToInsert?.Select(item => new EmployeeBenefits()
                {
                    Id = Guid.NewGuid(),
                    Status = 0,
                    CreatedDate = DateTime.Now,
                    IdEmployee = item.IdEmployee,
                    IdCategoryCompensationBenefits = item.IdCategoryCompensationBenefits,
                    Quantity = item.Quantity
                })
                .ToList();

            await connection.ExecuteAsync(EmployeeBenefitsSqlQueries.QueryInsertEmployeeBenefits, employeeBenefits,
                tran);

            var diaries = employeeBenefits?.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng EmployeeBenefits",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "EmployeeBenefits",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            await connection.ExecuteAsync(EmployeeBenefitsSqlQueries.QueryUpdateEmployeeBenefits,
                dataToUpdate,
                tran);

            diaries = dataToUpdate?.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng EmployeeBenefits",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "EmployeeBenefits",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<EmployeeBenefitsDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
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