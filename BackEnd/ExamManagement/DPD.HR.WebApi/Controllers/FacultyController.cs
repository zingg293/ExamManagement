using CT.EXAMM.Infrastructure.Validation.Models.Faculty;
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
    [Route("api/v1/faculty")]
    [ApiController]
    public class FacultyController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FacultyController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public FacultyController(IUnitOfWork unitOfWork, ILogger<FacultyController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ FacultyController ]==============================================
        [HttpGet("getListFaculty")]
        public async Task<IActionResult> GetListFaculty(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.Faculty.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getFacultyById")]
        public async Task<IActionResult> GetFacultyById(Guid idFaculty)
        {
            var templateApi = await _unitOfWork.Faculty.GetById(idFaculty);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertFaculty")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertFaculty(FacultyModel FacultyModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var FacultyDto = FacultyModel.Adapt<FacultyDto>();

            FacultyDto.Id = Guid.NewGuid();
           // FacultyDto.CreatedDate = DateTime.Now;
            FacultyDto.Status = 0;

            var result = await _unitOfWork.Faculty.Insert(FacultyDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateFaculty")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateFaculty(FacultyModel FacultyModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var FacultyDto = FacultyModel.Adapt<FacultyDto>();

            var result = await _unitOfWork.Faculty.Update(FacultyDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteFaculty")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteFaculty(List<Guid> idFaculty)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.Faculty.RemoveByList(idFaculty, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
