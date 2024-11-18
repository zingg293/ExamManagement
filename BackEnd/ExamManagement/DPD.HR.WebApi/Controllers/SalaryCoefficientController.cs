using DPD.HR.Infrastructure.Validation.Models.SalaryCoefficient;
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
    [Route("api/v1/SalaryCoefficient")]
    [ApiController]
    public class SalaryCoefficientController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SalaryCoefficientController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public SalaryCoefficientController(IUnitOfWork unitOfWork, ILogger<SalaryCoefficientController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getSalaryCoefficientById")]
        public async Task<IActionResult> GetSalaryCoefficientById(Guid idSalaryCoefficient)
        {
            var templateApi = await _unitOfWork.SalaryCoefficient.GetById(idSalaryCoefficient);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertSalaryCoefficient")]
        public async Task<IActionResult> InsertSalaryCoefficientEmployee(SalaryCoefficient SalaryCoefficientModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var SalaryCoefficientDto = SalaryCoefficientModel.Adapt<SalaryCoefficientDto>();

            SalaryCoefficientDto.Id = Guid.NewGuid();
            SalaryCoefficientDto.CreatedDate = DateTime.Now;
            SalaryCoefficientDto.Status = 0;

            var result = await _unitOfWork.SalaryCoefficient.Insert(SalaryCoefficientDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateSalaryCoefficient")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateSalaryCoefficient(SalaryCoefficientModel SalaryCoefficientModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var SalaryCoefficientDto = SalaryCoefficientModel.Adapt<SalaryCoefficientDto>();

            var result = await _unitOfWork.SalaryCoefficient.Update(SalaryCoefficientDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getSalaryCoefficientById")]
        //public async Task<IActionResult> GetSalaryCoefficientById(Guid idSalaryCoefficient)
        //{
        //    var templateApi = await _unitOfWork.SalaryCoefficient.GetById(idSalaryCoefficient);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteSalaryCoefficient")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteSalaryCoefficient(List<Guid> idSalaryCoefficient)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.SalaryCoefficient.RemoveByList(idSalaryCoefficient, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListSalaryCoefficient")]
        public async Task<IActionResult> GetListSalaryCoefficient(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.SalaryCoefficient.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
