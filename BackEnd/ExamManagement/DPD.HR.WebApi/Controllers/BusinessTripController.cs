using System.Globalization;
using DPD.HR.Infrastructure.Validation.Models.BusinessTrip;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/businessTrip")]
public class BusinessTripController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BusinessTripController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public BusinessTripController(IUnitOfWork unitOfWork, ILogger<BusinessTripController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ BusinessTripController ]==============================================

    // GET: api/BusinessTrip/filterListBusinessTrip
    [HttpGet("filterListBusinessTrip")]
    public async Task<IActionResult> FilterListBusinessTrip(int pageNumber, int pageSize, Guid? idUnit,
        string? startDate, string? endDate)
    {
        var templateApi =
            await _unitOfWork.BusinessTrip.GetListBusinessTrip(pageNumber,
                pageSize, idUnit, startDate, endDate);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/BusinessTrip/getListBusinessTripByHistory
    [HttpGet("getListBusinessTripByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListBusinessTripByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.BusinessTrip.GetBusinessTripAndWorkFlowByIdUserApproved(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/BusinessTrip/getListBusinessTripByRole
    [HttpGet("getListBusinessTripByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListBusinessTripByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.BusinessTrip.GetBusinessTripAndWorkFlow(idUserCurrent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/BusinessTripModel/getListBusinessTrip
    [HttpGet("getListBusinessTrip")]
    public async Task<IActionResult> GetListBusinessTrip(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.BusinessTrip.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/BusinessTripModel/getBusinessTripById
    [HttpGet("getBusinessTripById")]
    public async Task<IActionResult> GetBusinessTripById(Guid idBusinessTrip)
    {
        var templateApi = await _unitOfWork.BusinessTrip.GetById(idBusinessTrip);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/BusinessTripModel/insertBusinessTrip
    [HttpPost("insertBusinessTrip")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> InsertBusinessTrip([FromForm] BusinessTripModel businessTripModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var startDay =
            DateTime.ParseExact(businessTripModel.StartDate, "dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("vi-VN"));
        var endDay =
            DateTime.ParseExact(businessTripModel.EndDate, "dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("vi-VN"));

        var businessTripDto = new BusinessTripDto
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            Status = 0,
            IdUserRequest = idUserCurrent,
            StartDate = startDay,
            EndDate = endDay,
            Description = businessTripModel.Description,
            IdUnit = businessTripModel.IdUnit,
            UnitName = businessTripModel.UnitName,
            Client = businessTripModel.Client,
            BusinessTripLocation = businessTripModel.BusinessTripLocation,
            Vehicle = businessTripModel.Vehicle,
            Expense = businessTripModel.Expense
        };
        
        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileBusinessTrip))
        {
            Directory.CreateDirectory(AppSettings.ServerFileBusinessTrip);
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                // Use Path.GetExtension to get the file extension
                var fileExtension = Path.GetExtension(file.FileName);
                
                var filename = Path.Combine(AppSettings.ServerFileBusinessTrip,
                    Path.GetFileName($"{businessTripDto.Id}.{fileExtension}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                businessTripDto.Attachments = file.FileName;
            }
        }

        var codeWorkFlow = AppSettings.BusinessTrip;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, businessTripDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.BusinessTrip.Insert(businessTripDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/BusinessTripModel/updateBusinessTrip
    [HttpPut("updateBusinessTrip")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateBusinessTrip([FromForm] BusinessTripModel businessTripModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var startDay =
            DateTime.ParseExact(businessTripModel.StartDate, "dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("vi-VN"));
        var endDay =
            DateTime.ParseExact(businessTripModel.EndDate, "dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("vi-VN"));

        var businessTripDto = new BusinessTripDto
        {
            Id = businessTripModel.Id ?? Guid.Empty,
            StartDate = startDay,
            EndDate = endDay,
            Description = businessTripModel.Description,
            IdUnit = businessTripModel.IdUnit,
            UnitName = businessTripModel.UnitName,
            Client = businessTripModel.Client,
            BusinessTripLocation = businessTripModel.BusinessTripLocation,
            Vehicle = businessTripModel.Vehicle,
            Expense = businessTripModel.Expense
        };
        
        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileBusinessTrip))
        {
            Directory.CreateDirectory(AppSettings.ServerFileBusinessTrip);
        }

        var getDataById = await _unitOfWork.BusinessTrip.GetDataById(businessTripDto.Id);

        if (getDataById.Attachments is not null)
        {
            // Use Path.GetExtension to get the file extension
            var fileExtension = Path.GetExtension(getDataById?.Attachments);
            
            if (fileExtension is not null)
            {
                var idFile = getDataById?.Id + "." + fileExtension;

                var filename = Path.Combine(AppSettings.ServerFileBusinessTrip, Path.GetFileName(idFile));

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
                
                var filename = Path.Combine(AppSettings.ServerFileBusinessTrip,
                    Path.GetFileName($"{businessTripDto.Id}.{fileExtension}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                businessTripDto.Attachments = file.FileName;
            }
        }

        var result = await _unitOfWork.BusinessTrip.Update(businessTripDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/BusinessTripModel/deleteBusinessTrip
    [HttpDelete("deleteBusinessTrip")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> DeleteBusinessTrip(List<Guid> idBusinessTrip)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.BusinessTrip.RemoveByList(idBusinessTrip, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }
    
    // HttpGet: api/BusinessTrip/getFileBusinessTrip
    [HttpGet]
    [Route("getFileBusinessTrip")]
    public IActionResult GetFileBusinessTrip(string fileNameId)
    {
        var temp = fileNameId.Split('.');
        var fileBytes = System.IO.File.ReadAllBytes(string.Concat(AppSettings.ServerFileBusinessTrip, "\\",
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