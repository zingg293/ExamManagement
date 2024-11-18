using DPD.HR.Infrastructure.Validation.Models.CategoryLaborEquipment;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/categoryLaborEquipment")]
public class CategoryLaborEquipmentController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryLaborEquipmentController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryLaborEquipmentController(IUnitOfWork unitOfWork, ILogger<CategoryLaborEquipmentController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ CategoryLaborEquipmentController ]=================================================================

    // GET: api/CategoryLaborEquipment/getListCategoryLaborEquipment
    [HttpGet("getListCategoryLaborEquipment")]
    public async Task<IActionResult> GetListCategoryLaborEquipment(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryLaborEquipment.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryLaborEquipment/getCategoryLaborEquipmentById
    [HttpGet("getCategoryLaborEquipmentById")]
    public async Task<IActionResult> GetCategoryLaborEquipmentById(Guid idCategoryLaborEquipment)
    {
        var templateApi = await _unitOfWork.CategoryLaborEquipment.GetById(idCategoryLaborEquipment);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/CategoryLaborEquipment/insertCategoryLaborEquipment
    [HttpPost("insertCategoryLaborEquipment")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertCategoryLaborEquipment(
        CategoryLaborEquipmentModel categoryLaborEquipmentModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryLaborEquipmentDto = categoryLaborEquipmentModel.Adapt<CategoryLaborEquipmentDto>();

        categoryLaborEquipmentDto.Id = Guid.NewGuid();
        categoryLaborEquipmentDto.CreatedDate = DateTime.Now;
        categoryLaborEquipmentDto.Status = 0;
        categoryLaborEquipmentDto.Code = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

        var result =
            await _unitOfWork.CategoryLaborEquipment.Insert(categoryLaborEquipmentDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/CategoryLaborEquipment/updateCategoryLaborEquipment
    [HttpPut("updateCategoryLaborEquipment")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateCategoryLaborEquipment(
        CategoryLaborEquipmentModel categoryLaborEquipmentModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryLaborEquipmentDto = categoryLaborEquipmentModel.Adapt<CategoryLaborEquipmentDto>();

        var result =
            await _unitOfWork.CategoryLaborEquipment.Update(categoryLaborEquipmentDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/CategoryLaborEquipment/deleteCategoryLaborEquipment
    [HttpDelete("deleteCategoryLaborEquipment")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteCategoryLaborEquipment(List<Guid> idCategoryLaborEquipment)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.CategoryLaborEquipment.RemoveByList(idCategoryLaborEquipment, idUserCurrent,
                nameUserCurrent);
        return Ok(result);
    }

    #endregion
}