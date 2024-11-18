using CT.EXAMM.Infrastructure.Validation.Models.StudyGroup;
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
    [Route("api/v1/StudyGroup")]
    [ApiController]
    public class StudyGroupController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudyGroupController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public StudyGroupController(IUnitOfWork unitOfWork, ILogger<StudyGroupController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ StudyGroupController ]==============================================
        [HttpGet("getListStudyGroup")]
        public async Task<IActionResult> GetListStudyGroup(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.StudyGroup.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getStudyGroupById")]
        public async Task<IActionResult> GetStudyGroupById(Guid idStudyGroup)
        {
            var templateApi = await _unitOfWork.StudyGroup.GetById(idStudyGroup);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertStudyGroup")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertStudyGroup(StudyGroupModel StudyGroupModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var StudyGroupDto = StudyGroupModel.Adapt<StudyGroupDto>();

            StudyGroupDto.Id = Guid.NewGuid();
            //StudyGroupDto.CreateDate = DateTime.Now;
            //   StudyGroupDto.Status = 0;

            var result = await _unitOfWork.StudyGroup.Insert(StudyGroupDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateStudyGroup")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateStudyGroup(StudyGroupModel StudyGroupModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var StudyGroupDto = StudyGroupModel.Adapt<StudyGroupDto>();

            var result = await _unitOfWork.StudyGroup.Update(StudyGroupDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteStudyGroup")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteStudyGroup(List<Guid> idStudyGroup)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.StudyGroup.RemoveByList(idStudyGroup, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
