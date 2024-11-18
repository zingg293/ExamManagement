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
    public class TestScheduleRepository : ITestScheduleRepository
    {
        #region ===[ Private Members ]=============================================================

        private readonly IConfiguration _configuration;

        #endregion

        #region ===[ Constructor ]=================================================================

        public TestScheduleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region ===[ TestScheduleRepository Methods ]==================================================
        public async Task<TemplateApi<TestScheduleDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var units = (await connection.QueryAsync<TestSchedule>(TestScheduleSqlQueries.QueryGetAllTestSchedule))
                .ToList();

            return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<TestScheduleDto>()),
                units.Count);
        }

        public Task<TemplateApi<TestScheduleDto>> GetAllAvailable(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<TemplateApi<TestScheduleDto>> GetById(Guid id)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var unit = await connection.QueryFirstOrDefaultAsync<TestSchedule>(
                TestScheduleSqlQueries.QueryGetByIdTestSchedule, new { Id = id });

            return new Pagination().HandleGetByIdRespond(unit.Adapt<TestScheduleDto>());
        }

        public Task<TemplateApi<TestScheduleDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public async Task<TemplateApi<TestScheduleDto>> Insert(TestScheduleDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(TestScheduleSqlQueries.QueryInsertTestSchedule, model, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã thêm mới bảng TestSchedule",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Thêm mới CSDL",
                    Operation = "Create",
                    Table = "TestSchedule",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();
                return new TemplateApi<TestScheduleDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<TestScheduleDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
        {

            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var TestSchedules =
                    (await connection.QueryAsync<TestSchedule>(TestScheduleSqlQueries.QueryGetTestScheduleByIds, new { Ids = ids },
                        tran))
                    .ToList();

                await connection.ExecuteAsync(TestScheduleSqlQueries.QueryInsertTestScheduleDeleted, TestSchedules, tran);

                await connection.ExecuteAsync(TestScheduleSqlQueries.QueryDeleteTestSchedule, new { Ids = ids }, tran);

                var diaries = ids.Select(id => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảng TestSchedule",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "TestSchedule",
                    IsSuccess = true,
                    WithId = id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

                tran.Commit();
                return new TemplateApi<TestScheduleDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<TestScheduleDto>> Update(TestScheduleDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var TestSchedule = model.Adapt<TestSchedule>();
                await connection.ExecuteAsync(TestScheduleSqlQueries.QueryUpdateTestSchedule, TestSchedule, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã cập nhật bảng TestSchedule",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Cập nhật CSDL",
                    Operation = "Update",
                    Table = "TestSchedule",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();

                return new TemplateApi<TestScheduleDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
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
