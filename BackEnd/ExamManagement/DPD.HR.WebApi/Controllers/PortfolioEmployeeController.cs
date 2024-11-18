using DPD.HR.Infrastructure.Validation.Models.PortfolioEmployee;
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
    [Route("api/v1/PortfolioEmployee")]
    [ApiController]
    public class PortfolioEmployeeController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PortfolioEmployeeController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public PortfolioEmployeeController(IUnitOfWork unitOfWork, ILogger<PortfolioEmployeeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getPortfolioEmployeeById")]
        public async Task<IActionResult> GetPortfolioEmployeeById(Guid idPortfolioEmployee)
        {
            var templateApi = await _unitOfWork.PortfolioEmployee.GetById(idPortfolioEmployee);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertPortfolioEmployee")]
        public async Task<IActionResult> InsertPortfolioEmployeeEmployee(PortfolioEmployee PortfolioEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var PortfolioEmployeeDto = PortfolioEmployeeModel.Adapt<PortfolioEmployeeDto>();

            PortfolioEmployeeDto.Id = Guid.NewGuid();
            PortfolioEmployeeDto.CreatedDate = DateTime.Now;
            PortfolioEmployeeDto.Status = 0;

            var result = await _unitOfWork.PortfolioEmployee.Insert(PortfolioEmployeeDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updatePortfolioEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdatePortfolioEmployee(PortfolioEmployeeModel PortfolioEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var PortfolioEmployeeDto = PortfolioEmployeeModel.Adapt<PortfolioEmployeeDto>();

            var result = await _unitOfWork.PortfolioEmployee.Update(PortfolioEmployeeDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getPortfolioEmployeeById")]
        //public async Task<IActionResult> GetPortfolioEmployeeById(Guid idPortfolioEmployee)
        //{
        //    var templateApi = await _unitOfWork.PortfolioEmployee.GetById(idPortfolioEmployee);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deletePortfolioEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeletePortfolioEmployee(List<Guid> idPortfolioEmployee)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.PortfolioEmployee.RemoveByList(idPortfolioEmployee, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListPortfolioEmployee")]
        public async Task<IActionResult> GetListPortfolioEmployee(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.PortfolioEmployee.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
