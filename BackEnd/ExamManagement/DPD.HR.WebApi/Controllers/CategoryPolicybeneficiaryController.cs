using DPD.HR.Infrastructure.Validation.Models.CategoryCity;
using DPD.HR.Infrastructure.Validation.Models.CategoryPolicybeneficiary;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("/api/v1/categoryPolicybeneficiary")]
    public class CategoryPolicybeneficiaryController : Controller
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryPolicybeneficiaryController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public CategoryPolicybeneficiaryController(IUnitOfWork unitOfWork, ILogger<CategoryPolicybeneficiaryController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ CategoryPolicybeneficiaryController ]==============================================
        [HttpGet("getListCategoryPolicybeneficiary")]
        public async Task<IActionResult> GetListCategoryPolicybeneficiary(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.CategoryPolicybeneficiary.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getCategoryPolicybeneficiaryById")]
        public async Task<IActionResult> GetCategoryPolicybeneficiaryById(Guid idCategoryPolicybeneficiary)
        {
            var templateApi = await _unitOfWork.CategoryPolicybeneficiary.GetById(idCategoryPolicybeneficiary);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }


        [HttpPost("insertCategoryPolicybeneficiary")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> InsertCategoryPolicybeneficiary(CategoryPolicybeneficiaryModel categoryPolicybeneficiaryModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var categoryPolicybeneficiaryDto = categoryPolicybeneficiaryModel.Adapt<CategoryPolicybeneficiaryDto>();

            categoryPolicybeneficiaryDto.Id = Guid.NewGuid();
            categoryPolicybeneficiaryDto.CreatedDate = DateTime.Now;
            categoryPolicybeneficiaryDto.Status = 0;

            var result = await _unitOfWork.CategoryPolicybeneficiary.Insert(categoryPolicybeneficiaryDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateCategoryPolicybeneficiary")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateCategoryPolicybeneficiary(CategoryPolicybeneficiaryModel categoryPolicybeneficiaryModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var categoryPolicybeneficiaryDto = categoryPolicybeneficiaryModel.Adapt<CategoryPolicybeneficiaryDto>();

            var result = await _unitOfWork.CategoryPolicybeneficiary.Update(categoryPolicybeneficiaryDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpDelete("deleteCategoryPolicybeneficiary")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> deleteCategoryPolicybeneficiary(List<Guid> idCategoryPolicybeneficiary)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.CategoryPolicybeneficiary.RemoveByList(idCategoryPolicybeneficiary, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        #endregion
    }
}