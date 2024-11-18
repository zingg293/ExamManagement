using CT.EXAMM.Infrastructure.Validation.Models.EducationProgram;
using DPD.HR.Infrastructure.WebApi.Controllers;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.EXAMM.Infrastructure.WebApi.Controllers
{
    [Route("api/v1/educationProgram")]
    [ApiController]
    public class EducationProgramController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EducationProgramController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public EducationProgramController(IUnitOfWork unitOfWork, ILogger<EducationProgramController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ EducationProgramController ]==============================================
        [HttpGet("getListEducationProgram")]
        public async Task<IActionResult> GetListEducationProgram(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.EducationProgram.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getEducationProgramById")]
        public async Task<IActionResult> GetEducationProgramById(Guid idEducationProgram)
        {
            var templateApi = await _unitOfWork.EducationProgram.GetById(idEducationProgram);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertEducationProgram")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertEducationProgram(EducationProgramModel EducationProgramModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;
            var educationProgramName = (string)Request.HttpContext.Items["EducationProgramName"]!;

            var EducationProgramDto = EducationProgramModel.Adapt<EducationProgramDto>();

            EducationProgramDto.Id = Guid.NewGuid();
        //    EducationProgramDto.CreatedDate = DateTime.Now;
            EducationProgramDto.Status = 0;

            var result = await _unitOfWork.EducationProgram.Insert(EducationProgramDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateEducationProgram")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateEducationProgram(EducationProgramModel EducationProgramModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var EducationProgramDto = EducationProgramModel.Adapt<EducationProgramDto>();

            var result = await _unitOfWork.EducationProgram.Update(EducationProgramDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteEducationProgram")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteEducationProgram(List<Guid> idEducationProgram)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.EducationProgram.RemoveByList(idEducationProgram, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
