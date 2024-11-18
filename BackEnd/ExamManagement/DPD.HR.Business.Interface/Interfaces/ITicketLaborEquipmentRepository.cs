using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface ITicketLaborEquipmentRepository : IRepository<TicketLaborEquipmentDto>
{
    Task<TicketLaborEquipment> GetDataById(Guid id);
    Task<TemplateApi<TicketLaborEquipmentDto>> GetTicketLaborEquipmentAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<TicketLaborEquipmentDto>> GetTicketLaborEquipmentAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
}