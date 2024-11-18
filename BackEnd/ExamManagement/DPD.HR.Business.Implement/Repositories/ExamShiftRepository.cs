using CT.EXAMM.Application.Queries.Queries;
using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface;
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
    public class ExamShiftRepository : IExamShiftRepository
    {
        public Task<TemplateApi<ExamShiftDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<ExamShiftDto>> GetAllAvailable(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<ExamShiftDto>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<ExamShiftDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<ExamShiftDto>> Insert(ExamShiftDto model, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<ExamShiftDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<ExamShiftDto>> Update(ExamShiftDto model, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }
    }
}
