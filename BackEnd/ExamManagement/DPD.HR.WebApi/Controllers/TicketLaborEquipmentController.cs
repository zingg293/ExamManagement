using DPD.HR.Infrastructure.Validation.Models.TicketLaborEquipment;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/ticketLaborEquipment")]
public class TicketLaborEquipmentController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TicketLaborEquipmentController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public TicketLaborEquipmentController(IUnitOfWork unitOfWork, ILogger<TicketLaborEquipmentController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ TicketLaborEquipmentController ]==============================================

    // GET: api/TicketLaborEquipment/getListTicketLaborEquipmentByHistory
    [HttpGet("getListTicketLaborEquipmentByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListTicketLaborEquipmentByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.TicketLaborEquipment.GetTicketLaborEquipmentAndWorkFlowByIdUserApproved(idUserCurrent,
                pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/TicketLaborEquipment/getListTicketLaborEquipmentByRole
    [HttpGet("getListTicketLaborEquipmentByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListTicketLaborEquipmentByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.TicketLaborEquipment.GetTicketLaborEquipmentAndWorkFlow(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpGet: api/TicketLaborEquipment/getFileTicketLaborEquipment
    [HttpGet]
    [Route("getFileTicketLaborEquipment")]
    public IActionResult GetFileTicketLaborEquipment(string fileNameId)
    {
        var temp = fileNameId.Split('.');
        var fileBytes = System.IO.File.ReadAllBytes(string.Concat(AppSettings.ServerFileTicketLaborEquipment, "\\",
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

    // GET: api/TicketLaborEquipment/getTicketLaborEquipmentById
    [HttpGet("getTicketLaborEquipmentById")]
    public async Task<IActionResult> GetTicketLaborEquipmentById(Guid idTicketLaborEquipment)
    {
        var templateApi = await _unitOfWork.TicketLaborEquipment.GetById(idTicketLaborEquipment);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/TicketLaborEquipment/insertTicketLaborEquipment
    [HttpPost("insertTicketLaborEquipment")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> InsertTicketLaborEquipment(
        [FromForm] TicketLaborEquipmentModel ticketLaborEquipmentModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var ticketLaborEquipmentDto = ticketLaborEquipmentModel.Adapt<TicketLaborEquipmentDto>();

        ticketLaborEquipmentDto.Id = Guid.NewGuid();
        ticketLaborEquipmentDto.CreatedDate = DateTime.Now;
        ticketLaborEquipmentDto.Status = 0;

        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileTicketLaborEquipment))
        {
            Directory.CreateDirectory(AppSettings.ServerFileTicketLaborEquipment);
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                var filename = Path.Combine(AppSettings.ServerFileTicketLaborEquipment,
                    Path.GetFileName($"{ticketLaborEquipmentDto.Id}.{file.FileName.Split('.')[1]}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                ticketLaborEquipmentDto.FileAttachment = file.FileName;
            }
        }

        var codeWorkFlow = AppSettings.TicketLaborEquipment;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, ticketLaborEquipmentDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.TicketLaborEquipment.Insert(ticketLaborEquipmentDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/TicketLaborEquipment/updateTicketLaborEquipment
    [HttpPut("updateTicketLaborEquipment")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateTicketLaborEquipment(
        [FromForm] TicketLaborEquipmentModel ticketLaborEquipmentModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var ticketLaborEquipmentDto = ticketLaborEquipmentModel.Adapt<TicketLaborEquipmentDto>();

        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileTicketLaborEquipment))
        {
            Directory.CreateDirectory(AppSettings.ServerFileTicketLaborEquipment);
        }

        var getDataById = await _unitOfWork.TicketLaborEquipment.GetDataById(ticketLaborEquipmentDto.Id);

        if (getDataById.FileAttachment is not null)
        {
            var splitFileName = getDataById?.FileAttachment?.Split('.');
            if (splitFileName is not null)
            {
                var lastIndexOfSplit = splitFileName.Length;
                var idFile = getDataById?.Id + "." + splitFileName[lastIndexOfSplit - 1];

                var filename = Path.Combine(AppSettings.ServerFileTicketLaborEquipment, Path.GetFileName(idFile));

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
                var filename = Path.Combine(AppSettings.ServerFileTicketLaborEquipment,
                    Path.GetFileName($"{ticketLaborEquipmentDto.Id}.{file.FileName.Split('.')[1]}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                ticketLaborEquipmentDto.FileAttachment = file.FileName;
            }
        }

        var result =
            await _unitOfWork.TicketLaborEquipment.Update(ticketLaborEquipmentDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/TicketLaborEquipment/deleteTicketLaborEquipment
    [HttpDelete("deleteTicketLaborEquipment")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> DeleteTicketLaborEquipment(List<Guid> idTicketLaborEquipment)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.TicketLaborEquipment.RemoveByList(idTicketLaborEquipment, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}