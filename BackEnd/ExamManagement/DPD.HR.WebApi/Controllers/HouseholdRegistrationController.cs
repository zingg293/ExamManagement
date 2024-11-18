using DPD.HR.Infrastructure.Validation.Models.HouseholdRegistration;
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
    [Route("api/v1/HouseholdRegistration")]
    [ApiController]
    public class HouseholdRegistrationController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HouseholdRegistrationController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public HouseholdRegistrationController(IUnitOfWork unitOfWork, ILogger<HouseholdRegistrationController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getHouseholdRegistrationById")]
        public async Task<IActionResult> GetHouseholdRegistrationById(Guid idHouseholdRegistration)
        {
            var templateApi = await _unitOfWork.HouseholdRegistration.GetById(idHouseholdRegistration);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertHouseholdRegistration")]
        public async Task<IActionResult> InsertHouseholdRegistrationEmployee(HouseholdRegistration HouseholdRegistrationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var HouseholdRegistrationDto = HouseholdRegistrationModel.Adapt<HouseholdRegistrationDto>();

            HouseholdRegistrationDto.Id = Guid.NewGuid();
            HouseholdRegistrationDto.CreatedDate = DateTime.Now;
            HouseholdRegistrationDto.Status = 0;

            var result = await _unitOfWork.HouseholdRegistration.Insert(HouseholdRegistrationDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateHouseholdRegistration")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateHouseholdRegistration(HouseholdRegistrationModel HouseholdRegistrationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var HouseholdRegistrationDto = HouseholdRegistrationModel.Adapt<HouseholdRegistrationDto>();

            var result = await _unitOfWork.HouseholdRegistration.Update(HouseholdRegistrationDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getHouseholdRegistrationById")]
        //public async Task<IActionResult> GetHouseholdRegistrationById(Guid idHouseholdRegistration)
        //{
        //    var templateApi = await _unitOfWork.HouseholdRegistration.GetById(idHouseholdRegistration);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteHouseholdRegistration")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteHouseholdRegistration(List<Guid> idHouseholdRegistration)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.HouseholdRegistration.RemoveByList(idHouseholdRegistration, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListHouseholdRegistration")]
        public async Task<IActionResult> GetListHouseholdRegistration(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.HouseholdRegistration.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
