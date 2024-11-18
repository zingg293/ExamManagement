using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IPositionEmployeeRepository : IRepository<PositionEmployeeDto>
{
    Task<TemplateApi<PositionEmployeeDto>> GetListByIdEmployee(int pageNumber, int pageSize, Guid idEmployee);
    Task<TemplateApi<PositionEmployeeDto>> InsertPositionEmployeeByList(List<PositionEmployeeDto> models, Guid idUserCurrent, string fullName);
    Task<TemplateApi<PositionEmployeeDto>> UpdatePositionEmployeeByList(List<PositionEmployeeDto> models, Guid idUserCurrent, string fullName);
}