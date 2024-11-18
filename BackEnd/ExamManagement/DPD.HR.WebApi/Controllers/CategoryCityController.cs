using DPD.HR.Infrastructure.Validation.Models.CategoryCity;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/categoryCity")]
public class CategoryCityController : Controller
{
    #region ===[ Private Members ]=============================================================
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryCityController> _logger;
    #endregion

    #region ===[ Constructor ]=================================================================
    public CategoryCityController(IUnitOfWork unitOfWork, ILogger<CategoryCityController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    #endregion

    #region ===[ CategoryCityController ]==============================================

    // GET: api/CategoryCity/getListCategoryCity
    [HttpGet("getListCategoryCity")]
    public async Task<IActionResult> GetListCategoryCity(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryCity.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryCity/getListCategoryCityAvailable
    [HttpGet("getListCategoryCityAvailable")]
    public async Task<IActionResult> GetListCategoryCityAvailable(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryCity.GetAllAvailable(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryCity/getCategoryCityById
    [HttpGet("getCategoryCityById")]
    public async Task<IActionResult> GetCategoryCityById(Guid idCategoryCity)
    {
        var templateApi = await _unitOfWork.CategoryCity.GetById(idCategoryCity);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/CategoryCity/insertCategoryCity
    [HttpPost("insertCategoryCity")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertCategoryCity(CategoryCityModel categoryCityModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryCityDto = categoryCityModel.Adapt<CategoryCityDto>();

        categoryCityDto.Id = Guid.NewGuid();
        categoryCityDto.CreateDate = DateTime.Now;
        categoryCityDto.Status = 0;
        categoryCityDto.IsHide = false;

        var result = await _unitOfWork.CategoryCity.Insert(categoryCityDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/CategoryCity/updateCategoryCity
    [HttpPut("updateCategoryCity")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateCategoryCity(CategoryCityModel categoryCityModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryCityDto = categoryCityModel.Adapt<CategoryCityDto>();

        var result = await _unitOfWork.CategoryCity.Update(categoryCityDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/CategoryCity/deleteCategoryCity
    [HttpDelete("deleteCategoryCity")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteCategoryCity(List<Guid> idCategoryCity)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.CategoryCity.RemoveByList(idCategoryCity, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpPut: /api/CategoryCity/hideCategoryCity
    [HttpPut("hideCategoryCity")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> HideCategoryCity(List<Guid> idCategoryCity, bool isHide)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.CategoryCity.HideByList(idCategoryCity, isHide, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}