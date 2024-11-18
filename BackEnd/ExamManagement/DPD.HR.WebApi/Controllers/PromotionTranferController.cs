using DPD.HR.Infrastructure.Validation.Models.PromotionTransfer;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/promotionTransfer")]
public class PromotionTransferController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PromotionTransferController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public PromotionTransferController(IUnitOfWork unitOfWork, ILogger<PromotionTransferController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ PromotionTransferController ]==============================================

    // GET: api/PromotionTransfer/filterPromotionTransfer
    [HttpGet("filterPromotionTransfer")]
    public async Task<IActionResult> FilterPromotionTransfer(int pageNumber, int pageSize, Guid? idUnit,
        Guid? idEmployee)
    {
        var templateApi =
            await _unitOfWork.PromotionTransfer.GetListPromotionTransfer(
                pageNumber,
                pageSize, idUnit, idEmployee);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/PromotionTransfer.cs/getListPromotionTransferByHistory
    [HttpGet("getListPromotionTransferByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListPromotionTransferByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.PromotionTransfer.GetPromotionTransferAndWorkFlowByIdUserApproved(idUserCurrent,
                pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/PromotionTransfer.cs/getListPromotionTransferByRole
    [HttpGet("getListPromotionTransferByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListPromotionTransferByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.PromotionTransfer.GetPromotionTransferAndWorkFlow(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/PromotionTransfer.cs/getListPromotionTransfer
    [HttpGet("getListPromotionTransfer")]
    public async Task<IActionResult> GetListPromotionTransfer(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.PromotionTransfer.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/PromotionTransfer.cs/getPromotionTransferById
    [HttpGet("getPromotionTransferById")]
    public async Task<IActionResult> GetPromotionTransferById(Guid idPromotionTransfer)
    {
        var templateApi = await _unitOfWork.PromotionTransfer.GetById(idPromotionTransfer);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/PromotionTransfer.cs/insertPromotionTransfer
    [HttpPost("insertPromotionTransfer")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> InsertPromotionTransfer(PromotionTransferModel promotionTransferModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var promotionTransferDto = promotionTransferModel.Adapt<PromotionTransferDto>();

        promotionTransferDto.Id = Guid.NewGuid();
        promotionTransferDto.CreatedDate = DateTime.Now;
        promotionTransferDto.Status = 0;
        promotionTransferDto.IdUserRequest = idUserCurrent;

        promotionTransferDto.IdPositionEmployeeCurrent = promotionTransferDto.IdPositionEmployeeCurrent == Guid.Empty
            ? null
            : promotionTransferDto.IdPositionEmployeeCurrent;

        promotionTransferDto.PositionNameCurrent = promotionTransferDto.IdPositionEmployeeCurrent == Guid.Empty
            ? null
            : promotionTransferDto.PositionNameCurrent;

        promotionTransferDto.IdCategoryPosition = promotionTransferDto.IdCategoryPosition == Guid.Empty
            ? null
            : promotionTransferDto.IdCategoryPosition;

        promotionTransferDto.NameCategoryPosition = promotionTransferDto.IdCategoryPosition == Guid.Empty
            ? null
            : promotionTransferDto.NameCategoryPosition;

        var codeWorkFlow = AppSettings.PromotionTransfer;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, promotionTransferDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.PromotionTransfer.Insert(promotionTransferDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/PromotionTransfer.cs/updatePromotionTransfer
    [HttpPut("updatePromotionTransfer")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdatePromotionTransfer(PromotionTransferModel promotionTransferModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var promotionTransferDto = promotionTransferModel.Adapt<PromotionTransferDto>();

        var result = await _unitOfWork.PromotionTransfer.Update(promotionTransferDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/PromotionTransfer.cs/deletePromotionTransfer
    [HttpDelete("deletePromotionTransfer")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> DeletePromotionTransfer(List<Guid> idPromotionTransfer)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.PromotionTransfer.RemoveByList(idPromotionTransfer, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}