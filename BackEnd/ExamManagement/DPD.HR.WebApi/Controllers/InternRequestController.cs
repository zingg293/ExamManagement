using DPD.HR.Infrastructure.Validation.Models.InternRequest;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/internRequest")]
public class InternRequestController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<InternRequestController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public InternRequestController(IUnitOfWork unitOfWork, ILogger<InternRequestController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ InternRequestController ]==============================================

    // GET: api/InternRequest/filterListInternRequest
    [HttpPost("filterListInternRequest")]
    public async Task<IActionResult> FilterListInternRequest(FilterInternRequestModel model)
    {
        var templateApi = await _unitOfWork.InternRequest.FilterInternRequest(model, model.PageNumber, model.PageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/InternRequest/getListInternRequestByHistory
    [HttpGet("getListInternRequestByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListInternRequestByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.InternRequest.GetInternRequestAndWorkFlowByIdUserApproved(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/InternRequest/getListInternRequestByRole
    [HttpGet("getListInternRequestByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListInternRequestByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.InternRequest.GetInternRequestAndWorkFlow(idUserCurrent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/InternRequest/getListInternRequest
    [HttpGet("getListInternRequest")]
    public async Task<IActionResult> GetListInternRequest(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.InternRequest.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/InternRequest/getInternRequestById
    [HttpGet("getInternRequestById")]
    public async Task<IActionResult> GetInternRequestById(Guid idInternRequest)
    {
        var templateApi = await _unitOfWork.InternRequest.GetById(idInternRequest);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/InternRequest/insertInternRequest
    [HttpPost("insertInternRequest")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> InsertInternRequest([FromForm] InternRequestModel internRequestModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var internRequestDto = internRequestModel.Adapt<InternRequestDto>();

        internRequestDto.Id = Guid.NewGuid();   
        internRequestDto.CreatedDate = DateTime.Now;
        internRequestDto.Status = 0;
        internRequestDto.IdUserRequest = idUserCurrent;
        
        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileInternRequest))
        {
            Directory.CreateDirectory(AppSettings.ServerFileInternRequest);
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                // Use Path.GetExtension to get the file extension
                var fileExtension = Path.GetExtension(file.FileName);
                
                var filename = Path.Combine(AppSettings.ServerFileInternRequest,
                    Path.GetFileName($"{internRequestDto.Id}.{fileExtension}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                internRequestDto.Attachments = file.FileName;
            }
        }

        var codeWorkFlow = AppSettings.InternRequest;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, internRequestDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.InternRequest.Insert(internRequestDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/InternRequest/updateInternRequest
    [HttpPut("updateInternRequest")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateInternRequest([FromForm] InternRequestModel internRequestModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var internRequestDto = internRequestModel.Adapt<InternRequestDto>();
        
        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileInternRequest))
        {
            Directory.CreateDirectory(AppSettings.ServerFileInternRequest);
        }

        var getDataById = await _unitOfWork.InternRequest.GetDataById(internRequestDto.Id);

        if (getDataById.Attachments is not null)
        {
            // Use Path.GetExtension to get the file extension
            var fileExtension = Path.GetExtension(getDataById?.Attachments);
            
            if (fileExtension is not null)
            {
                var idFile = getDataById?.Id + "." + fileExtension;

                var filename = Path.Combine(AppSettings.ServerFileInternRequest, Path.GetFileName(idFile));

                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
            }
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                // Use Path.GetExtension to get the file extension
                var fileExtension = Path.GetExtension(file.FileName);
                
                var filename = Path.Combine(AppSettings.ServerFileInternRequest,
                    Path.GetFileName($"{internRequestDto.Id}.{fileExtension}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                internRequestDto.Attachments = file.FileName;
            }
        }

        var result = await _unitOfWork.InternRequest.Update(internRequestDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/InternRequest/deleteInternRequest
    [HttpDelete("deleteInternRequest")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> DeleteInternRequest(List<Guid> idInternRequest)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.InternRequest.RemoveByList(idInternRequest, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }
    
    // HttpGet: api/InternRequest/getFileInternRequest
    [HttpGet]
    [Route("getFileInternRequest")]
    public IActionResult GetFileInternRequest(string fileNameId)
    {
        var temp = fileNameId.Split('.');
        var fileBytes = System.IO.File.ReadAllBytes(string.Concat(AppSettings.ServerFileInternRequest, "\\",
            fileNameId));

        switch (temp[1])
        {
            case "jpg":
            case "png":
                return File(fileBytes, "image/jpeg");
            case "docx":
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            case "pdf":
                return File(fileBytes, "application/pdf");
            default:
                return Ok();
        }
    }

    #endregion
}