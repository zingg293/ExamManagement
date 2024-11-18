using DPD.HR.Infrastructure.Validation.Models.MilitaryInformationEmployee;
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
    [Route("api/v1/MilitaryInformationEmployee")]
    [ApiController]
    public class MilitaryInformationEmployeeController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MilitaryInformationEmployeeController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public MilitaryInformationEmployeeController(IUnitOfWork unitOfWork, ILogger<MilitaryInformationEmployeeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getMilitaryInformationEmployeeById")]
        public async Task<IActionResult> GetMilitaryInformationEmployeeById(Guid idMilitaryInformationEmployee)
        {
            var templateApi = await _unitOfWork.MilitaryInformationEmployee.GetById(idMilitaryInformationEmployee);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertMilitaryInformationEmployee")]
        public async Task<IActionResult> InsertMilitaryInformationEmployeeEmployee(MilitaryInformationEmployee MilitaryInformationEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var MilitaryInformationEmployeeDto = MilitaryInformationEmployeeModel.Adapt<MilitaryInformationEmployeeDto>();

            MilitaryInformationEmployeeDto.Id = Guid.NewGuid();
            MilitaryInformationEmployeeDto.CreatedDate = DateTime.Now;
            MilitaryInformationEmployeeDto.Status = 0;

            var result = await _unitOfWork.MilitaryInformationEmployee.Insert(MilitaryInformationEmployeeDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateMilitaryInformationEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateMilitaryInformationEmployee(MilitaryInformationEmployeeModel MilitaryInformationEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var MilitaryInformationEmployeeDto = MilitaryInformationEmployeeModel.Adapt<MilitaryInformationEmployeeDto>();

            var result = await _unitOfWork.MilitaryInformationEmployee.Update(MilitaryInformationEmployeeDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getMilitaryInformationEmployeeById")]
        //public async Task<IActionResult> GetMilitaryInformationEmployeeById(Guid idMilitaryInformationEmployee)
        //{
        //    var templateApi = await _unitOfWork.MilitaryInformationEmployee.GetById(idMilitaryInformationEmployee);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteMilitaryInformationEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteMilitaryInformationEmployee(List<Guid> idMilitaryInformationEmployee)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.MilitaryInformationEmployee.RemoveByList(idMilitaryInformationEmployee, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListMilitaryInformationEmployee")]
        public async Task<IActionResult> GetListMilitaryInformationEmployee(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.MilitaryInformationEmployee.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
