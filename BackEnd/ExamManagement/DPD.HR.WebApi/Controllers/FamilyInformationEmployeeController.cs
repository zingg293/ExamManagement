using DPD.HR.Infrastructure.Validation.Models.FamilyInformationEmployee;
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
    [Route("api/v1/FamilyInformationEmployee")]
    [ApiController]
    public class FamilyInformationEmployeeController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FamilyInformationEmployeeController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public FamilyInformationEmployeeController(IUnitOfWork unitOfWork, ILogger<FamilyInformationEmployeeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getFamilyInformationEmployeeById")]
        public async Task<IActionResult> GetFamilyInformationEmployeeById(Guid idFamilyInformationEmployee)
        {
            var templateApi = await _unitOfWork.FamilyInformationEmployee.GetById(idFamilyInformationEmployee);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertFamilyInformationEmployee")]
        public async Task<IActionResult> InsertFamilyInformationEmployeeEmployee(FamilyInformationEmployee FamilyInformationEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var FamilyInformationEmployeeDto = FamilyInformationEmployeeModel.Adapt<FamilyInformationEmployeeDto>();

            FamilyInformationEmployeeDto.Id = Guid.NewGuid();
            FamilyInformationEmployeeDto.CreatedDate = DateTime.Now;
            FamilyInformationEmployeeDto.Status = 0;

            var result = await _unitOfWork.FamilyInformationEmployee.Insert(FamilyInformationEmployeeDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateFamilyInformationEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateFamilyInformationEmployee(FamilyInformationEmployeeModel FamilyInformationEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var FamilyInformationEmployeeDto = FamilyInformationEmployeeModel.Adapt<FamilyInformationEmployeeDto>();

            var result = await _unitOfWork.FamilyInformationEmployee.Update(FamilyInformationEmployeeDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getFamilyInformationEmployeeById")]
        //public async Task<IActionResult> GetFamilyInformationEmployeeById(Guid idFamilyInformationEmployee)
        //{
        //    var templateApi = await _unitOfWork.FamilyInformationEmployee.GetById(idFamilyInformationEmployee);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteFamilyInformationEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteFamilyInformationEmployee(List<Guid> idFamilyInformationEmployee)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.FamilyInformationEmployee.RemoveByList(idFamilyInformationEmployee, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListFamilyInformationEmployee")]
        public async Task<IActionResult> GetListFamilyInformationEmployee(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.FamilyInformationEmployee.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
