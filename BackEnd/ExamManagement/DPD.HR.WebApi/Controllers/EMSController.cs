using CT.EXAMM.Infrastructure.Validation.Models.EMS;
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
    [Route("api/v1/ems")]
    [ApiController]
    public class EMSController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EMSController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public EMSController(IUnitOfWork unitOfWork, ILogger<EMSController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ EMSController ]==============================================
        [HttpGet("getListEMS")]
        public async Task<IActionResult> GetListEMS(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.EMS.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getEMSById")]
        public async Task<IActionResult> GetEMSById(Guid idEMS)
        {
            var templateApi = await _unitOfWork.EMS.GetById(idEMS);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertEMS")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertEMS(EMSModel EMSModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var EMSDto = EMSModel.Adapt<EMSDto>();

            EMSDto.Id = Guid.NewGuid();
            EMSDto.CreatedDate = DateTime.Now;
          //  EMSDto.Status = 0;

            var result = await _unitOfWork.EMS.Insert(EMSDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateEMS")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateEMS(EMSModel EMSModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var EMSDto = EMSModel.Adapt<EMSDto>();

            var result = await _unitOfWork.EMS.Update(EMSDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteEMS")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteEMS(List<Guid> idEMS)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.EMS.RemoveByList(idEMS, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
