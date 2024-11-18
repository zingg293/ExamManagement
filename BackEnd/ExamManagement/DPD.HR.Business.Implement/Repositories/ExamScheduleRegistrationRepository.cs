using CT.EXAMM.Application.Queries.Queries;
using Dapper;
using DPD.HR.Application.Implement.Repositories;
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
    public class ExamScheduleRegistrationRepository : IExamScheduleRegistrationRepository
    {
        #region ===[ Private Members ]=============================================================

        private readonly IConfiguration _configuration;

        #endregion

        #region ===[ Constructor ]=================================================================

        public ExamScheduleRegistrationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion
        public async Task<TemplateApi<ExamScheduleRegistrationDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var units = (await connection.QueryAsync<ExamScheduleRegistration>(ExamScheduleRegistrationSqlQueries.QueryGetAllExamScheduleRegistration))
                .ToList();

            return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<ExamScheduleRegistrationDto>()),
                units.Count);
        }

        public async Task<TemplateApi<ExamScheduleRegistrationDto>> GetAllAvailable(int pageNumber, int pageSize)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var units = (await connection.QueryAsync<ExamScheduleRegistrationRepository>(ExamScheduleRegistrationSqlQueries.QueryGetAllExamScheduleRegistration))
                .ToList();

            return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<ExamScheduleRegistrationDto>()),
                units.Count);
        }

        public Task<TemplateApi<ExamScheduleRegistrationDto>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<ExamScheduleRegistrationDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<ExamScheduleRegistrationDto>> Insert(ExamScheduleRegistrationDto model, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<ExamScheduleRegistrationDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<ExamScheduleRegistrationDto>> Update(ExamScheduleRegistrationDto model, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }
    }
}
