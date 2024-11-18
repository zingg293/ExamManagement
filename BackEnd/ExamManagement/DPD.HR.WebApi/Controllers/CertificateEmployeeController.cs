using DPD.HR.Infrastructure.Validation.Models.CertificateEmployee;
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
    [Route("api/v1/CertificateEmployee")]
    [ApiController]
    public class CertificateEmployeeController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CertificateEmployeeController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public CertificateEmployeeController(IUnitOfWork unitOfWork, ILogger<CertificateEmployeeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getCertificateEmployeeById")]
        public async Task<IActionResult> GetCertificateEmployeeById(Guid idCertificateEmployee)
        {
            var templateApi = await _unitOfWork.CertificateEmployee.GetById(idCertificateEmployee);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertCertificateEmployee")]
        public async Task<IActionResult> InsertCertificateEmployeeEmployee(CertificateEmployee CertificateEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CertificateEmployeeDto = CertificateEmployeeModel.Adapt<CertificateEmployeeDto>();

            CertificateEmployeeDto.Id = Guid.NewGuid();
            CertificateEmployeeDto.CreatedDate = DateTime.Now;
            CertificateEmployeeDto.Status = 0;

            var result = await _unitOfWork.CertificateEmployee.Insert(CertificateEmployeeDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateCertificateEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateCertificateEmployee(CertificateEmployeeModel CertificateEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CertificateEmployeeDto = CertificateEmployeeModel.Adapt<CertificateEmployeeDto>();

            var result = await _unitOfWork.CertificateEmployee.Update(CertificateEmployeeDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getCertificateEmployeeById")]
        //public async Task<IActionResult> GetCertificateEmployeeById(Guid idCertificateEmployee)
        //{
        //    var templateApi = await _unitOfWork.CertificateEmployee.GetById(idCertificateEmployee);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteCertificateEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteCertificateEmployee(List<Guid> idCertificateEmployee)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.CertificateEmployee.RemoveByList(idCertificateEmployee, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListCertificateEmployee")]
        public async Task<IActionResult> GetListCertificateEmployee(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.CertificateEmployee.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
