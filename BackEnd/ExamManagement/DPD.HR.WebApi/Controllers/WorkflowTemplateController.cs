using DPD.HR.Infrastructure.Validation.Models.WorkflowTemplate;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/workflowTemplate")]
public class WorkflowTemplateController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkflowTemplateController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public WorkflowTemplateController(IUnitOfWork unitOfWork, ILogger<WorkflowTemplateController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ WorkflowTemplateController ]==============================================

    // GET: api/WorkflowTemplate/getListWorkflowTemplate
    [HttpGet("getListWorkflowTemplate")]
    public async Task<IActionResult> GetListWorkflowTemplate(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.WorkflowTemplate.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/WorkflowTemplate/getWorkflowTemplateById
    [HttpGet("getWorkflowTemplateById")]
    public async Task<IActionResult> GetWorkflowTemplateById(Guid idWorkflowTemplate)
    {
        var templateApi = await _unitOfWork.WorkflowTemplate.GetById(idWorkflowTemplate);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPut: api/WorkflowTemplate/updateWorkflowTemplate
    [HttpPut("updateWorkflowTemplate")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateWorkflowTemplate(WorkflowTemplateModel workflowTemplateModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var workflowTemplateDto = workflowTemplateModel.Adapt<WorkflowTemplateDto>();

        var result = await _unitOfWork.WorkflowTemplate.Update(workflowTemplateDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPost: /api/WorkflowTemplate/insertWorkflowTemplate
    [HttpPost("insertWorkflowTemplate")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertWorkflowTemplate(WorkflowTemplateModel workflowTemplateModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var workflowTemplateDto = workflowTemplateModel.Adapt<WorkflowTemplateDto>();

        workflowTemplateDto.Id = Guid.NewGuid();
        workflowTemplateDto.CreatedDate = DateTime.Now;
        workflowTemplateDto.Status = 0;

        switch (workflowTemplateModel.WorkflowCode)
        {
            case AppSettings.RequestToHired:
                workflowTemplateDto.TableName = "RequestToHired";
                break;
            case AppSettings.TicketLaborEquipment:
                workflowTemplateDto.TableName = "TicketLaborEquipment";
                break;
            case AppSettings.PromotionTransfer:
                workflowTemplateDto.TableName = "PromotionTransfer";
                break;
            case AppSettings.InternRequest:
                workflowTemplateDto.TableName = "InternRequest";
                break;
            case AppSettings.OnLeave:
                workflowTemplateDto.TableName = "OnLeave";
                break;
            case AppSettings.Overtime:
                workflowTemplateDto.TableName = "Overtime";
                break;
            case AppSettings.Resign:
                workflowTemplateDto.TableName = "Resign";
                break;
            case AppSettings.BusinessTrip:
                workflowTemplateDto.TableName = "BusinessTrip";
                break;
        }

        var result = await _unitOfWork.WorkflowTemplate.Insert(workflowTemplateDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/WorkflowTemplate/deleteWorkflowTemplate
    [HttpDelete("deleteWorkflowTemplate")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteWorkflowTemplate(List<Guid> idWorkflowTemplate)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.WorkflowTemplate.RemoveByList(idWorkflowTemplate, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}