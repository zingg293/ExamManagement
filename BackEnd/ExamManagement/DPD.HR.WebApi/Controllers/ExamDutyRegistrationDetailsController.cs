using CT.EXAMM.Infrastructure.Validation.Models.ExamDutyRegistrationDetails;
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
    public class ExamDutyRegistrationDetailsController : Controller
    {
        #region ===[ Private Members ]=============================================================

        private readonly IUnitOfWork _unitOfWork;
        //private readonly ILogger<ExamDutyRegistrationDetailsController> _logger;

        #endregion

        #region ===[ Constructor ]=================================================================

        public ExamDutyRegistrationDetailsController(IUnitOfWork unitOfWork
        //ILogger<ExamDutyRegistrationDetailsController> logger
        )
        {
            _unitOfWork = unitOfWork;
            //_logger = logger;
        }

        #endregion

        #region ===[ ExamDutyRegistrationDetailsController ]=================================================================

        // GET: api/ExamDutyRegistrationDetails/getListExamDutyRegistrationDetails
        [HttpGet("getListExamDutyRegistrationDetails")]
        public async Task<IActionResult> GetListExamDutyRegistrationDetails(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.ExamDutyRegistrationDetails.GetAllAsync(pageNumber, pageSize);
            //_logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        // GET: api/ExamDutyRegistrationDetails/getExamDutyRegistrationDetailsById
        [HttpGet("getExamDutyRegistrationDetailsById")]
        public async Task<IActionResult> GetExamDutyRegistrationDetailsById(Guid idExamDutyRegistrationDetails)
        {
            var templateApi = await _unitOfWork.ExamDutyRegistrationDetails.GetById(idExamDutyRegistrationDetails);
            //_logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        // HttpPost: /api/ExamDutyRegistrationDetails/insertExamDutyRegistrationDetails
        [HttpPost("insertExamDutyRegistrationDetails")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> InsertExamDutyRegistrationDetails(ExamDutyRegistrationDetailsModel ExamDutyRegistrationDetailsModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExamDutyRegistrationDetailsDto = ExamDutyRegistrationDetailsModel.Adapt<ExamDutyRegistrationDetailsDto>();

            ExamDutyRegistrationDetailsDto.Id = Guid.NewGuid();
            //ExamDutyRegistrationDetailsDto.CreatedDate = DateTime.Now;
            //ExamDutyRegistrationDetailsDto.Status = 0;

            var result = await _unitOfWork.ExamDutyRegistrationDetails.Insert(ExamDutyRegistrationDetailsDto, idUserCurrent, nameUserCurrent);

            //_logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        // HttpPut: api/ExamDutyRegistrationDetails/updateExamDutyRegistrationDetails
        [HttpPut("updateExamDutyRegistrationDetails")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateExamDutyRegistrationDetails(ExamDutyRegistrationDetailsModel ExamDutyRegistrationDetailsModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExamDutyRegistrationDetailsDto = ExamDutyRegistrationDetailsModel.Adapt<ExamDutyRegistrationDetailsDto>();

            var result = await _unitOfWork.ExamDutyRegistrationDetails.Update(ExamDutyRegistrationDetailsDto, idUserCurrent, nameUserCurrent);
            //_logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        // HttpDelete: /api/ExamDutyRegistrationDetails/deleteExamDutyRegistrationDetails
        [HttpDelete("deleteExamDutyRegistrationDetails")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteExamDutyRegistrationDetails(List<Guid> idExamDutyRegistrationDetails)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.ExamDutyRegistrationDetails.RemoveByList(idExamDutyRegistrationDetails, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        #endregion
    }
}
