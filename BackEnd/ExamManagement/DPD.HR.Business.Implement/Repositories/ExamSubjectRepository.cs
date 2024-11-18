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
    public class ExamSubjectRepository :IExamSubjecRepository
    {
        #region ===[ Private Members ]=============================================================

        private readonly IConfiguration _configuration;

        #endregion

        #region ===[ Constructor ]=================================================================

        public ExamSubjectRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region ===[ ExamSubjectRepository Methods ]==================================================
        public async Task<TemplateApi<ExamSubjectDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var units = (await connection.QueryAsync<ExamSubject>(ExamSubjectSqlQueries.QueryGetAllExamSubject))
                .ToList();

            return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<ExamSubjectDto>()),
                units.Count);
        }

        public Task<TemplateApi<ExamSubjectDto>> GetAllAvailable(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<TemplateApi<ExamSubjectDto>> GetById(Guid id)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var unit = await connection.QueryFirstOrDefaultAsync<ExamSubject>(
                ExamSubjectSqlQueries.QueryGetByIdExamSubject, new { Id = id });

            return new Pagination().HandleGetByIdRespond(unit.Adapt<ExamSubjectDto>());
        }

        public Task<TemplateApi<ExamSubjectDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public async Task<TemplateApi<ExamSubjectDto>> Insert(ExamSubjectDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(ExamSubjectSqlQueries.QueryInsertExamSubject, model, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã thêm mới bảng ExamSubject",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Thêm mới CSDL",
                    Operation = "Create",
                    Table = "ExamSubject",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();
                return new TemplateApi<ExamSubjectDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<ExamSubjectDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
        {

            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var ExamSubjects =
                    (await connection.QueryAsync<ExamSubject>(ExamSubjectSqlQueries.QueryGetExamSubjectByIds, new { Ids = ids },
                        tran))
                    .ToList();

                await connection.ExecuteAsync(ExamSubjectSqlQueries.QueryInsertExamSubjectDeleted, ExamSubjects, tran);

                await connection.ExecuteAsync(ExamSubjectSqlQueries.QueryDeleteExamSubject, new { Ids = ids }, tran);

                var diaries = ids.Select(id => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảng ExamSubject",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "ExamSubject",
                    IsSuccess = true,
                    WithId = id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

                tran.Commit();
                return new TemplateApi<ExamSubjectDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<ExamSubjectDto>> Update(ExamSubjectDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var ExamSubject = model.Adapt<ExamSubject>();
                await connection.ExecuteAsync(ExamSubjectSqlQueries.QueryUpdateExamSubject, ExamSubject, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã cập nhật bảng ExamSubject",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Cập nhật CSDL",
                    Operation = "Update",
                    Table = "ExamSubject",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();

                return new TemplateApi<ExamSubjectDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
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
