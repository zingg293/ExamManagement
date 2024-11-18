using DPD.HR.Infrastructure.Validation.Models.TypeDayOff;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/typeDayOff")]
public class TypeDayOffController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TypeDayOffController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public TypeDayOffController(IUnitOfWork unitOfWork, ILogger<TypeDayOffController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ TypeDayOffController ]==============================================

    // GET: api/TypeDayOff/getListTypeDayOff
    [HttpGet("getListTypeDayOff")]
    public async Task<IActionResult> GetListTypeDayOff(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.TypeDayOff.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/TypeDayOff/getTypeDayOffById
    [HttpGet("getTypeDayOffById")]
    public async Task<IActionResult> GetTypeDayOffById(Guid idTypeDayOff)
    {
        var templateApi = await _unitOfWork.TypeDayOff.GetById(idTypeDayOff);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/TypeDayOff/insertTypeDayOff
    [HttpPost("insertTypeDayOff")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertTypeDayOff(TypeDayOffModel typeDayOffModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var typeDayOffDto = typeDayOffModel.Adapt<TypeDayOffDto>();

        typeDayOffDto.Id = Guid.NewGuid();
        typeDayOffDto.CreatedDate = DateTime.Now;
        typeDayOffDto.Status = 0;

        var result = await _unitOfWork.TypeDayOff.Insert(typeDayOffDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/TypeDayOff/updateTypeDayOff
    [HttpPut("updateTypeDayOff")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateTypeDayOff(TypeDayOffModel typeDayOffModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var typeDayOffDto = typeDayOffModel.Adapt<TypeDayOffDto>();

        var result = await _unitOfWork.TypeDayOff.Update(typeDayOffDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/TypeDayOff/deleteTypeDayOff
    [HttpDelete("deleteTypeDayOff")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteTypeDayOff(List<Guid> idTypeDayOff)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.TypeDayOff.RemoveByList(idTypeDayOff, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }
    #endregion
}