using DPD.HR.Infrastructure.Validation.Models.CategoryWard;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/categoryWard")]
public class CategoryWardController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryWardController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryWardController(IUnitOfWork unitOfWork, ILogger<CategoryWardController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ CategoryWardController ]=================================================================

    // GET: api/CategoryWard/getCategoryWardByDistrictCode
    [HttpGet("getCategoryWardByDistrictCode")]
    public async Task<IActionResult> GetCategoryWardByDistrictCode(string districtCode, int pageNumber, int pageSize)
    {
        var templateApi =
            await _unitOfWork.CategoryWard.GetCategoryWardByDistrictCode(districtCode, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryWard/getListCategoryWard
    [HttpGet("getListCategoryWard")]
    public async Task<IActionResult> GetListCategoryWard(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryWard.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryWard/getListCategoryWardAvailable
    [HttpGet("getListCategoryWardAvailable")]
    public async Task<IActionResult> GetListCategoryWardAvailable(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryWard.GetAllAvailable(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryWard/getCategoryWardById
    [HttpGet("getCategoryWardById")]
    public async Task<IActionResult> GetCategoryWardById(Guid idCategoryWard)
    {
        var templateApi = await _unitOfWork.CategoryWard.GetById(idCategoryWard);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/CategoryWard/insertCategoryWard
    [HttpPost("insertCategoryWard")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertCategoryWard(CategoryWardModel categoryWardModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryWardDto = categoryWardModel.Adapt<CategoryWardDto>();

        categoryWardDto.Id = Guid.NewGuid();
        categoryWardDto.CreatedDate = DateTime.Now;
        categoryWardDto.Status = 0;
        categoryWardDto.IsHide = false;

        var result = await _unitOfWork.CategoryWard.Insert(categoryWardDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/CategoryWard/updateCategoryWard
    [HttpPut("updateCategoryWard")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateCategoryWard(CategoryWardModel categoryWardModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryWardDto = categoryWardModel.Adapt<CategoryWardDto>();

        var result = await _unitOfWork.CategoryWard.Update(categoryWardDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/CategoryWard/deleteCategoryWard
    [HttpDelete("deleteCategoryWard")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteCategoryWard(List<Guid> idCategoryWard)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.CategoryWard.RemoveByList(idCategoryWard, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpPut: /api/CategoryWard/hideCategoryWard
    [HttpPut("hideCategoryWard")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> HideCategoryWard(List<Guid> idCategoryWard, bool isHide)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.CategoryWard.HideByList(idCategoryWard, isHide, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}