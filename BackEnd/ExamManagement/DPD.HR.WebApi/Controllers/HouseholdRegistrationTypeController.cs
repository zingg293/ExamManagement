using DPD.HR.Infrastructure.Validation.Models.HouseholdRegistrationType;
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
    [Route("api/v1/HouseholdRegistrationType")]
    [ApiController]
    public class HouseholdRegistrationTypeController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HouseholdRegistrationTypeController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public HouseholdRegistrationTypeController(IUnitOfWork unitOfWork, ILogger<HouseholdRegistrationTypeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getHouseholdRegistrationTypeById")]
        public async Task<IActionResult> GetHouseholdRegistrationTypeById(Guid idHouseholdRegistrationType)
        {
            var templateApi = await _unitOfWork.HouseholdRegistrationType.GetById(idHouseholdRegistrationType);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertHouseholdRegistrationType")]
        public async Task<IActionResult> InsertHouseholdRegistrationTypeEmployee(HouseholdRegistrationType HouseholdRegistrationTypeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var HouseholdRegistrationTypeDto = HouseholdRegistrationTypeModel.Adapt<HouseholdRegistrationTypeDto>();

            HouseholdRegistrationTypeDto.Id = Guid.NewGuid();
            HouseholdRegistrationTypeDto.CreatedDate = DateTime.Now;
            HouseholdRegistrationTypeDto.Status = 0;

            var result = await _unitOfWork.HouseholdRegistrationType.Insert(HouseholdRegistrationTypeDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateHouseholdRegistrationType")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateHouseholdRegistrationType(HouseholdRegistrationTypeModel HouseholdRegistrationTypeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var HouseholdRegistrationTypeDto = HouseholdRegistrationTypeModel.Adapt<HouseholdRegistrationTypeDto>();

            var result = await _unitOfWork.HouseholdRegistrationType.Update(HouseholdRegistrationTypeDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getHouseholdRegistrationTypeById")]
        //public async Task<IActionResult> GetHouseholdRegistrationTypeById(Guid idHouseholdRegistrationType)
        //{
        //    var templateApi = await _unitOfWork.HouseholdRegistrationType.GetById(idHouseholdRegistrationType);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteHouseholdRegistrationType")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteHouseholdRegistrationType(List<Guid> idHouseholdRegistrationType)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.HouseholdRegistrationType.RemoveByList(idHouseholdRegistrationType, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListHouseholdRegistrationType")]
        public async Task<IActionResult> GetListHouseholdRegistrationType(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.HouseholdRegistrationType.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
