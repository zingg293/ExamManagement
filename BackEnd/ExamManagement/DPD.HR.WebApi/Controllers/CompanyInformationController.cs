using DPD.HR.Infrastructure.Validation.Models.CompanyInformation;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/companyInformation")]
public class CompanyInformationController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompanyInformationController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CompanyInformationController(IUnitOfWork unitOfWork, ILogger<CompanyInformationController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ CompanyInformationController ]==============================================

    // GET: api/CompanyInformation/getListCompanyInformation
    [HttpGet("getListCompanyInformation")]
    public async Task<IActionResult> GetListCompanyInformation(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CompanyInformation.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CompanyInformation/getCompanyInformationById
    [HttpGet("getCompanyInformationById")]
    public async Task<IActionResult> GetCompanyInformationById(Guid idCompanyInformation)
    {
        var templateApi = await _unitOfWork.CompanyInformation.GetById(idCompanyInformation);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/CompanyInformation/insertCompanyInformation
    [HttpPost("insertCompanyInformation")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertCompanyInformation(
        [FromForm] CompanyInformationModel companyInformationModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var companyInformationDto = companyInformationModel.Adapt<CompanyInformationDto>();

        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileCompanyInformation))
        {
            Directory.CreateDirectory(AppSettings.ServerFileCompanyInformation);
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                var fileContentType = file.ContentType;

                switch (fileContentType)
                {
                    case "image/jpeg":
                    case "image/png":
                    case "image/jpg":
                    {
                        var filename = Path.Combine(AppSettings.ServerFileCompanyInformation,
                            Path.GetFileName($"{companyInformationDto.Id}.jpg"));

                        await using (var stream = System.IO.File.Create(filename))
                        {
                            await file.CopyToAsync(stream);
                        }

                        companyInformationDto.Logo = file.FileName;
                        break;
                    }
                }
            }
        }

        companyInformationDto.Id = Guid.NewGuid();
        companyInformationDto.CreatedDate = DateTime.Now;
        companyInformationDto.Status = 0;

        var result = await _unitOfWork.CompanyInformation.Insert(companyInformationDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/CompanyInformation/updateCompanyInformation
    [HttpPut("updateCompanyInformation")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateCompanyInformation(
        [FromForm] CompanyInformationModel companyInformationModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var companyInformationDto = companyInformationModel.Adapt<CompanyInformationDto>();

        // If directory does not exist, create it. 
        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileCompanyInformation))
        {
            Directory.CreateDirectory(AppSettings.ServerFileCompanyInformation);
        }

        if (companyInformationModel.IdFile is null)
        {
            var idFile = companyInformationModel.Id + ".jpg";

            var filename = Path.Combine(AppSettings.ServerFileCompanyInformation, Path.GetFileName(idFile));

            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                var fileContentType = file.ContentType;

                switch (fileContentType)
                {
                    case "image/jpeg":
                    case "image/png":
                    case "image/jpg":
                    {
                        var filename = Path.Combine(AppSettings.ServerFileCompanyInformation,
                            Path.GetFileName($"{companyInformationModel.Id}.jpg"));

                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }

                        await using (var stream = System.IO.File.Create(filename))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // set data document avatar
                        companyInformationDto.Logo = file.FileName;
                        break;
                    }
                }
            }
        }

        var result = await _unitOfWork.CompanyInformation.Update(companyInformationDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/CompanyInformation/deleteCompanyInformation
    [HttpDelete("deleteCompanyInformation")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteCompanyInformation(List<Guid> idCompanyInformation)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.CompanyInformation.RemoveByList(idCompanyInformation, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }
    
    // HttpGet: api/CompanyInformation/GetFileImage
    [HttpGet]
    [Route("getFileImage")]
    public IActionResult GetFileImage(string fileNameId)
    {
        var temp = fileNameId.Split('.');
        var fileBytes = Array.Empty<byte>();
        if (temp[1] == "jpg" || temp[1] == "png")
        {
            fileBytes = System.IO.File.ReadAllBytes(string.Concat(AppSettings.ServerFileCompanyInformation, "\\",
                fileNameId));
        }

        return File(fileBytes, "image/jpeg");
    }

    #endregion
}