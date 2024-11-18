using DPD.HR.Infrastructure.Validation.Models.PassportVisaWorkPermit;
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
    [Route("api/v1/PassportVisaWorkPermit")]
    [ApiController]
    public class PassportVisaWorkPermitController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PassportVisaWorkPermitController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public PassportVisaWorkPermitController(IUnitOfWork unitOfWork, ILogger<PassportVisaWorkPermitController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getPassportVisaWorkPermitById")]
        public async Task<IActionResult> GetPassportVisaWorkPermitById(Guid idPassportVisaWorkPermit)
        {
            var templateApi = await _unitOfWork.PassportVisaWorkPermit.GetById(idPassportVisaWorkPermit);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertPassportVisaWorkPermit")]
        public async Task<IActionResult> InsertPassportVisaWorkPermitEmployee(PassportVisaWorkPermit PassportVisaWorkPermitModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var PassportVisaWorkPermitDto = PassportVisaWorkPermitModel.Adapt<PassportVisaWorkPermitDto>();

            PassportVisaWorkPermitDto.Id = Guid.NewGuid();
            PassportVisaWorkPermitDto.CreatedDate = DateTime.Now;
            PassportVisaWorkPermitDto.Status = 0;

            var result = await _unitOfWork.PassportVisaWorkPermit.Insert(PassportVisaWorkPermitDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updatePassportVisaWorkPermit")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdatePassportVisaWorkPermit(PassportVisaWorkPermitModel PassportVisaWorkPermitModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var PassportVisaWorkPermitDto = PassportVisaWorkPermitModel.Adapt<PassportVisaWorkPermitDto>();

            var result = await _unitOfWork.PassportVisaWorkPermit.Update(PassportVisaWorkPermitDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getPassportVisaWorkPermitById")]
        //public async Task<IActionResult> GetPassportVisaWorkPermitById(Guid idPassportVisaWorkPermit)
        //{
        //    var templateApi = await _unitOfWork.PassportVisaWorkPermit.GetById(idPassportVisaWorkPermit);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deletePassportVisaWorkPermit")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeletePassportVisaWorkPermit(List<Guid> idPassportVisaWorkPermit)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.PassportVisaWorkPermit.RemoveByList(idPassportVisaWorkPermit, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListPassportVisaWorkPermit")]
        public async Task<IActionResult> GetListPassportVisaWorkPermit(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.PassportVisaWorkPermit.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
