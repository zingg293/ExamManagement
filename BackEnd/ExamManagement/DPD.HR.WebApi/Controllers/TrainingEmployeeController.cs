using DPD.HR.Infrastructure.Validation.Models.TrainingEmployee;
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
    [Route("api/v1/TrainingEmployee")]
    [ApiController]
    public class TrainingEmployeeController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TrainingEmployeeController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public TrainingEmployeeController(IUnitOfWork unitOfWork, ILogger<TrainingEmployeeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getTrainingEmployeeById")]
        public async Task<IActionResult> GetTrainingEmployeeById(Guid idTrainingEmployee)
        {
            var templateApi = await _unitOfWork.TrainingEmployee.GetById(idTrainingEmployee);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertTrainingEmployee")]
        public async Task<IActionResult> InsertTrainingEmployeeEmployee(TrainingEmployee TrainingEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var TrainingEmployeeDto = TrainingEmployeeModel.Adapt<TrainingEmployeeDto>();

            TrainingEmployeeDto.Id = Guid.NewGuid();
            TrainingEmployeeDto.CreatedDate = DateTime.Now;
            TrainingEmployeeDto.Status = 0;

            var result = await _unitOfWork.TrainingEmployee.Insert(TrainingEmployeeDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateTrainingEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateTrainingEmployee(TrainingEmployeeModel TrainingEmployeeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var TrainingEmployeeDto = TrainingEmployeeModel.Adapt<TrainingEmployeeDto>();

            var result = await _unitOfWork.TrainingEmployee.Update(TrainingEmployeeDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getTrainingEmployeeById")]
        //public async Task<IActionResult> GetTrainingEmployeeById(Guid idTrainingEmployee)
        //{
        //    var templateApi = await _unitOfWork.TrainingEmployee.GetById(idTrainingEmployee);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteTrainingEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteTrainingEmployee(List<Guid> idTrainingEmployee)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.TrainingEmployee.RemoveByList(idTrainingEmployee, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListTrainingEmployee")]
        public async Task<IActionResult> GetListTrainingEmployee(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.TrainingEmployee.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
