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
    public class LecturersRepository : ILecturersRepository
    {
        #region ===[ Private Members ]=============================================================

        private readonly IConfiguration _configuration;

        #endregion

        #region ===[ Constructor ]=================================================================

        public LecturersRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region ===[ LecturersRepository Methods ]==================================================
        public async Task<TemplateApi<LecturersDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var units = (await connection.QueryAsync<Lecturers>(LecturersSqlQueries.QueryGetAllLecturers))
                .ToList();

            return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<LecturersDto>()),
                units.Count);
        }

        public Task<TemplateApi<LecturersDto>> GetAllAvailable(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<TemplateApi<LecturersDto>> GetById(Guid id)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var unit = await connection.QueryFirstOrDefaultAsync<Lecturers>(
                LecturersSqlQueries.QueryGetByIdLecturers, new { Id = id });

            return new Pagination().HandleGetByIdRespond(unit.Adapt<LecturersDto>());
        }

        public Task<TemplateApi<LecturersDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public async Task<TemplateApi<LecturersDto>> Insert(LecturersDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(LecturersSqlQueries.QueryInsertLecturers, model, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã thêm mới bảng Lecturers",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Thêm mới CSDL",
                    Operation = "Create",
                    Table = "Lecturers",
                    IsSuccess = true,
                    WithId = model.ID
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();
                return new TemplateApi<LecturersDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<LecturersDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
        {

            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var Lecturerss =
                    (await connection.QueryAsync<Lecturers>(LecturersSqlQueries.QueryGetLecturersByIds, new { Ids = ids },
                        tran))
                    .ToList();

                await connection.ExecuteAsync(LecturersSqlQueries.QueryInsertLecturersDeleted, Lecturerss, tran);

                await connection.ExecuteAsync(LecturersSqlQueries.QueryDeleteLecturers, new { Ids = ids }, tran);

                var diaries = ids.Select(id => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảng Lecturers",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "Lecturers",
                    IsSuccess = true,
                    WithId = id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

                tran.Commit();
                return new TemplateApi<LecturersDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<LecturersDto>> Update(LecturersDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var Lecturers = model.Adapt<Lecturers>();
                await connection.ExecuteAsync(LecturersSqlQueries.QueryUpdateLecturers, Lecturers, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã cập nhật bảng Lecturers",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Cập nhật CSDL",
                    Operation = "Update",
                    Table = "Lecturers",
                    IsSuccess = true,
                    WithId = model.ID
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();

                return new TemplateApi<LecturersDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
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
