using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface.Interfaces;

public interface IPromotionTransferRepository : IRepository<PromotionTransferDto>
{
    Task<TemplateApi<PromotionTransferDto>> GetPromotionTransferAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<PromotionTransferDto>> GetPromotionTransferAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);

    Task<TemplateApi<PromotionTransferDto>> GetListPromotionTransfer(int pageNumber, int pageSize, Guid? idUnit,
        Guid? idEmployee);
}