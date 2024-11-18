using CT.EXAMM.Infrastructure.Validation.Models.ExamSubject;
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
    [Route("api/v1/examSubject")]
    [ApiController]
    public class ExamSubjectController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExamSubjectController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public ExamSubjectController(IUnitOfWork unitOfWork, ILogger<ExamSubjectController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ ExamSubjectController ]==============================================
        [HttpGet("getListExamSubject")]
        public async Task<IActionResult> GetListExamSubject(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.ExamSubjec.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getExamSubjectById")]
        public async Task<IActionResult> GetExamSubjectById(Guid idExamSubject)
        {
            var templateApi = await _unitOfWork.ExamSubjec.GetById(idExamSubject);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertExamSubject")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertExamSubject(ExamSubjectModel ExamSubjectModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExamSubjectDto = ExamSubjectModel.Adapt<ExamSubjectDto>();

            ExamSubjectDto.Id = Guid.NewGuid();
            ExamSubjectDto.CreatedDate = DateTime.Now;
         //   ExamSubjectDto.Status = 0;

            var result = await _unitOfWork.ExamSubjec.Insert(ExamSubjectDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateExamSubject")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateExamSubject(ExamSubjectModel ExamSubjectModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExamSubjectDto = ExamSubjectModel.Adapt<ExamSubjectDto>();

            var result = await _unitOfWork.ExamSubjec.Update(ExamSubjectDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteExamSubject")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteExamSubject(List<Guid> idExamSubject)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.ExamSubjec.RemoveByList(idExamSubject, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
