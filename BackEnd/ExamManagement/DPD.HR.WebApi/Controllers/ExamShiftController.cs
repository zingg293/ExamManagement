using CT.EXAMM.Infrastructure.Validation.Models.ExamShift;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CT.EXAMM.Infrastructure.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamShiftController : Controller
    {
        #region ===[ Private Members ]=============================================================

        private readonly IUnitOfWork _unitOfWork;
        //private readonly ILogger<ExamShiftController> _logger;

        #endregion

        #region ===[ Constructor ]=================================================================

        public ExamShiftController(IUnitOfWork unitOfWork
        //ILogger<ExamShiftController> logger
        )
        {
            _unitOfWork = unitOfWork;
            //_logger = logger;
        }

        #endregion

        #region ===[ ExamShiftController ]=================================================================

        // GET: api/ExamShift/getListExamShift
        [HttpGet("getListExamShift")]
        public async Task<IActionResult> GetListExamShift(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.ExamShift.GetAllAsync(pageNumber, pageSize);
            //_logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        // GET: api/ExamShift/getExamShiftById
        [HttpGet("getExamShiftById")]
        public async Task<IActionResult> GetExamShiftById(Guid idExamShift)
        {
            var templateApi = await _unitOfWork.ExamShift.GetById(idExamShift);
            //_logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        // HttpPost: /api/ExamShift/insertExamShift
        [HttpPost("insertExamShift")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> InsertExamShift(ExamShiftModel ExamShiftModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExamShiftDto = ExamShiftModel.Adapt<ExamShiftDto>();

            ExamShiftDto.Id = Guid.NewGuid();
            //ExamShiftDto.CreatedDate = DateTime.Now;
            //ExamShiftDto.Status = 0;

            var result = await _unitOfWork.ExamShift.Insert(ExamShiftDto, idUserCurrent, nameUserCurrent);

            //_logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        // HttpPut: api/ExamShift/updateExamShift
        [HttpPut("updateExamShift")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateExamShift(ExamShiftModel ExamShiftModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExamShiftDto = ExamShiftModel.Adapt<ExamShiftDto>();

            var result = await _unitOfWork.ExamShift.Update(ExamShiftDto, idUserCurrent, nameUserCurrent);
            //_logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        // HttpDelete: /api/ExamShift/deleteExamShift
        [HttpDelete("deleteExamShift")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteExamShift(List<Guid> idExamShift)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.ExamShift.RemoveByList(idExamShift, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        #endregion
    }
}
