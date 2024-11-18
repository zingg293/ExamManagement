using DPD.HR.Infrastructure.Validation.Models.CategoryCity;
using DPD.HR.Infrastructure.Validation.Models.CategoryNationality;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;



namespace DPD.HR.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("/api/v1/categoryNationality")]
    public class CategoryNationalityController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryNationalityController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public CategoryNationalityController(IUnitOfWork unitOfWork, ILogger<CategoryNationalityController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ CategoryNationalityController ]==============================================
        [HttpGet("getListCategoryNationality")]
        public async Task<IActionResult> GetListCategoryNationality(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.CategoryNationality.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getCategorynationalityById")]
        public async Task<IActionResult> GetCategorynationalityById(Guid idCategoryNationality)
        {
            var templateApi = await _unitOfWork.CategoryNationality.GetById(idCategoryNationality);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertCategorynationality")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertCategorynationality(CategoryNationalityModel categoryNationalityModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var categoryNationalityDto = categoryNationalityModel.Adapt<CategoryNationalityDto>();

            categoryNationalityDto.Id = Guid.NewGuid();
            categoryNationalityDto.CreatedDate = DateTime.Now;
            categoryNationalityDto.Status = 0;

            var result = await _unitOfWork.CategoryNationality.Insert(categoryNationalityDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateCategoryNationality")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateCategoryNationality(CategoryNationalityModel categoryNationalityModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var categoryNationalityDto = categoryNationalityModel.Adapt<CategoryNationalityDto>();

            var result = await _unitOfWork.CategoryNationality.Update(categoryNationalityDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteCategoryNationality")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteCategoryNationality(List<Guid> idCategoryNationality)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.CategoryNationality.RemoveByList(idCategoryNationality, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
