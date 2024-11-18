using DPD.HR.Infrastructure.Validation.Models.TicketLaborEquipmentDetail;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/ticketLaborEquipmentDetail")]
public class TicketLaborEquipmentDetailController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TicketLaborEquipmentDetailController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public TicketLaborEquipmentDetailController(IUnitOfWork unitOfWork,
        ILogger<TicketLaborEquipmentDetailController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ TicketLaborEquipmentDetailController ]==============================================

    // HttpPut: api/TicketLaborEquipmentDetail/updateTicketLaborEquipmentDetail
    [HttpPut("updateTicketLaborEquipmentDetail")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateTicketLaborEquipmentDetail(
        List<TicketLaborEquipmentDetailModel> ticketLaborEquipmentDetailModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var ticketLaborEquipmentDetail = ticketLaborEquipmentDetailModel.Adapt<List<TicketLaborEquipmentDetailDto>>();

        var result =
            await _unitOfWork.TicketLaborEquipmentDetail.UpdateTicketLaborEquipmentDetail(ticketLaborEquipmentDetail,
                idUserCurrent,
                nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    #endregion
}