using DPD.HR.Infrastructure.Validation.Models.CategoryProfessionalQualification;
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

    [ApiController]
    [Route("api/v1/CategoryProfessionalQualification/")]
    public class CategoryProfessionalQualificationController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryProfessionalQualificationController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public CategoryProfessionalQualificationController(IUnitOfWork unitOfWork, ILogger<CategoryProfessionalQualificationController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getCategoryProfessionalQualificationById")]
        public async Task<IActionResult> GetCategoryProfessionalQualificationByIdCategoryProfessionalQualification(Guid idCategoryProfessionalQualification)
        {
            var templateApi = await _unitOfWork.CategoryProfessionalQualification.GetById(idCategoryProfessionalQualification);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertCategoryProfessionalQualification")]
        public async Task<IActionResult> InsertCategoryProfessionalQualificationEmployee(CategoryProfessionalQualification CategoryProfessionalQualificationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategoryProfessionalQualificationDto = CategoryProfessionalQualificationModel.Adapt<CategoryProfessionalQualificationDto>();

            CategoryProfessionalQualificationDto.Id = Guid.NewGuid();
            CategoryProfessionalQualificationDto.CreatedDate = DateTime.Now;
            CategoryProfessionalQualificationDto.Status = 0;

            var result = await _unitOfWork.CategoryProfessionalQualification.Insert(CategoryProfessionalQualificationDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateCategoryProfessionalQualification")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateCategoryProfessionalQualification(CategoryProfessionalQualificationModel CategoryProfessionalQualificationModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategoryProfessionalQualificationDto = CategoryProfessionalQualificationModel.Adapt<CategoryProfessionalQualificationDto>();

            var result = await _unitOfWork.CategoryProfessionalQualification.Update(CategoryProfessionalQualificationDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getCategoryProfessionalQualificationById")]
        //public async Task<IActionResult> GetCategoryProfessionalQualificationById(Guid idCategoryProfessionalQualification)
        //{
        //    var templateApi = await _unitOfWork.CategoryProfessionalQualification.GetById(idCategoryProfessionalQualification);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteCategoryProfessionalQualification")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteCategoryProfessionalQualification(List<Guid> idCategoryProfessionalQualification)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.CategoryProfessionalQualification.RemoveByList(idCategoryProfessionalQualification, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListCategoryProfessionalQualification")]
        public async Task<IActionResult> GetListCategoryProfessionalQualification(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.CategoryProfessionalQualification.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }
    }
}
