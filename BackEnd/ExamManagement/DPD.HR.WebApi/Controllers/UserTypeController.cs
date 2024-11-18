using DPD.HR.Infrastructure.Validation.Models.UserType;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/userType")]
public class UserTypeController:Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserTypeController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public UserTypeController(IUnitOfWork unitOfWork, ILogger<UserTypeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ UserTypeController ]=================================================================

    // GET: api/UserType/getListUserType
    [HttpGet("getListUserType")]
    public async Task<IActionResult> GetListUserType(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.UserType.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/UserType/getUserTypeById
    [HttpGet("getUserTypeById")]
    public async Task<IActionResult> GetUserTypeById(Guid idUserType)
    {
        var templateApi = await _unitOfWork.UserType.GetById(idUserType);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/UserType/insertUserType
    [HttpPost("insertUserType")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertUserType(UserTypeModel userTypeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var userTypeDto = userTypeModel.Adapt<UserTypeDto>();

        userTypeDto.Id = Guid.NewGuid();
        userTypeDto.CreatedDate = DateTime.Now;
        userTypeDto.Status = 0;
        userTypeDto.CreateBy = idUserCurrent;
        userTypeDto.TypeCode = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

        var result = await _unitOfWork.UserType.Insert(userTypeDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/UserType/updateUserType
    [HttpPut("updateUserType")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateUserType(UserTypeModel userTypeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var userTypeDto = userTypeModel.Adapt<UserTypeDto>();

        var result = await _unitOfWork.UserType.Update(userTypeDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/UserType/deleteUserType
    [HttpDelete("deleteUserType")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteUserType(List<Guid> idUserType)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.UserType.RemoveByList(idUserType, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}