using DPD.HR.Infrastructure.Validation.Models.BusinessTripEmployee;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/businessTripEmployee")]
public class BusinessTripEmployeeController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BusinessTripEmployeeController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public BusinessTripEmployeeController(IUnitOfWork unitOfWork, ILogger<BusinessTripEmployeeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ BusinessTripEmployeeController ]==============================================

    // GET: api/BusinessTripEmployee/getListBusinessTripEmployeeByIdBusinessTrip
    [HttpGet("getListBusinessTripEmployeeByIdBusinessTrip")]
    public async Task<IActionResult> GetListBusinessTripEmployeeByIdBusinessTrip(int pageNumber, int pageSize,
        Guid idBusinessTrip)
    {
        var templateApi =
            await _unitOfWork.BusinessTripEmployee.GetListByIBusinessTrip(idBusinessTrip, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/BusinessTripEmployee/getListBusinessTripEmployee
    [HttpGet("getListBusinessTripEmployee")]
    public async Task<IActionResult> GetListBusinessTripEmployee(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.BusinessTripEmployee.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/BusinessTripEmployee/getBusinessTripEmployeeById
    [HttpGet("getBusinessTripEmployeeById")]
    public async Task<IActionResult> GetBusinessTripEmployeeById(Guid idBusinessTripEmployee)
    {
        var templateApi = await _unitOfWork.BusinessTripEmployee.GetById(idBusinessTripEmployee);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/BusinessTripEmployee/insertBusinessTripEmployee
    [HttpPost("insertBusinessTripEmployee")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> InsertBusinessTripEmployee(BusinessTripEmployeeModel businessTripEmployeeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var businessTripEmployeeDto = businessTripEmployeeModel.Adapt<BusinessTripEmployeeDto>();

        businessTripEmployeeDto.Id = Guid.NewGuid();
        businessTripEmployeeDto.CreatedDate = DateTime.Now;
        businessTripEmployeeDto.Status = 0;

        var result =
            await _unitOfWork.BusinessTripEmployee.Insert(businessTripEmployeeDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPost: /api/BusinessTripEmployee/insertBusinessTripEmployeeByList
    [HttpPost("insertBusinessTripEmployeeByList")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> InsertBusinessTripEmployeeByList(
        List<BusinessTripEmployeeModel> businessTripEmployeeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var businessTripEmployee = businessTripEmployeeModel.Adapt<List<BusinessTripEmployeeDto>>();

        var result =
            await _unitOfWork.BusinessTripEmployee.InsertBusinessTripEmployeeByList(businessTripEmployee,
                idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/BusinessTripEmployee/updateBusinessTripEmployee
    [HttpPut("updateBusinessTripEmployee")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateBusinessTripEmployee(BusinessTripEmployeeModel businessTripEmployeeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var businessTripEmployeeDto = businessTripEmployeeModel.Adapt<BusinessTripEmployeeDto>();

        var result =
            await _unitOfWork.BusinessTripEmployee.Update(businessTripEmployeeDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/BusinessTripEmployee/deleteBusinessTripEmployee
    [HttpDelete("deleteBusinessTripEmployee")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> DeleteBusinessTripEmployee(List<Guid> idBusinessTripEmployee)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.BusinessTripEmployee.RemoveByList(idBusinessTripEmployee, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}