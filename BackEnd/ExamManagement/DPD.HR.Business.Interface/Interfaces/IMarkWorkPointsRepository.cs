using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IMarkWorkPointsRepository
{
    Task<TemplateApi<MarkWorkPointsDto>> MarkWorkPointsEmployee(FilterMarkWorkPointsModel model);
    Task<MarkWorkPointsDto> MarkWorkPointsEmployeeForExcel(FilterMarkWorkPointsModel model);
}