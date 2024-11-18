using DPD.HR.Infrastructure.WebApi.Controllers;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Dapper;
using CT.EXAMM.Infrastructure.Validation.Models.Lecturers;

namespace CT.EXAMM.Infrastructure.WebApi.Controllers
{
    [Route("api/v1/lecturers")]
    [ApiController]
    public class LecturersController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LecturersController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public LecturersController(IUnitOfWork unitOfWork, ILogger<LecturersController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ LecturersController ]==============================================
        [HttpGet("getListLecturers")]
        public async Task<IActionResult> GetListLecturers(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.Lecturers.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getLecturersById")]
        public async Task<IActionResult> GetLecturersById(Guid idLecturers)
        {
            var templateApi = await _unitOfWork.Lecturers.GetById(idLecturers);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertLecturers")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertLecturers(LecturersModel LecturersModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;
            var instructorCode = (string)Request.HttpContext.Items["InstructorCode"]!;
            var birthday = (DateTime)Request.HttpContext.Items["Birthday"]!;
            var LecturersDto = LecturersModel.Adapt<LecturersDto>();

            LecturersDto.ID = Guid.NewGuid();
            LecturersDto.CreatedDate = DateTime.Now;
            LecturersDto.Status = 0;
          //  LecturersDto.Birthday = DateTime.Now;

            var result = await _unitOfWork.Lecturers.Insert(LecturersDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateLecturers")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateLecturers(LecturersModel LecturersModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;
            var instructorCode = (string)Request.HttpContext.Items["InstructorCode"]!;
            var birthday = (DateTime)Request.HttpContext.Items["Birthday"]!;
            var LecturersDto = LecturersModel.Adapt<LecturersDto>();

            var result = await _unitOfWork.Lecturers.Update(LecturersDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteLecturers")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteLecturers(List<Guid> idLecturers)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.Lecturers.RemoveByList(idLecturers, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
