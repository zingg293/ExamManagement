using System.Globalization;
using DPD.HR.Infrastructure.Validation.Models.Overtime;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/overtime")]
public class OvertimeController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OvertimeController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public OvertimeController(IUnitOfWork unitOfWork, ILogger<OvertimeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ OvertimeController ]==============================================

    // GET: api/Overtime/filterOverTime
    [HttpPost("filterOverTime")]
    public async Task<IActionResult> FilterOverTime(FilterOverTimeModel model)
    {
        var templateApi = await _unitOfWork.Overtime.FilterOverTime(model, model.PageNumber, model.PageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Overtime/getListOvertimeByHistory
    [HttpGet("getListOvertimeByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListOvertimeByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.Overtime.GetOvertimeDAndWorkFlowByIdUserApproved(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Overtime/getListOvertimeByRole
    [HttpGet("getListOvertimeByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListOvertimeByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.Overtime.GetOvertimeAndWorkFlow(idUserCurrent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Overtime/getListOvertime
    [HttpGet("getListOvertime")]
    public async Task<IActionResult> GetListOvertime(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.Overtime.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Overtime/getOvertimeById
    [HttpGet("getOvertimeById")]
    public async Task<IActionResult> GetOvertimeById(Guid idOvertime)
    {
        var templateApi = await _unitOfWork.Overtime.GetById(idOvertime);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/Overtime/insertOvertime
    [HttpPost("insertOvertime")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertOvertime(OvertimeModel overtimeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var startDay = DateTime.Now;
        var endDay = DateTime.Now;
        if (overtimeModel.FromDate != null)
        {
            startDay =
                DateTime.ParseExact(overtimeModel.FromDate, "dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("vi-VN"));
        }

        if (overtimeModel.ToDate != null)
        {
            endDay =
                DateTime.ParseExact(overtimeModel.ToDate, "dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("vi-VN"));
        }

        var overtimeDto = new OvertimeDto
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            Status = 0,
            IdUserRequest = idUserCurrent,
            FromDate = startDay,
            ToDate = endDay,
            Description = overtimeModel.Description,
            IdEmployee = overtimeModel.IdEmployee,
            IdUnit = overtimeModel.IdUnit,
            UnitName = overtimeModel.UnitName
        };

        var codeWorkFlow = AppSettings.Overtime;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, overtimeDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.Overtime.Insert(overtimeDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/Overtime/updateOvertime
    [HttpPut("updateOvertime")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateOvertime(OvertimeModel overtimeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var startDay = DateTime.Now;
        var endDay = DateTime.Now;
        if (overtimeModel.FromDate != null)
        {
            startDay =
                DateTime.ParseExact(overtimeModel.FromDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        if (overtimeModel.ToDate != null)
        {
            endDay =
                DateTime.ParseExact(overtimeModel.ToDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        var overtimeDto = new OvertimeDto
        {
            Id = overtimeModel.Id ?? Guid.Empty,
            IdUserRequest = idUserCurrent,
            FromDate = startDay,
            ToDate = endDay,
            Description = overtimeModel.Description,
            IdEmployee = overtimeModel.IdEmployee,
            IdUnit = overtimeModel.IdUnit,
            UnitName = overtimeModel.UnitName
        };

        var result = await _unitOfWork.Overtime.Update(overtimeDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/Overtime/deleteOvertime
    [HttpDelete("deleteOvertime")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteOvertime(List<Guid> idOvertime)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.Overtime.RemoveByList(idOvertime, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}