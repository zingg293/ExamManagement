using DPD.HR.Infrastructure.Validation.Models.Candidate;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/candidate")]
public class CandidateController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CandidateController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CandidateController(IUnitOfWork unitOfWork, ILogger<CandidateController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ CandidateController ]==============================================

    // GET: api/Candidate/getListCandidate
    [HttpGet("getListCandidate")]
    public async Task<IActionResult> GetListCandidate(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.Candidate.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Candidate/getCandidateById
    [HttpGet("getCandidateById")]
    public async Task<IActionResult> GetCandidateById(Guid idCandidate)
    {
        var templateApi = await _unitOfWork.Candidate.GetById(idCandidate);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/Candidate/insertCandidate
    [HttpPost("insertCandidate")]
    public async Task<IActionResult> InsertCandidate([FromForm] CandidateModel candidateModel)
    {
        var idUserCurrent = Guid.Empty;
        var nameUserCurrent = "";

        var candidateDto = candidateModel.Adapt<CandidateDto>();

        candidateDto.Id = Guid.NewGuid();
        candidateDto.CreatedDate = DateTime.Now;
        candidateDto.Status = 0;
        
        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileCandidate))
        {
            Directory.CreateDirectory(AppSettings.ServerFileCandidate);
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                var filename = Path.Combine(AppSettings.ServerFileCandidate,
                    Path.GetFileName($"{candidateDto.Id}.{file.FileName.Split('.')[1]}"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                candidateDto.File = file.FileName;
            }
        }

        var result = await _unitOfWork.Candidate.Insert(candidateDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/Candidate/updateCandidate
    [HttpPut("updateCandidate")]
    public async Task<IActionResult> UpdateCandidate([FromForm] CandidateModel candidateModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var candidateDto = candidateModel.Adapt<CandidateDto>();

        var result = await _unitOfWork.Candidate.Update(candidateDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/Candidate/deleteCandidate
    [HttpDelete("deleteCandidate")]
    public async Task<IActionResult> DeleteCandidate(List<Guid> idCandidate)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.Candidate.RemoveByList(idCandidate, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpGet: api/Candidate/getFileCandidate
    [HttpGet]
    [Route("getFileCandidate")]
    public IActionResult GetFileCandidate(string fileNameId)
    {
        var temp = fileNameId.Split('.');
        var fileBytes = System.IO.File.ReadAllBytes(string.Concat(AppSettings.ServerFileCandidate, "\\",
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