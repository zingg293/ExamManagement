using DPD.HR.Infrastructure.Validation.Models.CategoryNationality;
using DPD.HR.Infrastructure.Validation.Models.CurriculumVitae;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;


namespace DPD.HR.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/CurriculumVitae/")]
    public class CurriculumVitaeController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CurriculumVitaeController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public CurriculumVitaeController(IUnitOfWork unitOfWork, ILogger<CurriculumVitaeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getCurriculumVitaeByIdCurriculumVitae")]
        public async Task<IActionResult> GetCurriculumVitaeByIdCurriculumVitae(Guid idCurriculumVitae)
        {
            var templateApi = await _unitOfWork.CurriculumVitae.GetById(idCurriculumVitae);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertCurriculumVitaeEmployee")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> InsertCurriculumVitaeEmployee(CurriculumVitae curriculumVitaeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var curriculumVitaeDto = curriculumVitaeModel.Adapt<CurriculumVitaeDto>();

            curriculumVitaeDto.Id = Guid.NewGuid();
            curriculumVitaeDto.CreatedDate = DateTime.Now;
            curriculumVitaeDto.Status = 0;

            var result = await _unitOfWork.CurriculumVitae.Insert(curriculumVitaeDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateCurriculumVitae")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateCurriculumVitae(CurriculumVitaeModel curriculumVitaeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var curriculumVitaeDto = curriculumVitaeModel.Adapt<CurriculumVitaeDto>();

            var result = await _unitOfWork.CurriculumVitae.Update(curriculumVitaeDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpGet("getCurriculumVitaeById")]
        public async Task<IActionResult> GetCurriculumVitaeById(Guid idCurriculumVitae)
        {
            var templateApi = await _unitOfWork.CurriculumVitae.GetById(idCurriculumVitae);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpDelete("deleteCurriculumVitae")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteCurriculumVitae(List<Guid> idCurriculumVitae)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.CurriculumVitae.RemoveByList(idCurriculumVitae, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

    }
}