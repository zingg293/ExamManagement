using CT.EXAMM.Infrastructure.Validation.Models.RollCall;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.EXAMM.Infrastructure.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RollCallController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RollCallController> _logger;

        public RollCallController(IUnitOfWork unitOfWork, ILogger<RollCallController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("getListRollCall")]
        public async Task<IActionResult> GetListRollCall(int pageNumber, int pageSize)
        {
            var result = await _unitOfWork.RollCall.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpGet("getRollCallById")]
        public async Task<IActionResult> GetRollCallById(Guid id)
        {
            var result = await _unitOfWork.RollCall.GetById(id);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPost("insertRollCall")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> InsertRollCall(RollCallModel model)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var dto = model.Adapt<RollCallDto>();
            dto.Id = Guid.NewGuid();
            dto.CreateDated = DateTime.Now;
         

            var result = await _unitOfWork.RollCall.Insert(dto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateRollCall")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> UpdateRollCall(RollCallModel model)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var dto = model.Adapt<RollCallDto>();

            var result = await _unitOfWork.RollCall.Update(dto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpDelete("deleteRollCall")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteRollCall(List<Guid> ids)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.RollCall.RemoveByList(ids, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
    }
}
