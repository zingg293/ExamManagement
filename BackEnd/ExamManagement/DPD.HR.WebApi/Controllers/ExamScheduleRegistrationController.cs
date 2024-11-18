using CT.EXAMM.Infrastructure.Validation.Models.ExamScheduleRegistration;
using DPD.HR.Infrastructure.Validation.Models.PolicticialInformationEmployee;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace CT.EXAMM.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamScheduleRegistrationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExamScheduleRegistrationController> _logger;

        public ExamScheduleRegistrationController(IUnitOfWork unitOfWork, ILogger<ExamScheduleRegistrationController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("getListExamScheduleRegistration")]
        public async Task<IActionResult> GetListExamScheduleRegistration(int pageNumber, int pageSize)
        {
            var result = await _unitOfWork.ExamScheduleRegistration.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpGet("getExamScheduleRegistrationById")]
        public async Task<IActionResult> GetExamScheduleRegistrationById(Guid id)
        {
            var result = await _unitOfWork.ExamScheduleRegistration.GetById(id);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPost("insertExamScheduleRegistration")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> InsertExamScheduleRegistration(ExamScheduleRegistrationModel model)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var dto = model.Adapt<ExamScheduleRegistrationDto>();
            dto.Id = Guid.NewGuid();
            dto.CreatedDate = DateTime.Now;
            dto.IsActive = true;

            var result = await _unitOfWork.ExamScheduleRegistration.Insert(dto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateExamScheduleRegistration")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateExamScheduleRegistration(ExamScheduleRegistrationModel model)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var dto = model.Adapt<ExamScheduleRegistrationDto>();

            var result = await _unitOfWork.ExamScheduleRegistration.Update(dto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpDelete("deleteExamScheduleRegistration")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteExamScheduleRegistration(List<Guid> ids)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.ExamScheduleRegistration.RemoveByList(ids, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
    }

}
