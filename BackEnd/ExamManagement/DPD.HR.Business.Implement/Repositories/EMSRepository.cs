using CT.EXAMM.Application.Queries.Queries;
using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Implement.Repositories
{
    public class EMSRepository : IEMSRepository
    {
        #region ===[ Private Members ]=============================================================

        private readonly IConfiguration _configuration;

        #endregion

        #region ===[ Constructor ]=================================================================

        public EMSRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region ===[EMSRepository Methods ]==================================================
        public async Task<TemplateApi<EMSDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var units = (await connection.QueryAsync<EMS>(EMSSqlQueries.QueryGetAllEMS))
                .ToList();

            return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<EMSDto>()),
                units.Count);
        }

        public Task<TemplateApi<EMSDto>> GetAllAvailable(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<TemplateApi<EMSDto>> GetById(Guid id)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var unit = await connection.QueryFirstOrDefaultAsync<EMS>(
               EMSSqlQueries.QueryGetByIdEMS, new { Id = id });

            return new Pagination().HandleGetByIdRespond(unit.Adapt<EMSDto>());
        }

        public Task<TemplateApi<EMSDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public async Task<TemplateApi<EMSDto>> Insert(EMSDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(EMSSqlQueries.QueryInsertEMS, model, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã thêm mới bảngEMS",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Thêm mới CSDL",
                    Operation = "Create",
                    Table = "EMS",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();
                return new TemplateApi<EMSDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<EMSDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
        {

            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var EMSs =
                    (await connection.QueryAsync<EMS>(EMSSqlQueries.QueryGetEMSByIds, new { Ids = ids },
                        tran))
                    .ToList();

                await connection.ExecuteAsync(EMSSqlQueries.QueryInsertEMSDeleted,EMSs, tran);

                await connection.ExecuteAsync(EMSSqlQueries.QueryDeleteEMS, new { Ids = ids }, tran);

                var diaries = ids.Select(id => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảngEMS",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "EMS",
                    IsSuccess = true,
                    WithId = id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

                tran.Commit();
                return new TemplateApi<EMSDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<EMSDto>> Update(EMSDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var EMS = model.Adapt<EMS>();
                await connection.ExecuteAsync(EMSSqlQueries.QueryUpdateEMS,EMS, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã cập nhật bảngEMS",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Cập nhật CSDL",
                    Operation = "Update",
                    Table = "EMS",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();

                return new TemplateApi<EMSDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
        }
        #endregion

    }
}
