using DPD.HR.Infrastructure.Validation.Models.EmployeeDayOff;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/employeeDayOff")]
public class EmployeeDayOffController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmployeeDayOffController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public EmployeeDayOffController(IUnitOfWork unitOfWork, ILogger<EmployeeDayOffController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ EmployeeDayOffController ]=================================================================

    // GET: api/EmployeeDayOff/getListEmployeeDayOff
    [HttpGet("getListEmployeeDayOff")]
    public async Task<IActionResult> GetListEmployeeDayOff(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.EmployeeDayOff.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/EmployeeDayOff/getEmployeeDayOffById
    [HttpGet("getEmployeeDayOffById")]
    public async Task<IActionResult> GetEmployeeDayOffById(Guid idEmployeeDayOff)
    {
        var templateApi = await _unitOfWork.EmployeeDayOff.GetById(idEmployeeDayOff);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/EmployeeDayOff/insertEmployeeDayOff
    [HttpPost("insertEmployeeDayOff")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertEmployeeDayOff(EmployeeDayOffModel employeeDayOffModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var employeeDayOffDto = employeeDayOffModel.Adapt<EmployeeDayOffDto>();

        employeeDayOffDto.Id = Guid.NewGuid();
        employeeDayOffDto.CreatedDate = DateTime.Now;
        employeeDayOffDto.Status = 0;

        var result = await _unitOfWork.EmployeeDayOff.Insert(employeeDayOffDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/EmployeeDayOff/updateEmployeeDayOff
    [HttpPut("updateEmployeeDayOff")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateEmployeeDayOff(EmployeeDayOffModel employeeDayOffModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var employeeDayOffDto = employeeDayOffModel.Adapt<EmployeeDayOffDto>();

        var result = await _unitOfWork.EmployeeDayOff.Update(employeeDayOffDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/EmployeeDayOff/deleteEmployeeDayOff
    [HttpDelete("deleteEmployeeDayOff")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteEmployeeDayOff(List<Guid> idEmployeeDayOff)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.EmployeeDayOff.RemoveByList(idEmployeeDayOff, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}