using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces
{
    public interface IUnitRepository : IRepository<UnitDto>
    {
        #region ===[ CRUD TABLE UNIT ]==================================================
        Task<TemplateApi<UnitDto>> GetAllUnitByIdParent(Guid idUnitParent, int pageNumber, int pageSize);
        Task<UnitDto> GetUnitByUnitCode(string unitCode);

        #endregion
    }
}