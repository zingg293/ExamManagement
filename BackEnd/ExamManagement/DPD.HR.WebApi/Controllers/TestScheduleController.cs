using CT.EXAMM.Infrastructure.Validation.Models.TestSchedule;
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
    [Route("api/v1/testSchedule")]
    [ApiController]
    public class TestScheduleController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TestScheduleController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public TestScheduleController(IUnitOfWork unitOfWork, ILogger<TestScheduleController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ TestScheduleController ]==============================================
        [HttpGet("getListTestSchedule")]
        public async Task<IActionResult> GetListTestSchedule(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.TestSchedule.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getTestScheduleById")]
        public async Task<IActionResult> GetTestScheduleById(Guid idTestSchedule)
        {
            var templateApi = await _unitOfWork.TestSchedule.GetById(idTestSchedule);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertTestSchedule")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertTestSchedule(TestScheduleModel TestScheduleModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var TestScheduleDto = TestScheduleModel.Adapt<TestScheduleDto>();

            TestScheduleDto.Id = Guid.NewGuid();
            TestScheduleDto.CreatedDate = DateTime.Now;
           // TestScheduleDto.Status = 0;

            var result = await _unitOfWork.TestSchedule.Insert(TestScheduleDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateTestSchedule")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateTestSchedule(TestScheduleModel TestScheduleModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var TestScheduleDto = TestScheduleModel.Adapt<TestScheduleDto>();

            var result = await _unitOfWork.TestSchedule.Update(TestScheduleDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteTestSchedule")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteTestSchedule(List<Guid> idTestSchedule)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.TestSchedule.RemoveByList(idTestSchedule, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
