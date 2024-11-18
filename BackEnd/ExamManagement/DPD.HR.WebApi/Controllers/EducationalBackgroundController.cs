using DPD.HR.Infrastructure.Validation.Models.EducationalBackground;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers
{
    [Route("api/v1/EducationalBackground")]
    [ApiController]
    public class EducationalBackgroundController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EducationalBackgroundController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public EducationalBackgroundController(IUnitOfWork unitOfWork, ILogger<EducationalBackgroundController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getEducationalBackgroundById")]
        public async Task<IActionResult> GetEducationalBackgroundById(Guid idEducationalBackground)
        {
            var templateApi = await _unitOfWork.EducationalBackground.GetById(idEducationalBackground);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertEducationalBackground")]
        public async Task<IActionResult> InsertEducationalBackgroundEmployee(EducationalBackground EducationalBackgroundModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var EducationalBackgroundDto = EducationalBackgroundModel.Adapt<EducationalBackgroundDto>();

            EducationalBackgroundDto.Id = Guid.NewGuid();
            EducationalBackgroundDto.CreatedDate = DateTime.Now;
            EducationalBackgroundDto.Status = 0;

            var result = await _unitOfWork.EducationalBackground.Insert(EducationalBackgroundDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateEducationalBackground")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateEducationalBackground(EducationalBackgroundModel EducationalBackgroundModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var EducationalBackgroundDto = EducationalBackgroundModel.Adapt<EducationalBackgroundDto>();

            var result = await _unitOfWork.EducationalBackground.Update(EducationalBackgroundDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getEducationalBackgroundById")]
        //public async Task<IActionResult> GetEducationalBackgroundById(Guid idEducationalBackground)
        //{
        //    var templateApi = await _unitOfWork.EducationalBackground.GetById(idEducationalBackground);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteEducationalBackground")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteEducationalBackground(List<Guid> idEducationalBackground)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.EducationalBackground.RemoveByList(idEducationalBackground, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListEducationalBackground")]
        public async Task<IActionResult> GetListEducationalBackground(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.EducationalBackground.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
