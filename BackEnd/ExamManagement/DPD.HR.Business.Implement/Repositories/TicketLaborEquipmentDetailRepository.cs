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

public class TicketLaborEquipmentDetailRepository : ITicketLaborEquipmentDetailRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public TicketLaborEquipmentDetailRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ TicketLaborEquipmentDetailRepository Methods ]==================================================

    public async Task<TemplateApi<TicketLaborEquipmentDetailDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var ticketLaborEquipmentDetails = (await connection.QueryAsync<TicketLaborEquipmentDetail>(
                TicketLaborEquipmentDetailSqlQueries
                    .QueryGetAllTicketLaborEquipmentDetail))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            ticketLaborEquipmentDetails.Select(u => u.Adapt<TicketLaborEquipmentDetailDto>()),
            ticketLaborEquipmentDetails.Count);
    }

    public async Task<TemplateApi<TicketLaborEquipmentDetailDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<TicketLaborEquipmentDetail>(
            TicketLaborEquipmentDetailSqlQueries.QueryGetByIdTicketLaborEquipmentDetail, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<TicketLaborEquipmentDetailDto>());
    }

    public Task<TemplateApi<TicketLaborEquipmentDetailDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<TicketLaborEquipmentDetailDto>> Update(TicketLaborEquipmentDetailDto model,
        Guid idUserCurrent, string fullName)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<TicketLaborEquipmentDetailDto>> Insert(TicketLaborEquipmentDetailDto model,
        Guid idUserCurrent, string fullName)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<TicketLaborEquipmentDetailDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public Task<TemplateApi<TicketLaborEquipmentDetailDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<TicketLaborEquipmentDetailDto>> UpdateTicketLaborEquipmentDetail(
        List<TicketLaborEquipmentDetailDto> ticketLaborEquipmentDetailDto, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var ticketLaborEquipmentDetailsNotInDto =
                (await connection.QueryAsync<TicketLaborEquipmentDetail>(
                    TicketLaborEquipmentDetailSqlQueries.QueryGetTicketLaborEquipmentDetailNotInIds,
                    new
                    {
                        Ids = ticketLaborEquipmentDetailDto.Select(e => e.Id).ToList(),
                        IdTicketLaborEquipment = ticketLaborEquipmentDetailDto?.FirstOrDefault()?.IdTicketLaborEquipment
                    },
                    tran))
                .ToList();

            if (ticketLaborEquipmentDetailsNotInDto.Any())
            {
                await connection.ExecuteAsync(
                    TicketLaborEquipmentDetailSqlQueries.QueryInsertTicketLaborEquipmentDetailDeleted,
                    ticketLaborEquipmentDetailsNotInDto, tran);

                await connection.ExecuteAsync(
                    TicketLaborEquipmentDetailSqlQueries.QueryDeleteTicketLaborEquipmentDetail,
                    new { Ids = ticketLaborEquipmentDetailsNotInDto.Select(e => e.Id).ToList() },
                    tran);

                var diariesNotIn = ticketLaborEquipmentDetailsNotInDto.Select(item => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảng TicketLaborEquipmentDetail",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "TicketLaborEquipmentDetail",
                    IsSuccess = true,
                    WithId = item.Id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diariesNotIn, tran);
            }

            var dataToInsert = ticketLaborEquipmentDetailDto?.Where(e => e.Id == Guid.Empty).ToList();
            var dataToUpdate = ticketLaborEquipmentDetailDto?.Where(e => e.Id != Guid.Empty).ToList();

            var ticketLaborEquipmentDetails = dataToInsert?.Select(item => new TicketLaborEquipmentDetail()
                {
                    Id = Guid.NewGuid(),
                    Status = 0,
                    CreatedDate = DateTime.Now,
                    IdTicketLaborEquipment = item.IdTicketLaborEquipment,
                    IdCategoryLaborEquipment = item.IdCategoryLaborEquipment,
                    IdEmployee = item.IdEmployee,
                    EquipmentCode = item.EquipmentCode,
                    Quantity = item.Quantity,
                    IsCheck = false
                })
                .ToList();

            await connection.ExecuteAsync(TicketLaborEquipmentDetailSqlQueries.QueryInsertTicketLaborEquipmentDetail,
                ticketLaborEquipmentDetails,
                tran);

            var diaries = ticketLaborEquipmentDetails?.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng TicketLaborEquipmentDetail",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "TicketLaborEquipmentDetail",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            await connection.ExecuteAsync(TicketLaborEquipmentDetailSqlQueries.QueryUpdateTicketLaborEquipmentDetail,
                dataToUpdate,
                tran);

            diaries = dataToUpdate?.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng TicketLaborEquipmentDetail",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "TicketLaborEquipmentDetail",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<TicketLaborEquipmentDetailDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0,
                0);
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