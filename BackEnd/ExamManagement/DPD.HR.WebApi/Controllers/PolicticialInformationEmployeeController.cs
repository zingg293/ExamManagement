using DPD.HR.Infrastructure.Validation.Models.PolicticialInformationEmployee;
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
    [Route("api/v1/PolicticialInformationEmployee")]
    [ApiController]
    public class PolicticialInformationEmployeeController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PolicticialInformationEmployeeController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public PolicticialInformationEmployeeController(IUnitOfWork unitOfWork, ILogger<PolicticialInformationEmployeeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getPolicticialInformationEmployeeById")]
        public async Task<IActionResult> GetPolicticialInformationEmployeeById(Guid idPolicticialInformationEmployee)
        {
            var templateApi = await _unitOfWork.PolicticialInformationEmployee.GetById(idPolicticialInformationEmployee);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertPolicticialInformationEmployee")]
        public async Task<IActionResult> InsertPolicticialInformationEmployeeEmployee(PolicticialInformationEmployee PolicticialInformationEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var PolicticialInformationEmployeeDto = PolicticialInformationEmployeeModel.Adapt<PolicticialInformationEmployeeDto>();

            PolicticialInformationEmployeeDto.Id = Guid.NewGuid();
            PolicticialInformationEmployeeDto.CreatedDate = DateTime.Now;
            PolicticialInformationEmployeeDto.Status = 0;

            var result = await _unitOfWork.PolicticialInformationEmployee.Insert(PolicticialInformationEmployeeDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updatePolicticialInformationEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdatePolicticialInformationEmployee(PolicticialInformationEmployeeModel PolicticialInformationEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var PolicticialInformationEmployeeDto = PolicticialInformationEmployeeModel.Adapt<PolicticialInformationEmployeeDto>();

            var result = await _unitOfWork.PolicticialInformationEmployee.Update(PolicticialInformationEmployeeDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getPolicticialInformationEmployeeById")]
        //public async Task<IActionResult> GetPolicticialInformationEmployeeById(Guid idPolicticialInformationEmployee)
        //{
        //    var templateApi = await _unitOfWork.PolicticialInformationEmployee.GetById(idPolicticialInformationEmployee);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deletePolicticialInformationEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeletePolicticialInformationEmployee(List<Guid> idPolicticialInformationEmployee)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.PolicticialInformationEmployee.RemoveByList(idPolicticialInformationEmployee, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListPolicticialInformationEmployee")]
        public async Task<IActionResult> GetListPolicticialInformationEmployee(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.PolicticialInformationEmployee.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
