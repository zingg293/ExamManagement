using DPD.HR.Infrastructure.Validation.Models.CategoryVacancies;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/categoryVacancies")]
public class CategoryVacanciesController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryVacanciesController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryVacanciesController(IUnitOfWork unitOfWork, ILogger<CategoryVacanciesController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ CategoryVacanciesController ]=================================================================

    // GET: api/CategoryVacancies/getListCategoryVacanciesApproved
    [HttpGet("getListCategoryVacanciesApproved")]
    public async Task<IActionResult> GetListCategoryVacanciesApproved(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryVacancies.GetVacancyApproved(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPut: api/CategoryVacancies/updateStatusCategoryVacancies
    [HttpPut("updateStatusCategoryVacancies")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateStatusCategoryVacancies(Guid idCategoryVacancy, int status)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.CategoryVacancies.UpdateStatusVacancy(status, idCategoryVacancy, idUserCurrent,
                nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // GET: api/CategoryVacancies/getListCategoryVacancies
    [HttpGet("getListCategoryVacancies")]
    public async Task<IActionResult> GetListCategoryVacancies(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryVacancies.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryVacancies/getCategoryVacanciesById
    [HttpGet("getCategoryVacanciesById")]
    public async Task<IActionResult> GetCategoryVacanciesById(Guid idCategoryVacancies)
    {
        var templateApi = await _unitOfWork.CategoryVacancies.GetById(idCategoryVacancies);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/CategoryVacancies/insertCategoryVacancies
    [HttpPost("insertCategoryVacancies")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertCategoryVacancies(CategoryVacanciesModel categoryVacanciesModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryVacanciesDto = categoryVacanciesModel.Adapt<CategoryVacanciesDto>();

        categoryVacanciesDto.Id = Guid.NewGuid();
        categoryVacanciesDto.CreatedDate = DateTime.Now;
        categoryVacanciesDto.Status = 0;

        var result = await _unitOfWork.CategoryVacancies.Insert(categoryVacanciesDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/CategoryVacancies/updateCategoryVacancies
    [HttpPut("updateCategoryVacancies")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateCategoryVacancies(CategoryVacanciesModel categoryVacanciesModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryVacanciesDto = categoryVacanciesModel.Adapt<CategoryVacanciesDto>();

        var result = await _unitOfWork.CategoryVacancies.Update(categoryVacanciesDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/CategoryVacancies/deleteCategoryVacancies
    [HttpDelete("deleteCategoryVacancies")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteCategoryVacancies(List<Guid> idCategoryVacancies)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.CategoryVacancies.RemoveByList(idCategoryVacancies, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}