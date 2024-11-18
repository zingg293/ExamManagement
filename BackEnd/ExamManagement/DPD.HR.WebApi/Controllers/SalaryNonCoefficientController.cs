using DPD.HR.Infrastructure.Validation.Models.SalaryNonCoefficient;
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
    [Route("api/v1/SalaryNonCoefficient")]
    [ApiController]
    public class SalaryNonCoefficientController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SalaryNonCoefficientController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public SalaryNonCoefficientController(IUnitOfWork unitOfWork, ILogger<SalaryNonCoefficientController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getSalaryNonCoefficientById")]
        public async Task<IActionResult> GetSalaryNonCoefficientById(Guid idSalaryNonCoefficient)
        {
            var templateApi = await _unitOfWork.SalaryNonCoefficient.GetById(idSalaryNonCoefficient);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertSalaryNonCoefficient")]
        public async Task<IActionResult> InsertSalaryNonCoefficientEmployee(SalaryNonCoefficient SalaryNonCoefficientModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var SalaryNonCoefficientDto = SalaryNonCoefficientModel.Adapt<SalaryNonCoefficientDto>();

            SalaryNonCoefficientDto.Id = Guid.NewGuid();
            SalaryNonCoefficientDto.CreatedDate = DateTime.Now;
            SalaryNonCoefficientDto.Status = 0;

            var result = await _unitOfWork.SalaryNonCoefficient.Insert(SalaryNonCoefficientDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateSalaryNonCoefficient")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateSalaryNonCoefficient(SalaryNonCoefficientModel SalaryNonCoefficientModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var SalaryNonCoefficientDto = SalaryNonCoefficientModel.Adapt<SalaryNonCoefficientDto>();

            var result = await _unitOfWork.SalaryNonCoefficient.Update(SalaryNonCoefficientDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getSalaryNonCoefficientById")]
        //public async Task<IActionResult> GetSalaryNonCoefficientById(Guid idSalaryNonCoefficient)
        //{
        //    var templateApi = await _unitOfWork.SalaryNonCoefficient.GetById(idSalaryNonCoefficient);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteSalaryNonCoefficient")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteSalaryNonCoefficient(List<Guid> idSalaryNonCoefficient)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.SalaryNonCoefficient.RemoveByList(idSalaryNonCoefficient, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListSalaryNonCoefficient")]
        public async Task<IActionResult> GetListSalaryNonCoefficient(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.SalaryNonCoefficient.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
