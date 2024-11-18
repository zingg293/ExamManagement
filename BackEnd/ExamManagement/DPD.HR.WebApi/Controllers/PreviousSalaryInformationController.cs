using DPD.HR.Infrastructure.Validation.Models.PreviousSalaryInformation;
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
    [Route("api/v1/PreviousSalaryInformation")]
    [ApiController]
    public class PreviousSalaryInformationController : ControllerBase
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PreviousSalaryInformationController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public PreviousSalaryInformationController(IUnitOfWork unitOfWork, ILogger<PreviousSalaryInformationController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getPreviousSalaryInformationById")]
        public async Task<IActionResult> GetPreviousSalaryInformationById(Guid idPreviousSalaryInformation)
        {
            var templateApi = await _unitOfWork.PreviousSalaryInformation.GetById(idPreviousSalaryInformation);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertPreviousSalaryInformation")]
        public async Task<IActionResult> InsertPreviousSalaryInformationEmployee(PreviousSalaryInformation PreviousSalaryInformationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var PreviousSalaryInformationDto = PreviousSalaryInformationModel.Adapt<PreviousSalaryInformationDto>();

            PreviousSalaryInformationDto.Id = Guid.NewGuid();
            PreviousSalaryInformationDto.CreatedDate = DateTime.Now;
            PreviousSalaryInformationDto.Status = 0;

            var result = await _unitOfWork.PreviousSalaryInformation.Insert(PreviousSalaryInformationDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updatePreviousSalaryInformation")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdatePreviousSalaryInformation(PreviousSalaryInformationModel PreviousSalaryInformationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var PreviousSalaryInformationDto = PreviousSalaryInformationModel.Adapt<PreviousSalaryInformationDto>();

            var result = await _unitOfWork.PreviousSalaryInformation.Update(PreviousSalaryInformationDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getPreviousSalaryInformationById")]
        //public async Task<IActionResult> GetPreviousSalaryInformationById(Guid idPreviousSalaryInformation)
        //{
        //    var templateApi = await _unitOfWork.PreviousSalaryInformation.GetById(idPreviousSalaryInformation);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deletePreviousSalaryInformation")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeletePreviousSalaryInformation(List<Guid> idPreviousSalaryInformation)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.PreviousSalaryInformation.RemoveByList(idPreviousSalaryInformation, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListPreviousSalaryInformation")]
        public async Task<IActionResult> GetListPreviousSalaryInformation(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.PreviousSalaryInformation.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
