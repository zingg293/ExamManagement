using DPD.HR.Infrastructure.Validation.Models.RequestToHired;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/workFlow")]
public class WorkFlowController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkFlowController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public WorkFlowController(IUnitOfWork unitOfWork, ILogger<WorkFlowController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ RequestToHiredController ]==============================================

    // HttpPut: api/WorkFlow/updateStepWorkFlow
    [HttpPut("updateStepWorkFlow")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateStepWorkFlow(UpdateStepRequestToHiredMode model)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.WorkFlow.UpdateStepWorkFlow(model.IdWorkFlowInstance, model.IsTerminated,
                model.IsRequestToChange, model.Message, idUserCurrent,
                nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // GET: api/WorkFlow/getListWorkflowHistoriesByIdInstance
    [HttpGet("getListWorkflowHistoriesByIdInstance")]
    public async Task<IActionResult> GetListWorkflowHistoriesByIdInstance(int pageNumber, int pageSize,
        Guid idWorkFlowInstance)
    {
        var templateApi =
            await _unitOfWork.WorkFlowHistory.GetWorkFlowHistoryByIdInstance(idWorkFlowInstance, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    #endregion
}