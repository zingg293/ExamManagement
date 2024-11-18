using DPD.HR.Infrastructure.Validation.Models.WorkflowStep;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/workflowStep")]
public class WorkflowStepController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkflowStepController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public WorkflowStepController(IUnitOfWork unitOfWork, ILogger<WorkflowStepController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ WorkflowStepController ]==============================================

    // HttpPut: api/WorkflowStep/cudWorkflowStep
    [HttpPut("cudWorkflowStep")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> CudWorkflowStep(List<WorkflowStepModel> workflowStepModels)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var workflowStepDto = workflowStepModels.Adapt<List<WorkflowStepDto>>();

        var result = await _unitOfWork.WorkflowStep.CUD_WorkflowStep(workflowStepDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // GET: api/WorkflowStep/getListWorkflowStepByIdTemplate
    [HttpGet("getListWorkflowStepByIdTemplate")]
    public async Task<IActionResult> GetListWorkflowStepByIdTemplate(int pageNumber, int pageSize, Guid idTemplate)
    {
        var templateApi = await _unitOfWork.WorkflowStep.GetAllByIdTemplate(pageNumber, pageSize, idTemplate);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/WorkflowStep/getWorkflowStepById
    [HttpGet("getWorkflowStepById")]
    public async Task<IActionResult> GetWorkflowStepById(Guid idWorkflowStep)
    {
        var templateApi = await _unitOfWork.WorkflowStep.GetById(idWorkflowStep);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/WorkflowStep/insertWorkflowStep
    [HttpPost("insertWorkflowStep")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertWorkflowStep(WorkflowStepModel workflowStepModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var workflowStepDto = workflowStepModel.Adapt<WorkflowStepDto>();

        workflowStepDto.Id = Guid.NewGuid();
        workflowStepDto.CreatedDate = DateTime.Now;
        workflowStepDto.Status = 0;

        var result = await _unitOfWork.WorkflowStep.Insert(workflowStepDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/WorkflowStep/updateWorkflowStep
    [HttpPut("updateWorkflowStep")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateWorkflowStep(WorkflowStepModel workflowStepModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var workflowStepDto = workflowStepModel.Adapt<WorkflowStepDto>();

        var result = await _unitOfWork.WorkflowStep.Update(workflowStepDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/WorkflowStep/deleteWorkflowStep
    [HttpDelete("deleteWorkflowStep")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteWorkflowStep(List<Guid> idWorkflowStep)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.WorkflowStep.RemoveByList(idWorkflowStep, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}