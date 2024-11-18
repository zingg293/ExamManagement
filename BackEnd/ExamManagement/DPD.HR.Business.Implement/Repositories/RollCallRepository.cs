using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Implement.Repositories
{
    public class RollCallRepository : IRollCallRepository
    {
        public Task<TemplateApi<RollCallDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<RollCallDto>> GetAllAvailable(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<RollCallDto>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<RollCallDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<RollCallDto>> Insert(RollCallDto model, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<RollCallDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }

        public Task<TemplateApi<RollCallDto>> Update(RollCallDto model, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }
    }
}
