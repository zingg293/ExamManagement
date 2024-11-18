using DPD.HR.Infrastructure.Validation.Models.CategoryGovernmentManagement;
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
    [Route("api/v1/CategoryGovernmentManagement/")]
    public class CategoryGovernmentManagementController : Controller
    {

        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryGovernmentManagementController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public CategoryGovernmentManagementController(IUnitOfWork unitOfWork, ILogger<CategoryGovernmentManagementController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getCategoryGovernmentManagementById")]
        public async Task<IActionResult> GetCategoryGovernmentManagementById(Guid idCategoryGovernmentManagement)
        {
            var templateApi = await _unitOfWork.CategoryGovernmentManagement.GetById(idCategoryGovernmentManagement);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertCategoryGovernmentManagement")]
        public async Task<IActionResult> InsertCategoryGovernmentManagementEmployee(CategoryGovernmentManagement CategoryGovernmentManagementModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategoryGovernmentManagementDto = CategoryGovernmentManagementModel.Adapt<CategoryGovernmentManagementDto>();

            CategoryGovernmentManagementDto.Id = Guid.NewGuid();
            CategoryGovernmentManagementDto.CreatedDate = DateTime.Now;
            CategoryGovernmentManagementDto.Status = 0;

            var result = await _unitOfWork.CategoryGovernmentManagement.Insert(CategoryGovernmentManagementDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateCategoryGovernmentManagement")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateCategoryGovernmentManagement(CategoryGovernmentManagementModel CategoryGovernmentManagementModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategoryGovernmentManagementDto = CategoryGovernmentManagementModel.Adapt<CategoryGovernmentManagementDto>();

            var result = await _unitOfWork.CategoryGovernmentManagement.Update(CategoryGovernmentManagementDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getCategoryGovernmentManagementById")]
        //public async Task<IActionResult> GetCategoryGovernmentManagementById(Guid idCategoryGovernmentManagement)
        //{
        //    var templateApi = await _unitOfWork.CategoryGovernmentManagement.GetById(idCategoryGovernmentManagement);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteCategoryGovernmentManagement")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteCategoryGovernmentManagement(List<Guid> idCategoryGovernmentManagement)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.CategoryGovernmentManagement.RemoveByList(idCategoryGovernmentManagement, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListCategoryGovernmentManagement")]
        public async Task<IActionResult> GetListCategoryGovernmentManagement(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.CategoryGovernmentManagement.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }
    }
}
