using DPD.HR.Infrastructure.Validation.Models.CategoryCompensationBenefits;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/categoryCompensationBenefits")]
public class CategoryCompensationBenefitsController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryCompensationBenefitsController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryCompensationBenefitsController(IUnitOfWork unitOfWork,
        ILogger<CategoryCompensationBenefitsController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ CategoryCompensationBenefitsController ]=================================================================

    // GET: api/CategoryCompensationBenefits/getListCategoryCompensationBenefits
    [HttpGet("getListCategoryCompensationBenefits")]
    public async Task<IActionResult> GetListCategoryCompensationBenefits(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryCompensationBenefits.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryCompensationBenefits/getCategoryCompensationBenefitsById
    [HttpGet("getCategoryCompensationBenefitsById")]
    public async Task<IActionResult> GetCategoryCompensationBenefitsById(Guid idCategoryCompensationBenefits)
    {
        var templateApi = await _unitOfWork.CategoryCompensationBenefits.GetById(idCategoryCompensationBenefits);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/CategoryCompensationBenefits/insertCategoryCompensationBenefits
    [HttpPost("insertCategoryCompensationBenefits")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertCategoryCompensationBenefits(
        CategoryCompensationBenefitsModel categoryCompensationBenefitsModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryCompensationBenefitsDto =
            categoryCompensationBenefitsModel.Adapt<CategoryCompensationBenefitsDto>();

        categoryCompensationBenefitsDto.Id = Guid.NewGuid();
        categoryCompensationBenefitsDto.CreatedDate = DateTime.Now;
        categoryCompensationBenefitsDto.Status = 0;

        var result =
            await _unitOfWork.CategoryCompensationBenefits.Insert(categoryCompensationBenefitsDto, idUserCurrent,
                nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/CategoryCompensationBenefits/updateCategoryCompensationBenefits
    [HttpPut("updateCategoryCompensationBenefits")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateCategoryCompensationBenefits(
        CategoryCompensationBenefitsModel categoryCompensationBenefitsModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryCompensationBenefitsDto =
            categoryCompensationBenefitsModel.Adapt<CategoryCompensationBenefitsDto>();

        var result =
            await _unitOfWork.CategoryCompensationBenefits.Update(categoryCompensationBenefitsDto, idUserCurrent,
                nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/CategoryCompensationBenefits/deleteCategoryCompensationBenefits
    [HttpDelete("deleteCategoryCompensationBenefits")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteCategoryCompensationBenefits(List<Guid> idCategoryCompensationBenefits)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.CategoryCompensationBenefits.RemoveByList(idCategoryCompensationBenefits, idUserCurrent,
                nameUserCurrent);
        return Ok(result);
    }

    #endregion
}