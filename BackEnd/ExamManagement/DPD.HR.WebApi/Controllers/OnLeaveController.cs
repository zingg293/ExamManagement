using System.Globalization;
using DPD.HR.Infrastructure.Validation.Models.OnLeave;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/onLeave")]
public class OnLeaveController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OnLeaveController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public OnLeaveController(IUnitOfWork unitOfWork, ILogger<OnLeaveController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ OnLeaveController ]==============================================

    // GET: api/OnLeave/filterListOnLeave
    [HttpPost("filterListOnLeave")]
    public async Task<IActionResult> FilterListOnLeave(FilterOnLeaveModel model)
    {
        var templateApi = await _unitOfWork.OnLeave.FilterOnLeave(model, model.PageNumber, model.PageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpGet: api/OnLeave/getFileOnLeave
    [HttpGet]
    [Route("getFileOnLeave")]
    public IActionResult GetFileOnLeave(string fileNameId)
    {
        var temp = fileNameId.Split('.');
        var fileBytes = System.IO.File.ReadAllBytes(string.Concat(AppSettings.ServerFileOnLeave, "\\",
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

    // GET: api/OnLeave/getListOnLeaveByHistory
    [HttpGet("getListOnLeaveByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListOnLeaveByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.OnLeave.GetOnLeaveAndWorkFlowByIdUserApproved(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/OnLeave/getListOnLeaveByRole
    [HttpGet("getListOnLeaveByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListOnLeaveByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.OnLeave.GetOnLeaveAndWorkFlow(idUserCurrent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/OnLeave/getListOnLeave
    [HttpGet("getListOnLeave")]
    public async Task<IActionResult> GetListOnLeave(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.OnLeave.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/OnLeave/getOnLeaveById
    [HttpGet("getOnLeaveById")]
    public async Task<IActionResult> GetOnLeaveById(Guid idOnLeave)
    {
        var templateApi = await _unitOfWork.OnLeave.GetById(idOnLeave);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/OnLeave/insertOnLeave
    [HttpPost("insertOnLeave")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertOnLeave([FromForm] OnLeaveModel onLeaveModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var startDay = DateTime.Now;
        var endDay = DateTime.Now;
        if (onLeaveModel.FromDate != null)
        {
            startDay =
                DateTime.ParseExact(onLeaveModel.FromDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        if (onLeaveModel.ToDate != null)
        {
            endDay =
                DateTime.ParseExact(onLeaveModel.ToDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        var onLeaveDto = new OnLeaveDto
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            Status = 0,
            IdUserRequest = idUserCurrent,
            FromDate = startDay,
            ToDate = endDay,
            Description = onLeaveModel.Description,
            IdEmployee = onLeaveModel.IdEmployee,
            IdUnit = onLeaveModel.IdUnit,
            UnitName = onLeaveModel.UnitName,
            UnPaidLeave = onLeaveModel.UnPaidLeave
        };

        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileOnLeave))
        {
            Directory.CreateDirectory(AppSettings.ServerFileOnLeave);
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                var filename = Path.Combine(AppSettings.ServerFileOnLeave,
                    Path.GetFileName($"{onLeaveDto.Id}.{file.FileName.Split('.')[1]}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                onLeaveDto.Attachments = file.FileName;
            }
        }

        var codeWorkFlow = AppSettings.OnLeave;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, onLeaveDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.OnLeave.Insert(onLeaveDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/OnLeave/updateOnLeave
    [HttpPut("updateOnLeave")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateOnLeave([FromForm] OnLeaveModel onLeaveModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;


        var startDay = DateTime.Now;
        var endDay = DateTime.Now;
        if (onLeaveModel.FromDate != null)
        {
            startDay =
                DateTime.ParseExact(onLeaveModel.FromDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        if (onLeaveModel.ToDate != null)
        {
            endDay =
                DateTime.ParseExact(onLeaveModel.ToDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        var onLeaveDto = new OnLeaveDto
        {
            Id = onLeaveModel.Id ?? Guid.Empty,
            FromDate = startDay,
            ToDate = endDay,
            Description = onLeaveModel.Description,
            IdEmployee = onLeaveModel.IdEmployee,
            IdUnit = onLeaveModel.IdUnit,
            UnitName = onLeaveModel.UnitName,
            UnPaidLeave = onLeaveModel.UnPaidLeave
        };

        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.OnLeave))
        {
            Directory.CreateDirectory(AppSettings.OnLeave);
        }

        var getDataById = await _unitOfWork.OnLeave.GetDataById(onLeaveModel.Id ?? Guid.Empty);

        if (getDataById.Attachments is not null)
        {
            var splitFileName = getDataById?.Attachments?.Split('.');
            if (splitFileName is not null)
            {
                var lastIndexOfSplit = splitFileName.Length;
                var idFile = getDataById?.Id + "." + splitFileName[lastIndexOfSplit - 1];

                var filename = Path.Combine(AppSettings.OnLeave, Path.GetFileName(idFile));

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
                var filename = Path.Combine(AppSettings.OnLeave,
                    Path.GetFileName($"{onLeaveDto.Id}.{file.FileName.Split('.')[1]}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                onLeaveDto.Attachments = file.FileName;
            }
        }

        var result = await _unitOfWork.OnLeave.Update(onLeaveDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/OnLeave/deleteOnLeave
    [HttpDelete("deleteOnLeave")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteOnLeave(List<Guid> idOnLeave)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.OnLeave.RemoveByList(idOnLeave, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}