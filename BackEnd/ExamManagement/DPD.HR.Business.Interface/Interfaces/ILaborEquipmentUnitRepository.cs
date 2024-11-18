using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface ILaborEquipmentUnitRepository : IRepository<LaborEquipmentUnitDto>
{
    Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByUnit(Guid idUnit, int pageNumber, int pageSize);

    Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByListEquipmentCode(List<string> equipmentCode,
        int pageNumber, int pageSize);

    Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByIdTicketLaborEquipment(Guid idTicketLaborEquipment,
        int pageNumber, int pageSize);

    Task<TemplateApi<LaborEquipmentUnitDto>> InsertLaborEquipmentUnitTypeInsert(Guid idTicketLaborEquipment,
        Guid idUserCurrent, string fullName);

    Task<TemplateApi<LaborEquipmentUnitDto>> UpdateStatusLaborEquipmentUnit(Guid idLaborEquipmentUnit, int status,
        Guid idUserCurrent, string fullName);

    Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByUnitAndEmployee(Guid idUserCurrent,
        int pageNumber, int pageSize);

    Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByStatus(int status, int pageNumber, int pageSize);

    Task<TemplateApi<LaborEquipmentUnitDto>> UpdateLaborEquipmentUnitByListIdAndStatus(string equipmentCode, int status,
        Guid idUserCurrent, string fullName);

    Task<TemplateApi<LaborEquipmentUnitAndRelevantInformation>> FilterLaborEquipmentUnit(
        FilterLaborEquipmentUnitModel model, int pageNumber, int pageSize);
}