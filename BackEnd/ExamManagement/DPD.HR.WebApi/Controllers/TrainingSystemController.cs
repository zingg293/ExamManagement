using CT.EXAMM.Infrastructure.Validation.Models.TrainingSystem;
using DPD.HR.Infrastructure.WebApi.Controllers;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.EXAMM.Infrastructure.WebApi.Controllers
{
    [Route("api/v1/trainingSystem")]
    [ApiController]
    public class TrainingSystemController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TrainingSystemController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public TrainingSystemController(IUnitOfWork unitOfWork, ILogger<TrainingSystemController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ TrainingSystemController ]==============================================
        [HttpGet("getListTrainingSystem")]
        public async Task<IActionResult> GetListTrainingSystem(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.TrainingSystem.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getTrainingSystemById")]
        public async Task<IActionResult> GetTrainingSystemById(Guid idTrainingSystem)
        {
            var templateApi = await _unitOfWork.TrainingSystem.GetById(idTrainingSystem);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        //[HttpPost("insertTrainingSystem")]
        //[Authorize(ListRole.Admin)]
        //public async Task<IActionResult> insertTrainingSystem(TrainingSystemModel TrainingSystemModel)
        //{
        //    var id = (Guid)Request.HttpContext.Items["Id"]!;
        //    var trainingSystemName = (string)Request.HttpContext.Items["TrainingSystemName"]!;
        //    var idEduProgram = (Guid)Request.HttpContext.Items["IdEduProgram"]!;

        //    var TrainingSystemDto = TrainingSystemModel.Adapt<TrainingSysDto>();

        //    TrainingSystemDto.Id = Guid.NewGuid();
        //  //  TrainingSystemDto.CreatedDate = DateTime.Now;
        //    TrainingSystemDto.Status = 0;

        //    var result = await _unitOfWork.TrainingSystem.Insert(TrainingSystemDto, id, trainingSystemName, idEduProgram);

        //    _logger.LogInformation("Thành công : {message}", result.Message);
        //    return Ok(result);
        //}

        [HttpPut("updateTrainingSystem")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateTrainingSystem(TrainingSystemModel TrainingSystemModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var TrainingSystemDto = TrainingSystemModel.Adapt<TrainingSysDto>();

            var result = await _unitOfWork.TrainingSystem.Update(TrainingSystemDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteTrainingSystem")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteTrainingSystem(List<Guid> idTrainingSystem)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.TrainingSystem.RemoveByList(idTrainingSystem, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
