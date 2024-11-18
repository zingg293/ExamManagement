using DPD.HR.Infrastructure.Validation.Models.CategorySalaryLevel;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers
{
    [Route("api/v1/CategorySalaryLevel")]
    [ApiController]
    public class CategorySalaryLevelController : Controller
    {


        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategorySalaryLevelController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================

        public CategorySalaryLevelController(IUnitOfWork unitOfWork, ILogger<CategorySalaryLevelController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        [HttpGet("getCategorySalaryLevelById")]
        public async Task<IActionResult> GetCategorySalaryLevelById(Guid idCategorySalaryLevel)
        {
            var templateApi = await _unitOfWork.CategorySalaryLevel.GetById(idCategorySalaryLevel);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [Authorize(ListRole.Admin)]
        [HttpPost("insertCategorySalaryLevel")]
        public async Task<IActionResult> InsertCategorySalaryLevelEmployee(CategorySalaryLevel CategorySalaryLevelModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategorySalaryLevelDto = CategorySalaryLevelModel.Adapt<CategorySalaryLevelDto>();

            CategorySalaryLevelDto.Id = Guid.NewGuid();
            CategorySalaryLevelDto.CreatedDate = DateTime.Now;
            CategorySalaryLevelDto.Status = 0;

            var result = await _unitOfWork.CategorySalaryLevel.Insert(CategorySalaryLevelDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        [HttpPut("updateCategorySalaryLevel")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateCategorySalaryLevel(CategorySalaryLevelModel CategorySalaryLevelModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var CategorySalaryLevelDto = CategorySalaryLevelModel.Adapt<CategorySalaryLevelDto>();

            var result = await _unitOfWork.CategorySalaryLevel.Update(CategorySalaryLevelDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }


        //[HttpGet("getCategorySalaryLevelById")]
        //public async Task<IActionResult> GetCategorySalaryLevelById(Guid idCategorySalaryLevel)
        //{
        //    var templateApi = await _unitOfWork.CategorySalaryLevel.GetById(idCategorySalaryLevel);
        //    _logger.LogInformation("Thành công : {message}", templateApi.Message);
        //    return Ok(templateApi);
        //}

        [HttpDelete("deleteCategorySalaryLevel")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteCategorySalaryLevel(List<Guid> idCategorySalaryLevel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.CategorySalaryLevel.RemoveByList(idCategorySalaryLevel, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }

        [HttpGet("getListCategorySalaryLevel")]
        public async Task<IActionResult> GetListCategorySalaryLevel(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.CategorySalaryLevel.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

    }
}
