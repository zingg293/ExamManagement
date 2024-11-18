using DPD.HR.Infrastructure.Validation.Models.EmployeeBenefits;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/employeeBenefits")]
public class EmployeeBenefitsController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmployeeBenefitsController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public EmployeeBenefitsController(IUnitOfWork unitOfWork, ILogger<EmployeeBenefitsController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ EmployeeBenefitsController ]=================================================================

    // GET: api/EmployeeBenefits/getListEmployeeBenefits
    [HttpGet("getListEmployeeBenefits")]
    public async Task<IActionResult> GetListEmployeeBenefits(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.EmployeeBenefits.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/EmployeeBenefits/getEmployeeBenefitsById
    [HttpGet("getEmployeeBenefitsById")]
    public async Task<IActionResult> GetEmployeeBenefitsById(Guid idEmployeeBenefits)
    {
        var templateApi = await _unitOfWork.EmployeeBenefits.GetById(idEmployeeBenefits);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/EmployeeBenefits/insertEmployeeBenefits
    [HttpPost("insertEmployeeBenefits")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertEmployeeBenefits(EmployeeBenefitsModel employeeBenefitsModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var employeeBenefitsDto = employeeBenefitsModel.Adapt<EmployeeBenefitsDto>();

        employeeBenefitsDto.Id = Guid.NewGuid();
        employeeBenefitsDto.CreatedDate = DateTime.Now;
        employeeBenefitsDto.Status = 0;

        var result = await _unitOfWork.EmployeeBenefits.Insert(employeeBenefitsDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/EmployeeBenefits/updateEmployeeBenefits
    [HttpPut("updateEmployeeBenefits")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateEmployeeBenefits(List<EmployeeBenefitsModel> employeeBenefitsModels)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var employeeBenefitsDto = employeeBenefitsModels.Adapt<List<EmployeeBenefitsDto>>();

        var result = await _unitOfWork.EmployeeBenefits.UpdateEmployeeBenefits(employeeBenefitsDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/EmployeeBenefits/deleteEmployeeBenefits
    [HttpDelete("deleteEmployeeBenefits")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteEmployeeBenefits(List<Guid> idEmployeeBenefits)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.EmployeeBenefits.RemoveByList(idEmployeeBenefits, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpPut: api/EmployeeBenefits/updateEmployeeAndBenefits
    [HttpPut("updateEmployeeAndBenefits")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateEmployeeAndBenefits(EmployeeAndBenefitsModel employeeAndBenefitsModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var employeeBenefitsDto = employeeAndBenefitsModel.EmployeeBenefitsList!.Adapt<List<EmployeeBenefitsDto>>();

        var result =
            await _unitOfWork.EmployeeBenefits.UpdateEmployeeBenefits(
                employeeBenefitsDto, idUserCurrent,
                nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    #endregion
}