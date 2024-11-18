using DPD.HR.Infrastructure.Validation.Models.EmployeeAllowance;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/employeeAllowance")]
public class EmployeeAllowanceController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmployeeAllowanceController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public EmployeeAllowanceController(IUnitOfWork unitOfWork, ILogger<EmployeeAllowanceController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ EmployeeAllowanceController ]=================================================================

    // GET: api/EmployeeAllowance/getEmployeeAllowanceByIdEmployee
    [HttpGet("getEmployeeAllowanceByIdEmployee")]
    public async Task<IActionResult> GetEmployeeAllowanceByIdEmployee(Guid idEmployee, int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.EmployeeAllowance.GetByIdEmployee(idEmployee, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/EmployeeAllowance/insertEmployeeAllowance
    [HttpPost("insertEmployeeAllowance")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertEmployeeAllowance(EmployeeAllowanceModel employeeAllowanceModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.EmployeeAllowance.InsertEmployeeAndAllowance(employeeAllowanceModel.Employee,
            employeeAllowanceModel.IdEmployeeAllowance!, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    #endregion
}