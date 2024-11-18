using DPD.HR.Infrastructure.Validation.Models.Allowance;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/allowance")]
public class AllowanceController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    //private readonly ILogger<AllowanceController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public AllowanceController(IUnitOfWork unitOfWork
        //ILogger<AllowanceController> logger
    )
    {
        _unitOfWork = unitOfWork;
        //_logger = logger;
    }

    #endregion

    #region ===[ AllowanceController ]=================================================================

    // GET: api/Allowance/getListAllowance
    [HttpGet("getListAllowance")]
    public async Task<IActionResult> GetListAllowance(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.Allowance.GetAllAsync(pageNumber, pageSize);
        //_logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Allowance/getAllowanceById
    [HttpGet("getAllowanceById")]
    public async Task<IActionResult> GetAllowanceById(Guid idAllowance)
    {
        var templateApi = await _unitOfWork.Allowance.GetById(idAllowance);
        //_logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/Allowance/insertAllowance
    [HttpPost("insertAllowance")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertAllowance(AllowanceModel allowanceModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var allowanceDto = allowanceModel.Adapt<AllowanceDto>();

        allowanceDto.Id = Guid.NewGuid();
        allowanceDto.CreatedDate = DateTime.Now;
        allowanceDto.Status = 0;

        var result = await _unitOfWork.Allowance.Insert(allowanceDto, idUserCurrent, nameUserCurrent);

        //_logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/Allowance/updateAllowance
    [HttpPut("updateAllowance")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateAllowance(AllowanceModel allowanceModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var allowanceDto = allowanceModel.Adapt<AllowanceDto>();

        var result = await _unitOfWork.Allowance.Update(allowanceDto, idUserCurrent, nameUserCurrent);
        //_logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/Allowance/deleteAllowance
    [HttpDelete("deleteAllowance")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteAllowance(List<Guid> idAllowance)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.Allowance.RemoveByList(idAllowance, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}