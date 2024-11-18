using CT.EXAMM.Infrastructure.Validation.Models.Room;
using DPD.HR.Infrastructure.WebApi.Controllers;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.EXAMM.Infrastructure.WebApi.Controllers
{
    [Route("api/v1/room")]
    [ApiController]
    public class RoomController : Controller   
    {
        #region ===[ Private Members ]=============================================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoomController> _logger;
        #endregion

        #region ===[ Constructor ]=================================================================
        public RoomController(IUnitOfWork unitOfWork, ILogger<RoomController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region ===[ RoomController ]==============================================
        [HttpGet("getListRoom")]
        public async Task<IActionResult> GetListRoom(int pageNumber, int pageSize)
        {
            var templateApi = await _unitOfWork.Room.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpGet("getRoomById")]
        public async Task<IActionResult> GetRoomById(Guid idRoom)
        {
            var templateApi = await _unitOfWork.Room.GetById(idRoom);
            _logger.LogInformation("Thành công : {message}", templateApi.Message);
            return Ok(templateApi);
        }

        [HttpPost("insertRoom")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> insertRoom(RoomModel RoomModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var RoomDto = RoomModel.Adapt<RoomDto>();

            RoomDto.Id = Guid.NewGuid();
            RoomDto.CreatedDate = DateTime.Now;
         //   RoomDto.Status = 0;

            var result = await _unitOfWork.Room.Insert(RoomDto, idUserCurrent, nameUserCurrent);

            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }

        [HttpPut("updateRoom")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> updateRoom(RoomModel RoomModel)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var RoomDto = RoomModel.Adapt<RoomDto>();

            var result = await _unitOfWork.Room.Update(RoomDto, idUserCurrent, nameUserCurrent);
            _logger.LogInformation("Thành công : {message}", result.Message);
            return Ok(result);
        }
        [HttpDelete("deleteRoom")]
        [Authorize(ListRole.Admin)]
        public async Task<IActionResult> DeleteRoom(List<Guid> idRoom)
        {
            var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
            var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

            var result = await _unitOfWork.Room.RemoveByList(idRoom, idUserCurrent, nameUserCurrent);
            return Ok(result);
        }
        #endregion

    }
}
