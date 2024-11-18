using DPD.HR.Infrastructure.Validation.Models.EmployeeType;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/employeeType")]
public class EmployeeTypeController:Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmployeeTypeController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public EmployeeTypeController(IUnitOfWork unitOfWork, ILogger<EmployeeTypeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ EmployeeTypeController ]=================================================================

    // GET: api/EmployeeType/getListEmployeeType
    [HttpGet("getListEmployeeType")]
    public async Task<IActionResult> GetListEmployeeType(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.EmployeeType.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/EmployeeType/getListEmployeeTypeAvailable
    [HttpGet("getListEmployeeTypeAvailable")]
    public async Task<IActionResult> GetListEmployeeTypeAvailable(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.EmployeeType.GetAllAvailable(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/EmployeeType/getEmployeeTypeById
    [HttpGet("getEmployeeTypeById")]
    public async Task<IActionResult> GetEmployeeTypeById(Guid idEmployeeType)
    {
        var templateApi = await _unitOfWork.EmployeeType.GetById(idEmployeeType);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/EmployeeType/insertEmployeeType
    [HttpPost("insertEmployeeType")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertEmployeeType(EmployeeTypeModel employeeTypeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var employeeTypeDto = employeeTypeModel.Adapt<EmployeeTypeDto>();

        employeeTypeDto.Id = Guid.NewGuid();
        employeeTypeDto.CreatedDate = DateTime.Now;
        employeeTypeDto.Status = 0;
        employeeTypeDto.IsActive = false;

        var result = await _unitOfWork.EmployeeType.Insert(employeeTypeDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/EmployeeType/updateEmployeeType
    [HttpPut("updateEmployeeType")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateEmployeeType(EmployeeTypeModel employeeTypeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var employeeTypeDto = employeeTypeModel.Adapt<EmployeeTypeDto>();

        var result = await _unitOfWork.EmployeeType.Update(employeeTypeDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/EmployeeType/deleteEmployeeType
    [HttpDelete("deleteEmployeeType")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteEmployeeType(List<Guid> idEmployeeType)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.EmployeeType.RemoveByList(idEmployeeType, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpPut: /api/EmployeeType/hideEmployeeType
    [HttpPut("hideEmployeeType")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> HideEmployeeType(List<Guid> idEmployeeType, bool isHide)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.EmployeeType.HideByList(idEmployeeType, isHide, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}