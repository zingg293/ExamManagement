using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface ITicketLaborEquipmentDetailRepository : IRepository<TicketLaborEquipmentDetailDto>
{
    Task<TemplateApi<TicketLaborEquipmentDetailDto>> UpdateTicketLaborEquipmentDetail(List<TicketLaborEquipmentDetailDto> ticketLaborEquipmentDetailDto,
        Guid idUserCurrent, string fullName);
}