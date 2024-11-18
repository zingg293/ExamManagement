using DPD.HR.Infrastructure.Validation.Models.LaborEquipmentUnit;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/LaborEquipmentUnit")]
public class LaborEquipmentUnitController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LaborEquipmentUnitController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public LaborEquipmentUnitController(IUnitOfWork unitOfWork, ILogger<LaborEquipmentUnitController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ LaborEquipmentUnitController ]==============================================

    // POST: api/LaborEquipmentUnit/filterLaborEquipmentUnitModel
    [HttpPost("filterLaborEquipmentUnitModel")]
    public async Task<IActionResult> FilterLaborEquipmentUnit(FilterLaborEquipmentUnitModel model)
    {
        var templateApi =
            await _unitOfWork.LaborEquipmentUnit.FilterLaborEquipmentUnit(model, model.PageNumber, model.PageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // POST: api/LaborEquipmentUnit/getListLaborEquipmentUnitByListEquipmentCode
    [HttpPost("getListLaborEquipmentUnitByListEquipmentCode")]
    public async Task<IActionResult> GetListLaborEquipmentUnitByListEquipmentCode(int pageNumber, int pageSize,
        List<string> lstEquipmentCode)
    {
        var templateApi =
            await _unitOfWork.LaborEquipmentUnit.GetLaborEquipmentUnitByListEquipmentCode(lstEquipmentCode, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPut: api/LaborEquipmentUnit/updateLaborEquipmentUnitByCodeAndStatus
    [HttpPut("updateLaborEquipmentUnitByCodeAndStatus")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateLaborEquipmentUnitByCodeAndStatus(string equipmentCode, int status)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.LaborEquipmentUnit.UpdateLaborEquipmentUnitByListIdAndStatus(equipmentCode, status,
                idUserCurrent,
                nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // GET: api/LaborEquipmentUnit/getListLaborEquipmentUnitByStatus
    [HttpGet("getListLaborEquipmentUnitByStatus")]
    public async Task<IActionResult> GetListLaborEquipmentByStatus(int pageNumber, int pageSize, int status)
    {
        var templateApi =
            await _unitOfWork.LaborEquipmentUnit.GetLaborEquipmentUnitByStatus(status, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/LaborEquipmentUnit/getListLaborEquipmentUnitByUnitAndEmployee
    [HttpGet("getListLaborEquipmentUnitByUnitAndEmployee")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListLaborEquipmentUnitByUnitAndEmployee(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.LaborEquipmentUnit.GetLaborEquipmentUnitByUnitAndEmployee(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/LaborEquipmentUnit/getListLaborEquipmentUnitByIdLaborEquipment
    [HttpGet("getListLaborEquipmentUnitByIdLaborEquipment")]
    public async Task<IActionResult> GetListLaborEquipmentUnitByIdLaborEquipment(int pageNumber, int pageSize,
        Guid idTicketLaborEquipment)
    {
        var templateApi =
            await _unitOfWork.LaborEquipmentUnit.GetLaborEquipmentUnitByIdTicketLaborEquipment(idTicketLaborEquipment,
                pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/LaborEquipmentUnit/getListLaborEquipmentUnitByIdUnit
    [HttpGet("getListLaborEquipmentUnitByIdUnit")]
    public async Task<IActionResult> GetListLaborEquipmentUnitByIdUnit(int pageNumber, int pageSize, Guid idUnit)
    {
        var templateApi =
            await _unitOfWork.LaborEquipmentUnit.GetLaborEquipmentUnitByUnit(idUnit, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/LaborEquipmentUnit/getListLaborEquipmentUnit
    [HttpGet("getListLaborEquipmentUnit")]
    public async Task<IActionResult> GetListLaborEquipmentUnit(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.LaborEquipmentUnit.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/LaborEquipmentUnit/getLaborEquipmentUnitById
    [HttpGet("getLaborEquipmentUnitById")]
    public async Task<IActionResult> GetLaborEquipmentUnitById(Guid idLaborEquipmentUnit)
    {
        var templateApi = await _unitOfWork.LaborEquipmentUnit.GetById(idLaborEquipmentUnit);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/LaborEquipmentUnit/createLaborEquipmentUnit
    [HttpPost("createLaborEquipmentUnit")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> CreateLaborEquipmentUnit(LaborEquipmentUnitPayload model)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var laborEquipmentUnitDto = model.Adapt<LaborEquipmentUnitDto>();
        laborEquipmentUnitDto.Id = Guid.NewGuid();
        laborEquipmentUnitDto.CreatedDate = DateTime.Now;
        laborEquipmentUnitDto.Status = model.Type;
        laborEquipmentUnitDto.IdTicketLaborEquipment = null;
        laborEquipmentUnitDto.EquipmentCode = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

        var result =
            await _unitOfWork.LaborEquipmentUnit.Insert(
                laborEquipmentUnitDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPost: /api/LaborEquipmentUnit/insertLaborEquipmentUnit
    [HttpPost("insertLaborEquipmentUnit")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertLaborEquipmentUnit(Guid idTicketLaborEquipment)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.LaborEquipmentUnit.InsertLaborEquipmentUnitTypeInsert(
                idTicketLaborEquipment, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/LaborEquipmentUnit/updateStatusLaborEquipmentUnit
    [HttpPut("updateStatusLaborEquipmentUnit")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateStatusLaborEquipmentUnit(Guid idLaborEquipmentUnit, int status)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.LaborEquipmentUnit.UpdateStatusLaborEquipmentUnit(idLaborEquipmentUnit, status,
                idUserCurrent,
                nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    #endregion
}