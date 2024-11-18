using CT.EXAMM.Infrastructure.Validation.Models.ExamForm;
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
    [Route("api/v1/examForm")]
    [ApiController]
    public class ExamFormController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExamFormController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public ExamFormController(IUnitOfWork unitOfWork, ILogger<ExamFormController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ ExamFormController ]==============================================
        [HttpGet("getListExamForm")]
        public async Task<IActionResult> GetListExamForm(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.ExamForm.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getExamFormById")]
        public async Task<IActionResult> GetExamFormById(Guid idExamForm)
        {
            var templateApi = await _unitOfWork.ExamForm.GetById(idExamForm);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertExamForm")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertExamForm(ExamFormModel ExamFormModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExamFormDto = ExamFormModel.Adapt<ExamFormDto>();

            ExamFormDto.Id = Guid.NewGuid();
           // ExamFormDto.CreatedDate = DateTime.Now;
           // ExamFormDto.Status = 0;

            var result = await _unitOfWork.ExamForm.Insert(ExamFormDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateExamForm")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateExamForm(ExamFormModel ExamFormModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExamFormDto = ExamFormModel.Adapt<ExamFormDto>();

            var result = await _unitOfWork.ExamForm.Update(ExamFormDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteExamForm")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteExamForm(List<Guid> idExamForm)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.ExamForm.RemoveByList(idExamForm, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
