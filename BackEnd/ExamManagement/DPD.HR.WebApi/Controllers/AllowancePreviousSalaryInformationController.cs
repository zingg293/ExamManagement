using DPD.HR.Infrastructure.Validation.Models.AllowancePreviousSalaryInformation;
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
    [Route("api/v1/AllowancePreviousSalaryInformation")]
    [ApiController]
    public class AllowancePreviousSalaryInformationController : ControllerBase
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AllowancePreviousSalaryInformationController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public AllowancePreviousSalaryInformationController(IUnitOfWork unitOfWork, ILogger<AllowancePreviousSalaryInformationController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getAllowancePreviousSalaryInformationById")]
        public async Task<IActionResult> GetAllowancePreviousSalaryInformationById(Guid idAllowancePreviousSalaryInformation)
        {
            var templateApi = await _unitOfWork.AllowancePreviousSalaryInformation.GetById(idAllowancePreviousSalaryInformation);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertAllowancePreviousSalaryInformation")]
        public async Task<IActionResult> InsertAllowancePreviousSalaryInformationEmployee(AllowancePreviousSalaryInformation AllowancePreviousSalaryInformationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var AllowancePreviousSalaryInformationDto = AllowancePreviousSalaryInformationModel.Adapt<AllowancePreviousSalaryInformationDto>();

            AllowancePreviousSalaryInformationDto.Id = Guid.NewGuid();
            AllowancePreviousSalaryInformationDto.CreatedDate = DateTime.Now;
            AllowancePreviousSalaryInformationDto.Status = 0;

            var result = await _unitOfWork.AllowancePreviousSalaryInformation.Insert(AllowancePreviousSalaryInformationDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateAllowancePreviousSalaryInformation")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateAllowancePreviousSalaryInformation(AllowancePreviousSalaryInformationModel AllowancePreviousSalaryInformationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var AllowancePreviousSalaryInformationDto = AllowancePreviousSalaryInformationModel.Adapt<AllowancePreviousSalaryInformationDto>();

            var result = await _unitOfWork.AllowancePreviousSalaryInformation.Update(AllowancePreviousSalaryInformationDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getAllowancePreviousSalaryInformationById")]
        //public async Task<IActionResult> GetAllowancePreviousSalaryInformationById(Guid idAllowancePreviousSalaryInformation)
        //{
        //    var templateApi = await _unitOfWork.AllowancePreviousSalaryInformation.GetById(idAllowancePreviousSalaryInformation);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteAllowancePreviousSalaryInformation")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteAllowancePreviousSalaryInformation(List<Guid> idAllowancePreviousSalaryInformation)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.AllowancePreviousSalaryInformation.RemoveByList(idAllowancePreviousSalaryInformation, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListAllowancePreviousSalaryInformation")]
        public async Task<IActionResult> GetListAllowancePreviousSalaryInformation(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.AllowancePreviousSalaryInformation.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
