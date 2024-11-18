using DPD.HR.Infrastructure.Validation.Models.WorkExperience;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers
{
    [Route("api/v1/WorkExperience")]
    [ApiController]
    public class WorkExperienceController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WorkExperienceController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public WorkExperienceController(IUnitOfWork unitOfWork, ILogger<WorkExperienceController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getWorkExperienceById")]
        public async Task<IActionResult> GetWorkExperienceById(Guid idWorkExperience)
        {
            var templateApi = await _unitOfWork.WorkExperience.GetById(idWorkExperience);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertWorkExperience")]
        public async Task<IActionResult> InsertWorkExperienceEmployee(WorkExperience WorkExperienceModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var WorkExperienceDto = WorkExperienceModel.Adapt<WorkExperienceDto>();

            WorkExperienceDto.Id = Guid.NewGuid();
            WorkExperienceDto.CreatedDate = DateTime.Now;
            WorkExperienceDto.Status = 0;

            var result = await _unitOfWork.WorkExperience.Insert(WorkExperienceDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateWorkExperience")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateWorkExperience(WorkExperienceModel WorkExperienceModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var WorkExperienceDto = WorkExperienceModel.Adapt<WorkExperienceDto>();

            var result = await _unitOfWork.WorkExperience.Update(WorkExperienceDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getWorkExperienceById")]
        //public async Task<IActionResult> GetWorkExperienceById(Guid idWorkExperience)
        //{
        //    var templateApi = await _unitOfWork.WorkExperience.GetById(idWorkExperience);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteWorkExperience")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteWorkExperience(List<Guid> idWorkExperience)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.WorkExperience.RemoveByList(idWorkExperience, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListWorkExperience")]
        public async Task<IActionResult> GetListWorkExperience(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.WorkExperience.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
