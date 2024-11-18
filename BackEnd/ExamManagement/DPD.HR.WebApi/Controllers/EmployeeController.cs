using DPD.HR.Infrastructure.Validation.Models.Employee;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/employee")]
public class EmployeeController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmployeeController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public EmployeeController(IUnitOfWork unitOfWork, ILogger<EmployeeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ EmployeeController ]=================================================================

    // HttpPost: api/Employee/getListEmployeeByCondition
    [HttpPost("getListEmployeeByCondition")]
    public async Task<IActionResult> GetListEmployeeByCondition(FilterEmployeeModel model)
    {
        var templateApi = await _unitOfWork.Employee.FilterEmployee(model, model.PageNumber, model.PageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPut: api/Employee/updateTypeOfEmployee
    [HttpPut("updateTypeOfEmployee")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateTypeOfEmployee(Guid idEmployee, Guid idTypeOfEmployee)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var templateApi =
            await _unitOfWork.Employee.UpdateEmployeeType(idEmployee, idTypeOfEmployee, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Employee/getEmployeeResigned
    [HttpGet("getEmployeeResigned")]
    public async Task<IActionResult> GetEmployeeResigned(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.Employee.GetEmployeeResigned(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Employee/getEmployeeAndBenefits
    [HttpGet("getEmployeeAndBenefits")]
    public async Task<IActionResult> GetEmployeeAndBenefits(Guid idEmployee)
    {
        var templateApi = await _unitOfWork.Employee.GetEmployeeAndBenefits(idEmployee);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Employee/getEmployeeAndAllowance
    [HttpGet("getEmployeeAndAllowance")]
    public async Task<IActionResult> GetEmployeeAndAllowance(Guid idEmployee)
    {
        var templateApi = await _unitOfWork.Employee.GetEmployeeAndAllowance(idEmployee);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Employee/getListEmployee
    [HttpGet("getListEmployee")]
    public async Task<IActionResult> GetListEmployee(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.Employee.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Employee/getEmployeeById
    [HttpGet("getEmployeeById")]
    public async Task<IActionResult> GetEmployeeById(Guid idEmployee)
    {
        var templateApi = await _unitOfWork.Employee.GetById(idEmployee);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/Employee/insertEmployee
    [HttpPost("insertEmployee")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertEmployee([FromForm] EmployeeModel employeeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var employeeDto = employeeModel.Adapt<EmployeeDto>();

        employeeDto.Id = Guid.NewGuid();
        employeeDto.CreatedDate = DateTime.Now;
        employeeDto.Status = 0;
        employeeDto.Code = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileAvatarEmployee))
        {
            Directory.CreateDirectory(AppSettings.ServerFileAvatarEmployee);
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                var fileContentType = file.ContentType;

                switch (fileContentType)
                {
                    case "image/jpeg":
                    case "image/png":
                    case "image/jpg":
                    {
                        var filename = Path.Combine(AppSettings.ServerFileAvatarEmployee,
                            Path.GetFileName($"{employeeDto.Id}.jpg"));

                        await using (var stream = System.IO.File.Create(filename))
                        {
                            await file.CopyToAsync(stream);
                        }

                        employeeDto.Avatar = file.FileName;
                        break;
                    }
                }
            }
        }

        var result = await _unitOfWork.Employee.Insert(employeeDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/Employee/updateEmployee
    [HttpPut("updateEmployee")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateEmployee([FromForm] EmployeeModel employeeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var employeeDto = employeeModel.Adapt<EmployeeDto>();

        // If directory does not exist, create it. 
        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileAvatarEmployee))
        {
            Directory.CreateDirectory(AppSettings.ServerFileAvatarEmployee);
        }

        if (employeeModel.IdFile is null)
        {
            var idFile = employeeModel.Id + ".jpg";

            var filename = Path.Combine(AppSettings.ServerFileAvatarEmployee, Path.GetFileName(idFile));

            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
        }

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                var fileContentType = file.ContentType;

                switch (fileContentType)
                {
                    case "image/jpeg":
                    case "image/png":
                    case "image/jpg":
                    {
                        var filename = Path.Combine(AppSettings.ServerFileAvatarEmployee,
                            Path.GetFileName($"{employeeModel.Id}.jpg"));

                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }

                        await using (var stream = System.IO.File.Create(filename))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // set data document avatar
                        employeeDto.Avatar = file.FileName;
                        break;
                    }
                }
            }
        }

        var result = await _unitOfWork.Employee.Update(employeeDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/Employee/deleteEmployee
    [HttpDelete("deleteEmployee")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteEmployee(List<Guid> idEmployee)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.Employee.RemoveByList(idEmployee, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpGet: api/Employee/GetFileImage
    [HttpGet]
    [Route("getFileImage")]
    public IActionResult GetFileImage(string fileNameId)
    {
        var temp = fileNameId.Split('.');
        var fileBytes = Array.Empty<byte>();
        if (temp[1] == "jpg" || temp[1] == "png")
        {
            fileBytes = System.IO.File.ReadAllBytes(string.Concat(AppSettings.ServerFileAvatarEmployee, "\\",
                fileNameId));
        }

        return File(fileBytes, "image/jpeg");
    }

    #endregion
}