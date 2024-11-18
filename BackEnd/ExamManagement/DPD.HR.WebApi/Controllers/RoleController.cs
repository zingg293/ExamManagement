using DPD.HR.Infrastructure.Validation.Models.Role;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/role")]
public class RoleController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RoleController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public RoleController(IUnitOfWork unitOfWork, ILogger<RoleController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ RoleController ]=================================================================

    // GET: api/Role/getListRole
    [HttpGet("getListRole")]
    public async Task<IActionResult> GetListRole(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.Role.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Role/getRoleAndNavigationByIdRole
    [HttpGet("getRoleAndNavigationByIdRole")]
    public async Task<IActionResult> GetRoleAndNavigationByIdRole(Guid idRole)
    {
        var templateApi = await _unitOfWork.Role.GetRoleAndNavigation(idRole);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/Role/insertRoleAndNavigation
    [HttpPost("insertRoleAndNavigation")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertRoleAndNavigation(RoleAndNavigationModel roleAndNavigationModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var roleDto = roleAndNavigationModel.Adapt<RoleDto>();

        roleDto.Id = Guid.NewGuid();
        roleDto.Status = 0;
        roleDto.IsDeleted = false;
        roleDto.IsAdmin = false;
        roleDto.RoleCode = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

        var result = await _unitOfWork.Role.InsertRoleAndNavigationRole(roleDto,
            roleAndNavigationModel.listIdNavigation!, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/Role/updateRoleAndNavigation
    [HttpPut("updateRoleAndNavigation")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateRoleAndNavigation(RoleAndNavigationModel roleAndNavigationModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var roleDto = roleAndNavigationModel.Adapt<RoleDto>();

        var result = await _unitOfWork.Role.UpdateRoleAndNavigationRole(roleDto,
            roleAndNavigationModel.listIdNavigation!, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/Role/deleteRoleAndNavigation
    [HttpDelete("deleteRoleAndNavigation")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteRoleAndNavigation(List<Guid> idRole)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.Role.DeleteRoleAndNavigationRole(idRole, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}