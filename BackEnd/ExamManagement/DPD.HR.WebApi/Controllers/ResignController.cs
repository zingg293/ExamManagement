using DPD.HR.Infrastructure.Validation.Models.Resign;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/resign")]
public class ResignController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ResignController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public ResignController(IUnitOfWork unitOfWork, ILogger<ResignController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ ResignController ]==============================================

    // HttpGet: api/Resign/getFileResign
    [HttpGet]
    [Route("getFileResign")]
    public IActionResult GetFileResign(string fileNameId)
    {
        var temp = fileNameId.Split('.');
        var fileBytes = System.IO.File.ReadAllBytes(string.Concat(AppSettings.ServerFileResign, "\\",
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

    // GET: api/Resign/getListResignByHistory
    [HttpGet("getListResignByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListResignByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.Resign.GetResignAndWorkFlowByIdUserApproved(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Resign/getListResignByRole
    [HttpGet("getListResignByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListResignByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.Resign.GetResignAndWorkFlow(idUserCurrent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Resign/getListResign
    [HttpGet("getListResign")]
    public async Task<IActionResult> GetListResign(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.Resign.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Resign/getResignById
    [HttpGet("getResignById")]
    public async Task<IActionResult> GetResignById(Guid idResign)
    {
        var templateApi = await _unitOfWork.Resign.GetById(idResign);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/Resign/insertResign
    [HttpPost("insertResign")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertResign([FromForm] ResignModel resignModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var resignDto = resignModel.Adapt<ResignDto>();

        resignDto.Id = Guid.NewGuid();
        resignDto.CreatedDate = DateTime.Now;
        resignDto.Status = 0;
        resignDto.IdUserRequest = idUserCurrent;

        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileResign))
        {
            Directory.CreateDirectory(AppSettings.ServerFileResign);
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                var filename = Path.Combine(AppSettings.ServerFileResign,
                    Path.GetFileName($"{resignDto.Id}.{file.FileName.Split('.')[1]}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                resignDto.ResignForm = file.FileName;
            }
        }

        var codeWorkFlow = AppSettings.Resign;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, resignDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.Resign.Insert(resignDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/Resign/updateResign
    [HttpPut("updateResign")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateResign([FromForm] ResignModel resignModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var resignDto = resignModel.Adapt<ResignDto>();

        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileResign))
        {
            Directory.CreateDirectory(AppSettings.ServerFileResign);
        }

        var getDataById = await _unitOfWork.Resign.GetDataById(resignDto.Id);

        if (getDataById.ResignForm is not null)
        {
            var splitFileName = getDataById?.ResignForm?.Split('.');
            if (splitFileName is not null)
            {
                var lastIndexOfSplit = splitFileName.Length;
                var idFile = getDataById?.Id + "." + splitFileName[lastIndexOfSplit - 1];

                var filename = Path.Combine(AppSettings.ServerFileResign, Path.GetFileName(idFile));

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
                var filename = Path.Combine(AppSettings.ServerFileResign,
                    Path.GetFileName($"{resignDto.Id}.{file.FileName.Split('.')[1]}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                resignDto.ResignForm = file.FileName;
            }
        }

        var result = await _unitOfWork.Resign.Update(resignDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/Resign/deleteResign
    [HttpDelete("deleteResign")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteResign(List<Guid> idResign)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.Resign.RemoveByList(idResign, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}