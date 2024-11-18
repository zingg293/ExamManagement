using CT.EXAMM.Infrastructure.Validation.Models.ExaminationType;
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
    [Route("api/v1/ExaminationType")]
    [ApiController]
    public class ExaminationTypeController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExaminationTypeController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public ExaminationTypeController(IUnitOfWork unitOfWork, ILogger<ExaminationTypeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ ExaminationTypeController ]==============================================
        [HttpGet("getListExaminationType")]
        public async Task<IActionResult> GetListExaminationType(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.ExaminationType.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getExaminationTypeById")]
        public async Task<IActionResult> GetExaminationTypeById(Guid idExaminationType)
        {
            var templateApi = await _unitOfWork.ExaminationType.GetById(idExaminationType);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertExaminationType")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertExaminationType(ExaminationTypeModel ExaminationTypeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExaminationTypeDto = ExaminationTypeModel.Adapt<ExaminationTypeDto>();

            ExaminationTypeDto.Id = Guid.NewGuid();
            ExaminationTypeDto.CreatedDate = DateTime.Now;
            //   ExaminationTypeDto.Status = 0;

            var result = await _unitOfWork.ExaminationType.Insert(ExaminationTypeDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateExaminationType")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateExaminationType(ExaminationTypeModel ExaminationTypeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var ExaminationTypeDto = ExaminationTypeModel.Adapt<ExaminationTypeDto>();

            var result = await _unitOfWork.ExaminationType.Update(ExaminationTypeDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteExaminationType")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteExaminationType(List<Guid> idExaminationType)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.ExaminationType.RemoveByList(idExaminationType, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
