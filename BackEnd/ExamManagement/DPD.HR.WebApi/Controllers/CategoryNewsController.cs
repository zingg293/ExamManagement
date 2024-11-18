using DPD.HR.Infrastructure.Validation.Models.CategoryNews;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/categoryNews")]
public class CategoryNewsController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryNewsController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public CategoryNewsController(IUnitOfWork unitOfWork, ILogger<CategoryNewsController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ CategoryNewsController ]==============================================

    // GET: api/CategoryNews/getListCategoryNewsByIdParent
    [HttpGet("getListCategoryNewsByIdParent")]
    public async Task<IActionResult> GetListCategoryNewsByIdParent(int pageNumber, int pageSize, Guid idParent)
    {
        var templateApi = await _unitOfWork.CategoryNews.GetByIdParent(pageNumber, pageSize, idParent);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryNews/getListCategoryNewsAvailable
    [HttpGet("getListCategoryNewsAvailable")]
    public async Task<IActionResult> GetListCategoryNewsAvailable(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryNews.GetAllAvailable(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryNews/getListCategoryNews
    [HttpGet("getListCategoryNews")]
    public async Task<IActionResult> GetListCategoryNews(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.CategoryNews.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/CategoryNews/getCategoryNewsById
    [HttpGet("getCategoryNewsById")]
    public async Task<IActionResult> GetCategoryNewsById(Guid idCategoryNews)
    {
        var templateApi = await _unitOfWork.CategoryNews.GetById(idCategoryNews);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/CategoryNews/insertCategoryNews
    [HttpPost("insertCategoryNews")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertCategoryNews([FromForm] CategoryNewsModel categoryNewsModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryNewsDto = categoryNewsModel.Adapt<CategoryNewsDto>();

        categoryNewsDto.Id = Guid.NewGuid();
        categoryNewsDto.CreatedDate = DateTime.Now;
        categoryNewsDto.Status = 0;
        categoryNewsDto.IsHide = false;
        categoryNewsDto.IsDeleted = false;
        categoryNewsDto.ShowChild = false;
        categoryNewsDto.UserCreated = idUserCurrent;

        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileCategoryNews))
        {
            Directory.CreateDirectory(AppSettings.ServerFileCategoryNews);
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
                        var filename = Path.Combine(AppSettings.ServerFileCategoryNews,
                            Path.GetFileName($"{categoryNewsDto.Id}.jpg"));

                        await using (var stream = System.IO.File.Create(filename))
                        {
                            await file.CopyToAsync(stream);
                        }

                        categoryNewsDto.Avatar = file.FileName.Split('.')[0] + ".jpg";
                        break;
                    }
                }
            }
        }

        var result = await _unitOfWork.CategoryNews.Insert(categoryNewsDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/CategoryNews/updateCategoryNews
    [HttpPut("updateCategoryNews")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateCategoryNews([FromForm] CategoryNewsModel categoryNewsModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var categoryNewsDto = categoryNewsModel.Adapt<CategoryNewsDto>();

        // If directory does not exist, create it. 
        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileCategoryNews))
        {
            Directory.CreateDirectory(AppSettings.ServerFileCategoryNews);
        }

        if (categoryNewsModel.IdFile is null)
        {
            var idFile = categoryNewsModel.Id + ".jpg";

            var filename = Path.Combine(AppSettings.ServerFileCategoryNews, Path.GetFileName(idFile));

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
                        var filename = Path.Combine(AppSettings.ServerFileCategoryNews,
                            Path.GetFileName($"{categoryNewsModel.Id}.jpg"));

                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }

                        await using (var stream = System.IO.File.Create(filename))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // set data document avatar
                        categoryNewsDto.Avatar = file.FileName.Split('.')[0] + ".jpg";
                        break;
                    }
                }
            }
        }

        var result = await _unitOfWork.CategoryNews.Update(categoryNewsDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/CategoryNews/deleteCategoryNews
    [HttpDelete("deleteCategoryNews")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteCategoryNews(List<Guid> idCategoryNews)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.CategoryNews.RemoveByList(idCategoryNews, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpPut: /api/CategoryNews/hideCategoryNews
    [HttpPut("hideCategoryNews")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> HideCategoryNews(List<Guid> idCategoryNews, bool isHide)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.CategoryNews.HideByList(idCategoryNews, isHide, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpPut: /api/CategoryNews/showCategoryNews
    [HttpPut("showCategoryNews")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> ShowCategoryNews(List<Guid> idCategoryNews, bool isShow)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.CategoryNews.UpdateShowChild(idCategoryNews, isShow, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    // HttpGet: api/CategoryNews/GetFileImage
    [HttpGet]
    [Route("getFileImage")]
    public IActionResult GetFileImage(string fileNameId)
    {
        var temp = fileNameId.Split('.');
        var fileBytes = Array.Empty<byte>();
        if (temp[1] == "jpg" || temp[1] == "png")
        {
            fileBytes = System.IO.File.ReadAllBytes(string.Concat(AppSettings.ServerFileCategoryNews, "\\",
                fileNameId));
        }

        return File(fileBytes, "image/jpeg");
    }

    #endregion
}