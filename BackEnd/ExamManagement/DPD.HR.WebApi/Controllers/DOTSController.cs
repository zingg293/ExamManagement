using CT.EXAMM.Infrastructure.Validation.Models.DOTS;
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
    [Route("api/v1/dots")]
    [ApiController]
    public class DOTSController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DOTSController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public DOTSController(IUnitOfWork unitOfWork, ILogger<DOTSController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ DOTSController ]==============================================
        [HttpGet("getListDOTS")]
        public async Task<IActionResult> GetListDOTS(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.DOTS.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getDOTSById")]
        public async Task<IActionResult> GetDOTSById(Guid idDOTS)
        {
            var templateApi = await _unitOfWork.DOTS.GetById(idDOTS);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertDOTS")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertDOTS(DOTSModel DOTSModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var DOTSDto = DOTSModel.Adapt<DOTSDto>();

            DOTSDto.Id = Guid.NewGuid();
          //  DOTSDto.CreatedDate = DateTime.Now;
          //  DOTSDto.Status = 0;

            var result = await _unitOfWork.DOTS.Insert(DOTSDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateDOTS")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateDOTS(DOTSModel DOTSModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var DOTSDto = DOTSModel.Adapt<DOTSDto>();

            var result = await _unitOfWork.DOTS.Update(DOTSDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteDOTS")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteDOTS(List<Guid> idDOTS)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.DOTS.RemoveByList(idDOTS, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
