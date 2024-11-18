using DPD.HR.Infrastructure.Validation.Models.CategorySalaryScale;
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
    [Route("api/v1/CategorySalaryScale")]
    [ApiController]
    public class CategorySalaryScaleController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategorySalaryScaleController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public CategorySalaryScaleController(IUnitOfWork unitOfWork, ILogger<CategorySalaryScaleController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getCategorySalaryScaleById")]
        public async Task<IActionResult> GetCategorySalaryScaleById(Guid idCategorySalaryScale)
        {
            var templateApi = await _unitOfWork.CategorySalaryScale.GetById(idCategorySalaryScale);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertCategorySalaryScale")]
        public async Task<IActionResult> InsertCategorySalaryScaleEmployee(CategorySalaryScaleModel CategorySalaryScaleModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategorySalaryScaleDto = CategorySalaryScaleModel.Adapt<CategorySalaryScaleDto>();

            CategorySalaryScaleDto.Id = Guid.NewGuid();
            CategorySalaryScaleDto.CreatedDate = DateTime.Now;
            CategorySalaryScaleDto.Status = 0;

            var result = await _unitOfWork.CategorySalaryScale.Insert(CategorySalaryScaleDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateCategorySalaryScale")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateCategorySalaryScale(CategorySalaryScaleModel CategorySalaryScaleModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategorySalaryScaleDto = CategorySalaryScaleModel.Adapt<CategorySalaryScaleDto>();

            var result = await _unitOfWork.CategorySalaryScale.Update(CategorySalaryScaleDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getCategorySalaryScaleById")]
        //public async Task<IActionResult> GetCategorySalaryScaleById(Guid idCategorySalaryScale)
        //{
        //    var templateApi = await _unitOfWork.CategorySalaryScale.GetById(idCategorySalaryScale);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteCategorySalaryScale")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteCategorySalaryScale(List<Guid> idCategorySalaryScale)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.CategorySalaryScale.RemoveByList(idCategorySalaryScale, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListCategorySalaryScale")]
        public async Task<IActionResult> GetListCategorySalaryScale(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.CategorySalaryScale.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
