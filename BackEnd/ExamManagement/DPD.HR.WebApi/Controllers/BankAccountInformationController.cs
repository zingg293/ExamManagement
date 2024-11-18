using DPD.HR.Infrastructure.Validation.Models.BankAccountInformation;
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
    [Route("api/v1/BankAccountInformation")]
    [ApiController]
    public class BankAccountInformationController : ControllerBase
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BankAccountInformationController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public BankAccountInformationController(IUnitOfWork unitOfWork, ILogger<BankAccountInformationController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getBankAccountInformationById")]
        public async Task<IActionResult> GetBankAccountInformationById(Guid idBankAccountInformation)
        {
            var templateApi = await _unitOfWork.BankAccountInformation.GetById(idBankAccountInformation);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertBankAccountInformation")]
        public async Task<IActionResult> InsertBankAccountInformationEmployee(BankAccountInformation BankAccountInformationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var BankAccountInformationDto = BankAccountInformationModel.Adapt<BankAccountInformationDto>();

            BankAccountInformationDto.Id = Guid.NewGuid();
            BankAccountInformationDto.CreatedDate = DateTime.Now;
            BankAccountInformationDto.Status = 0;

            var result = await _unitOfWork.BankAccountInformation.Insert(BankAccountInformationDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateBankAccountInformation")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateBankAccountInformation(BankAccountInformationModel BankAccountInformationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var BankAccountInformationDto = BankAccountInformationModel.Adapt<BankAccountInformationDto>();

            var result = await _unitOfWork.BankAccountInformation.Update(BankAccountInformationDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getBankAccountInformationById")]
        //public async Task<IActionResult> GetBankAccountInformationById(Guid idBankAccountInformation)
        //{
        //    var templateApi = await _unitOfWork.BankAccountInformation.GetById(idBankAccountInformation);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteBankAccountInformation")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteBankAccountInformation(List<Guid> idBankAccountInformation)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.BankAccountInformation.RemoveByList(idBankAccountInformation, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListBankAccountInformation")]
        public async Task<IActionResult> GetListBankAccountInformation(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.BankAccountInformation.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
