using DPD.HR.Infrastructure.Validation.Models.CategoryTypeSalaryScale;
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
    [Route("api/v1/CategoryTypeSalaryScale")]
    [ApiController]
    public class CategoryTypeSalaryScaleController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryTypeSalaryScaleController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public CategoryTypeSalaryScaleController(IUnitOfWork unitOfWork, ILogger<CategoryTypeSalaryScaleController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getCategoryTypeSalaryScaleById")]
        public async Task<IActionResult> GetCategoryTypeSalaryScaleById(Guid idCategoryTypeSalaryScale)
        {
            var templateApi = await _unitOfWork.CategoryTypeSalaryScale.GetById(idCategoryTypeSalaryScale);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertCategoryTypeSalaryScale")]
        public async Task<IActionResult> InsertCategoryTypeSalaryScaleEmployee(CategoryTypeSalaryScaleModel CategoryTypeSalaryScaleModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategoryTypeSalaryScaleDto = CategoryTypeSalaryScaleModel.Adapt<CategoryTypeSalaryScaleDto>();

            CategoryTypeSalaryScaleDto.Id = Guid.NewGuid();
            CategoryTypeSalaryScaleDto.CreatedDate = DateTime.Now;
            CategoryTypeSalaryScaleDto.Status = 0;

            var result = await _unitOfWork.CategoryTypeSalaryScale.Insert(CategoryTypeSalaryScaleDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateCategoryTypeSalaryScale")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateCategoryTypeSalaryScale(CategoryTypeSalaryScaleModel CategoryTypeSalaryScaleModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategoryTypeSalaryScaleDto = CategoryTypeSalaryScaleModel.Adapt<CategoryTypeSalaryScaleDto>();

            var result = await _unitOfWork.CategoryTypeSalaryScale.Update(CategoryTypeSalaryScaleDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getCategoryTypeSalaryScaleById")]
        //public async Task<IActionResult> GetCategoryTypeSalaryScaleById(Guid idCategoryTypeSalaryScale)
        //{
        //    var templateApi = await _unitOfWork.CategoryTypeSalaryScale.GetById(idCategoryTypeSalaryScale);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteCategoryTypeSalaryScale")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteCategoryTypeSalaryScale(List<Guid> idCategoryTypeSalaryScale)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.CategoryTypeSalaryScale.RemoveByList(idCategoryTypeSalaryScale, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListCategoryTypeSalaryScale")]
        public async Task<IActionResult> GetListCategoryTypeSalaryScale(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.CategoryTypeSalaryScale.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }
    }
}
