using DPD.HR.Infrastructure.Validation.Models.RequestToHired;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/requestToHired")]
public class RequestToHiredController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RequestToHiredController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public RequestToHiredController(IUnitOfWork unitOfWork, ILogger<RequestToHiredController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ RequestToHiredController ]==============================================

    // GET: api/RequestToHired/filterListRequestToHired
    [HttpGet("filterListRequestToHired")]
    public async Task<IActionResult> FilterListRequestToHired(int pageNumber, int pageSize, Guid? idUnit,
        Guid? idCategoryVacancies)
    {
        var templateApi =
            await _unitOfWork.RequestToHired.GetListRequestToHired(pageNumber,
                pageSize, idUnit, idCategoryVacancies);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/RequestToHired/getListRequestToHireByHistory
    [HttpGet("getListRequestToHireByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListRequestToHireByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.RequestToHired.GetRequestToHireAndWorkFlowByIdUserApproved(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/RequestToHired/getListRequestToHireByRole
    [HttpGet("getListRequestToHireByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListRequestToHireByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.RequestToHired.GetRequestToHireAndWorkFlow(idUserCurrent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpGet: api/RequestToHired/getFileRequestToHired
    [HttpGet]
    [Route("getFileRequestToHired")]
    public IActionResult GetFileRequestToHired(string fileNameId)
    {
        var temp = fileNameId.Split('.');
        var fileBytes = System.IO.File.ReadAllBytes(string.Concat(AppSettings.ServerFileRequestToHire, "\\",
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

    // GET: api/RequestToHired/getListRequestToHired
    [HttpGet("getListRequestToHired")]
    public async Task<IActionResult> GetListRequestToHired(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.RequestToHired.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/RequestToHired/getRequestToHiredById
    [HttpGet("getRequestToHiredById")]
    public async Task<IActionResult> GetRequestToHiredById(Guid idRequestToHired)
    {
        var templateApi = await _unitOfWork.RequestToHired.GetById(idRequestToHired);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/RequestToHired/insertRequestToHired
    [HttpPost("insertRequestToHired")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> InsertRequestToHired([FromForm] RequestToHiredModel requestToHiredModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var requestToHiredDto = requestToHiredModel.Adapt<RequestToHiredDto>();

        requestToHiredDto.Id = Guid.NewGuid();
        requestToHiredDto.CreatedDate = DateTime.Now;
        requestToHiredDto.Status = 0;
        requestToHiredDto.CreatedBy = idUserCurrent;

        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileRequestToHire))
        {
            Directory.CreateDirectory(AppSettings.ServerFileRequestToHire);
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                var filename = Path.Combine(AppSettings.ServerFileRequestToHire,
                    Path.GetFileName($"{requestToHiredDto.Id}.{file.FileName.Split('.')[1]}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                requestToHiredDto.FilePath = file.FileName;
            }
        }

        var codeWorkFlow = AppSettings.RequestToHired;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, requestToHiredDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.RequestToHired.Insert(requestToHiredDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/RequestToHired/updateRequestToHired
    [HttpPut("updateRequestToHired")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateRequestToHired([FromForm] RequestToHiredModel requestToHiredModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var requestToHiredDto = requestToHiredModel.Adapt<RequestToHiredDto>();

        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileRequestToHire))
        {
            Directory.CreateDirectory(AppSettings.ServerFileRequestToHire);
        }

        var getDataById = await _unitOfWork.RequestToHired.GetDataById(requestToHiredDto.Id);

        if (getDataById.FilePath is not null)
        {
            var splitFileName = getDataById?.FilePath?.Split('.');
            if (splitFileName is not null)
            {
                var lastIndexOfSplit = splitFileName.Length;
                var idFile = getDataById?.Id + "." + splitFileName[lastIndexOfSplit - 1];

                var filename = Path.Combine(AppSettings.ServerFileRequestToHire, Path.GetFileName(idFile));

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
                var filename = Path.Combine(AppSettings.ServerFileRequestToHire,
                    Path.GetFileName($"{requestToHiredDto.Id}.{file.FileName.Split('.')[1]}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                requestToHiredDto.FilePath = file.FileName;
            }
        }

        var result = await _unitOfWork.RequestToHired.Update(requestToHiredDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/RequestToHired/deleteRequestToHired
    [HttpDelete("deleteRequestToHired")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> DeleteRequestToHired(List<Guid> idRequestToHired)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.RequestToHired.RemoveByList(idRequestToHired, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}