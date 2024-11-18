using DPD.HR.Infrastructure.Validation.Models.Navigation;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/navigation")]
public class NavigationController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<NavigationController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public NavigationController(IUnitOfWork unitOfWork, ILogger<NavigationController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ NavigationController ]=================================================================

    // GET: api/Navigation/getListNavigationByToken
    [HttpGet("getListNavigationByToken")]
    [Authorize("USER")]
    public async Task<IActionResult> GetListNavigationByToken(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi = await _unitOfWork.Navigation.GetNavigationByIdUser(idUserCurrent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Navigation/getListNavigation
    [HttpGet("getListNavigation")]
    public async Task<IActionResult> GetListNavigation(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.Navigation.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Navigation/getNavigationById
    [HttpGet("getNavigationById")]
    public async Task<IActionResult> GetNavigationById(Guid idNavigation)
    {
        var templateApi = await _unitOfWork.Navigation.GetById(idNavigation);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/Navigation/insertNavigation
    [HttpPost("insertNavigation")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertNavigation(NavigationModel navigationModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var navigationDto = navigationModel.Adapt<NavigationDto>();

        navigationDto.Id = Guid.NewGuid();
        navigationDto.CreatedDate = DateTime.Now;
        navigationDto.Status = 0;

        var result = await _unitOfWork.Navigation.Insert(navigationDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/Navigation/updateNavigation
    [HttpPut("updateNavigation")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateNavigation(NavigationModel navigationModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var navigationDto = navigationModel.Adapt<NavigationDto>();

        var result = await _unitOfWork.Navigation.Update(navigationDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/Navigation/deleteNavigation
    [HttpDelete("deleteNavigation")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteNavigation(List<Guid> idNavigation)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.Navigation.RemoveByList(idNavigation, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}