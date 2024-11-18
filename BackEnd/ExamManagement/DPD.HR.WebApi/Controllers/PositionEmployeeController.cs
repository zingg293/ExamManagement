using DPD.HR.Infrastructure.Validation.Models.PositionEmployee;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/positionEmployee")]
public class PositionEmployeeController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PositionEmployeeController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public PositionEmployeeController(IUnitOfWork unitOfWork, ILogger<PositionEmployeeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ PositionEmployeeController ]==============================================

    // GET: api/PositionEmployee/getListPositionEmployeeByIdEmployee
    [HttpGet("getListPositionEmployeeByIdEmployee")]
    public async Task<IActionResult> GetListPositionEmployeeByIdEmployee(int pageNumber, int pageSize, Guid idEmployee)
    {
        var templateApi = await _unitOfWork.PositionEmployee.GetListByIdEmployee(pageNumber, pageSize, idEmployee);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/PositionEmployee/getListPositionEmployee
    [HttpGet("getListPositionEmployee")]
    public async Task<IActionResult> GetListPositionEmployee(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.PositionEmployee.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/PositionEmployee/getPositionEmployeeById
    [HttpGet("getPositionEmployeeById")]
    public async Task<IActionResult> GetPositionEmployeeById(Guid idPositionEmployee)
    {
        var templateApi = await _unitOfWork.PositionEmployee.GetById(idPositionEmployee);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/PositionEmployee/insertPositionEmployee
    [HttpPost("insertPositionEmployee")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> InsertPositionEmployee(PositionEmployeeModel positionEmployeeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var positionEmployeeDto = positionEmployeeModel.Adapt<PositionEmployeeDto>();

        positionEmployeeDto.Id = Guid.NewGuid();
        positionEmployeeDto.CreatedDate = DateTime.Now;
        positionEmployeeDto.Status = 0;

        var result = await _unitOfWork.PositionEmployee.Insert(positionEmployeeDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/PositionEmployee/updatePositionEmployee
    [HttpPut("updatePositionEmployee")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdatePositionEmployee(PositionEmployeeModel positionEmployeeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var positionEmployeeDto = positionEmployeeModel.Adapt<PositionEmployeeDto>();

        var result = await _unitOfWork.PositionEmployee.Update(positionEmployeeDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/PositionEmployee/deletePositionEmployee
    [HttpDelete("deletePositionEmployee")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> DeletePositionEmployee(List<Guid> idPositionEmployee)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.PositionEmployee.RemoveByList(idPositionEmployee, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpPost: /api/PositionEmployee/insertPositionEmployeeByList
    [HttpPost("insertPositionEmployeeByList")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> InsertPositionEmployeeByList(List<PositionEmployeeModel> positionEmployeeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var positionEmployeeDto = positionEmployeeModel.Adapt<List<PositionEmployeeDto>>();

        foreach (var item in positionEmployeeDto)
        {
            item.Id = Guid.NewGuid();
            item.CreatedDate = DateTime.Now;
            item.Status = 0;
        }

        var result =
            await _unitOfWork.PositionEmployee.InsertPositionEmployeeByList(positionEmployeeDto, idUserCurrent,
                nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/PositionEmployee/updatePositionEmployeeByList
    [HttpPut("updatePositionEmployeeByList")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdatePositionEmployeeByList(List<PositionEmployeeModel> positionEmployeeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var positionEmployeeDto = positionEmployeeModel.Adapt<List<PositionEmployeeDto>>();

        var result =
            await _unitOfWork.PositionEmployee.UpdatePositionEmployeeByList(positionEmployeeDto, idUserCurrent,
                nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    #endregion
}