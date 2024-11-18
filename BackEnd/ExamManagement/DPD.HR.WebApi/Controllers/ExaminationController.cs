using CT.EXAMM.Infrastructure.Validation.Models.Examination;
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
    [Route("api/v1/examination")]
    [ApiController]
    public class ExaminationController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExaminationController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public ExaminationController(IUnitOfWork unitOfWork, ILogger<ExaminationController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ ExaminationController ]==============================================
        [HttpGet("getListExamination")]
        public async Task<IActionResult> GetListExamination(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.Examination.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getExaminationById")]
        public async Task<IActionResult> GetExaminationById(Guid idExamination)
        {
            var templateApi = await _unitOfWork.Examination.GetById(idExamination);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertExamination")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertExamination(ExaminationModel ExaminationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExaminationDto = ExaminationModel.Adapt<ExaminationDto>();

            ExaminationDto.Id = Guid.NewGuid();
            ExaminationDto.CreatedDate = DateTime.Now;
          //  ExaminationDto.Status = 0;

            var result = await _unitOfWork.Examination.Insert(ExaminationDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateExamination")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateExamination(ExaminationModel ExaminationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExaminationDto = ExaminationModel.Adapt<ExaminationDto>();

            var result = await _unitOfWork.Examination.Update(ExaminationDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteExamination")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteExamination(List<Guid> idExamination)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.Examination.RemoveByList(idExamination, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
