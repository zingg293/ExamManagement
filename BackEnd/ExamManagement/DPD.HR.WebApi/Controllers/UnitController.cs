using DPD.HR.Infrastructure.Validation.Models.Unit;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/unit")]
public class UnitController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UnitController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public UnitController(IUnitOfWork unitOfWork, ILogger<UnitController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ UnitController ]=================================================================

    // GET: api/Unit/getListUnit
    [HttpGet("getListUnit")]
    public async Task<IActionResult> GetListUnit(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.Unit.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Unit/getListUnitAvailable
    [HttpGet("getListUnitAvailable")]
    public async Task<IActionResult> GetListUnitAvailable(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.Unit.GetAllAvailable(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Unit/getListUnitByIdParent
    [HttpGet("getListUnitByIdParent")]
    public async Task<IActionResult> GetListUnitByIdParent(int pageNumber, int pageSize, Guid idParent)
    {
        var templateApi = await _unitOfWork.Unit.GetAllUnitByIdParent(idParent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Unit/getUnitById
    [HttpGet("getUnitById")]
    public async Task<IActionResult> GetUnitById(Guid idUnit)
    {
        var templateApi = await _unitOfWork.Unit.GetById(idUnit);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/Unit/insertUnit
    [HttpPost("insertUnit")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertUnit(UnitRequest unitRequest)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var unitDto = unitRequest.Adapt<UnitDto>();

        unitDto.Id = Guid.NewGuid();
        unitDto.CreatedDate = DateTime.Now;
        unitDto.Status = 0;
        unitDto.IsHide = false;
        unitDto.CreatedBy = idUserCurrent;
        unitDto.UnitCode = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

        var result = await _unitOfWork.Unit.Insert(unitDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/Unit/UpdateUnit
    [HttpPut("updateUnit")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateUnit(UnitRequest unitRequest)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var unitDto = unitRequest.Adapt<UnitDto>();

        var result = await _unitOfWork.Unit.Update(unitDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/Unit/DeleteUnit
    [HttpDelete("deleteUnit")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteUnit(List<Guid> idUnit)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.Unit.RemoveByList(idUnit, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpPut: /api/Unit/HideUnit
    [HttpPut("hideUnit")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> HideUnit(List<Guid> idUnit, bool isHide)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.Unit.HideByList(idUnit, isHide, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}