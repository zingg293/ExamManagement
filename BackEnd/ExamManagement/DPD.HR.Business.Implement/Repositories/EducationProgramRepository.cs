﻿using CT.EXAMM.Application.Queries.Queries;
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
    public class EducationProgramRepository : IEducationProgramRepository
    {
        #region ===[ Private Members ]=============================================================

        private readonly IConfiguration _configuration;

        #endregion

        #region ===[ Constructor ]=================================================================

        public EducationProgramRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region ===[ EducationProgramRepository Methods ]==================================================
        public async Task<TemplateApi<EducationProgramDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var units = (await connection.QueryAsync<EducationProgram>(EducationProgramSqlQueries.QueryGetAllEducationProgram))
                .ToList();

            return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<EducationProgramDto>()),
                units.Count);
        }

        public Task<TemplateApi<EducationProgramDto>> GetAllAvailable(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<TemplateApi<EducationProgramDto>> GetById(Guid id)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var unit = await connection.QueryFirstOrDefaultAsync<EducationProgram>(
                EducationProgramSqlQueries.QueryGetByIdEducationProgram, new { Id = id });

            return new Pagination().HandleGetByIdRespond(unit.Adapt<EducationProgramDto>());
        }

        public Task<TemplateApi<EducationProgramDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public async Task<TemplateApi<EducationProgramDto>> Insert(EducationProgramDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(EducationProgramSqlQueries.QueryInsertEducationProgram, model, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã thêm mới bảng EducationProgram",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Thêm mới CSDL",
                    Operation = "Create",
                    Table = "EducationProgram",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();
                return new TemplateApi<EducationProgramDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<EducationProgramDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
        {

            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var EducationPrograms =
                    (await connection.QueryAsync<EducationProgram>(EducationProgramSqlQueries.QueryGetEducationProgramByIds, new { Ids = ids },
                        tran))
                    .ToList();

                await connection.ExecuteAsync(EducationProgramSqlQueries.QueryInsertEducationProgramDeleted, EducationPrograms, tran);

                await connection.ExecuteAsync(EducationProgramSqlQueries.QueryDeleteEducationProgram, new { Ids = ids }, tran);

                var diaries = ids.Select(id => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảng EducationProgram",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "EducationProgram",
                    IsSuccess = true,
                    WithId = id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

                tran.Commit();
                return new TemplateApi<EducationProgramDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<EducationProgramDto>> Update(EducationProgramDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var EducationProgram = model.Adapt<EducationProgram>();
                await connection.ExecuteAsync(EducationProgramSqlQueries.QueryUpdateEducationProgram, EducationProgram, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã cập nhật bảng EducationProgram",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Cập nhật CSDL",
                    Operation = "Update",
                    Table = "EducationProgram",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();

                return new TemplateApi<EducationProgramDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
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