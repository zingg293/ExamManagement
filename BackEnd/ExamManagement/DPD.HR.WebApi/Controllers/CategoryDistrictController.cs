using DPD.HR.Infrastructure.Validation.Models.CategoryDistrict;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/categoryDistrict")]
public class CategoryDistrictController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryDistrictController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryDistrictController(IUnitOfWork unitOfWork, ILogger<CategoryDistrictController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ CategoryDistrictController ]=================================================================
    
    // GET: api/CategoryDistrict/getCategoryDistrictByCityCode
    [HttpGet("getCategoryDistrictByCityCode")]
    public async Task<IActionResult> GetCategoryDistrictByCityCode(string cityCode, int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryDistrict.GetByCityCode(cityCode, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryDistrict/getListCategoryDistrict
    [HttpGet("getListCategoryDistrict")]
    public async Task<IActionResult> GetListCategoryDistrict(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryDistrict.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryDistrict/getListCategoryDistrictAvailable
    [HttpGet("getListCategoryDistrictAvailable")]
    public async Task<IActionResult> GetListCategoryDistrictAvailable(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryDistrict.GetAllAvailable(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryDistrict/getCategoryDistrictById
    [HttpGet("getCategoryDistrictById")]
    public async Task<IActionResult> GetCategoryDistrictById(Guid idCategoryDistrict)
    {
        var templateApi = await _unitOfWork.CategoryDistrict.GetById(idCategoryDistrict);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/CategoryDistrict/insertCategoryDistrict
    [HttpPost("insertCategoryDistrict")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertCategoryDistrict(CategoryDistrictModel categoryDistrictModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryDistrictDto = categoryDistrictModel.Adapt<CategoryDistrictDto>();

        categoryDistrictDto.Id = Guid.NewGuid();
        categoryDistrictDto.CreatedDate = DateTime.Now;
        categoryDistrictDto.Status = 0;
        categoryDistrictDto.IsHide = false;

        var result = await _unitOfWork.CategoryDistrict.Insert(categoryDistrictDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/CategoryDistrict/updateCategoryDistrict
    [HttpPut("updateCategoryDistrict")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateCategoryDistrict(CategoryDistrictModel categoryDistrictModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryDistrictDto = categoryDistrictModel.Adapt<CategoryDistrictDto>();

        var result = await _unitOfWork.CategoryDistrict.Update(categoryDistrictDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/CategoryDistrict/deleteCategoryDistrict
    [HttpDelete("deleteCategoryDistrict")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteCategoryDistrict(List<Guid> idCategoryDistrict)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.CategoryDistrict.RemoveByList(idCategoryDistrict, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpPut: /api/CategoryDistrict/hideCategoryDistrict
    [HttpPut("hideCategoryDistrict")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> HideCategoryDistrict(List<Guid> idCategoryDistrict, bool isHide)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.CategoryDistrict.HideByList(idCategoryDistrict, isHide, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}