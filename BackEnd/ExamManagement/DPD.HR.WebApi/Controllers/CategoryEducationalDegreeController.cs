using DPD.HR.Infrastructure.Validation.Models.CategoryEducationalDegree;
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
    [Route("api/v1/CategoryEducationalDegree")]
    [ApiController]
    public class CategoryEducationalDegreeController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryEducationalDegreeController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public CategoryEducationalDegreeController(IUnitOfWork unitOfWork, ILogger<CategoryEducationalDegreeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getCategoryEducationalDegreeById")]
        public async Task<IActionResult> GetCategoryEducationalDegreeById(Guid idCategoryEducationalDegree)
        {
            var templateApi = await _unitOfWork.CategoryEducationalDegree.GetById(idCategoryEducationalDegree);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertCategoryEducationalDegree")]
        public async Task<IActionResult> InsertCategoryEducationalDegreeEmployee(CategoryEducationalDegree CategoryEducationalDegreeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategoryEducationalDegreeDto = CategoryEducationalDegreeModel.Adapt<CategoryEducationalDegreeDto>();

            CategoryEducationalDegreeDto.Id = Guid.NewGuid();
            CategoryEducationalDegreeDto.CreatedDate = DateTime.Now;
            CategoryEducationalDegreeDto.Status = 0;

            var result = await _unitOfWork.CategoryEducationalDegree.Insert(CategoryEducationalDegreeDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateCategoryEducationalDegree")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateCategoryEducationalDegree(CategoryEducationalDegreeModel CategoryEducationalDegreeModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategoryEducationalDegreeDto = CategoryEducationalDegreeModel.Adapt<CategoryEducationalDegreeDto>();

            var result = await _unitOfWork.CategoryEducationalDegree.Update(CategoryEducationalDegreeDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getCategoryEducationalDegreeById")]
        //public async Task<IActionResult> GetCategoryEducationalDegreeById(Guid idCategoryEducationalDegree)
        //{
        //    var templateApi = await _unitOfWork.CategoryEducationalDegree.GetById(idCategoryEducationalDegree);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteCategoryEducationalDegree")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteCategoryEducationalDegree(List<Guid> idCategoryEducationalDegree)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.CategoryEducationalDegree.RemoveByList(idCategoryEducationalDegree, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListCategoryEducationalDegree")]
        public async Task<IActionResult> GetListCategoryEducationalDegree(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.CategoryEducationalDegree.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
