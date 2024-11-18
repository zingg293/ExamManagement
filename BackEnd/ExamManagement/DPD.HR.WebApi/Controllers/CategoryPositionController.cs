using DPD.HR.Infrastructure.Validation.Models.CategoryPosition;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/categoryPosition")]
public class CategoryPositionController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryPositionController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryPositionController(IUnitOfWork unitOfWork, ILogger<CategoryPositionController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ CategoryPositionController ]==============================================

    // GET: api/CategoryPosition/getListCategoryPosition
    [HttpGet("getListCategoryPosition")]
    public async Task<IActionResult> GetListCategoryPosition(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryPosition.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryPosition/getListCategoryPositionAvailable
    [HttpGet("getListCategoryPositionAvailable")]
    public async Task<IActionResult> GetListCategoryPositionAvailable(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryPosition.GetAllAvailable(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryPosition/getCategoryPositionById
    [HttpGet("getCategoryPositionById")]
    public async Task<IActionResult> GetCategoryPositionById(Guid idCategoryPosition)
    {
        var templateApi = await _unitOfWork.CategoryPosition.GetById(idCategoryPosition);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/CategoryPosition/insertCategoryPosition
    [HttpPost("insertCategoryPosition")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertCategoryPosition(CategoryPositionModel categoryPositionModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryPositionDto = categoryPositionModel.Adapt<CategoryPositionDto>();

        categoryPositionDto.Id = Guid.NewGuid();
        categoryPositionDto.CreatedDate = DateTime.Now;
        categoryPositionDto.Status = 0;
        categoryPositionDto.IsActive = false;

        var result = await _unitOfWork.CategoryPosition.Insert(categoryPositionDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/CategoryPosition/updateCategoryPosition
    [HttpPut("updateCategoryPosition")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateCategoryPosition(CategoryPositionModel categoryPositionModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryPositionDto = categoryPositionModel.Adapt<CategoryPositionDto>();

        var result = await _unitOfWork.CategoryPosition.Update(categoryPositionDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/CategoryPosition/deleteCategoryPosition
    [HttpDelete("deleteCategoryPosition")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteCategoryPosition(List<Guid> idCategoryPosition)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.CategoryPosition.RemoveByList(idCategoryPosition, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpPut: /api/CategoryPosition/hideCategoryPosition
    [HttpPut("hideCategoryPosition")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> HideCategoryPosition(List<Guid> idCategoryPosition, bool isHide)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.CategoryPosition.HideByList(idCategoryPosition, isHide, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}