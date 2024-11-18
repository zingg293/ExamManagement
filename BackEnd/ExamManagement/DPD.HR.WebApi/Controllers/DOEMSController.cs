using CT.EXAMM.Infrastructure.Validation.Models.DOEMS;
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
    [Route("api/v1/doems")]
    [ApiController]
    public class DOEMSController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DOEMSController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public DOEMSController(IUnitOfWork unitOfWork, ILogger<DOEMSController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ DOEMSController ]==============================================
        [HttpGet("getListDOEMS")]
        public async Task<IActionResult> GetListDOEMS(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.DOEMS.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getDOEMSById")]
        public async Task<IActionResult> GetDOEMSById(Guid idDOEMS)
        {
            var templateApi = await _unitOfWork.DOEMS.GetById(idDOEMS);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertDOEMS")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertDOEMS(DOEMSModel DOEMSModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var DOEMSDto = DOEMSModel.Adapt<DOEMSDto>();

            DOEMSDto.Id = Guid.NewGuid();
            DOEMSDto.CreatedDate = DateTime.Now;
           // DOEMSDto.Status = 0;

            var result = await _unitOfWork.DOEMS.Insert(DOEMSDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateDOEMS")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateDOEMS(DOEMSModel DOEMSModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var DOEMSDto = DOEMSModel.Adapt<DOEMSDto>();

            var result = await _unitOfWork.DOEMS.Update(DOEMSDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteDOEMS")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteDOEMS(List<Guid> idDOEMS)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.DOEMS.RemoveByList(idDOEMS, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
