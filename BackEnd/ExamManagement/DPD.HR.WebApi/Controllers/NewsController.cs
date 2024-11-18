using DPD.HR.Infrastructure.Validation.Models.News;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/news")]
public class NewsController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<NewsController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public NewsController(IUnitOfWork unitOfWork, ILogger<NewsController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ NewsController ]==============================================

    // GET: api/News/filterListNews
    [HttpPost("filterListNews")]
    public async Task<IActionResult> FilterListNews(FilterSearchNewsModel model)
    {
        var templateApi = await _unitOfWork.News.FilterSearchNews(model, model.PageNumber, model.PageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/News/searchNews
    [HttpGet("searchNews")]
    public async Task<IActionResult> SearchNews(int pageNumber, int pageSize, string filter)
    {
        var templateApi = await _unitOfWork.News.SearchNews(filter, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/News/getListNewsByIdCategoryNews
    [HttpGet("getListNewsByIdCategoryNews")]
    public async Task<IActionResult> GetListNewsByIdCategoryNews(int pageNumber, int pageSize, Guid idCategoryNews)
    {
        var templateApi = await _unitOfWork.News.GetAllByIdCategoryNews(idCategoryNews, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpGet: api/News/getFileImageNews
    [HttpGet]
    [Route("getFileImageNews")]
    public IActionResult GetFileImageNews(string fileNameId)
    {
        var temp = fileNameId.Split('.');
        var fileBytes = Array.Empty<byte>();
        if (temp[1] == "jpg" || temp[1] == "png")
        {
            fileBytes = System.IO.File.ReadAllBytes(string.Concat(AppSettings.ServerFileNews, "\\",
                fileNameId));
        }

        return File(fileBytes, "image/jpeg");
    }

    // HttpPost: api/News/saveNewsImage
    [HttpPost("saveNewsImage")]
    public async Task<IActionResult> SaveNewsImage()
    {
        EnsureDirectoriesExist();

        if (Request.Form.Files.Count <= 0) return Ok();

        foreach (var file in Request.Form.Files)
        {
            if (!IsImageFile(file)) continue;

            var idFile = Guid.NewGuid() + ".jpg";
            var filePath = Path.Combine(AppSettings.ServerFileNews, idFile);

            await using var stream = System.IO.File.Create(filePath);
            await file.CopyToAsync(stream);

            return Ok(idFile); // Return early after the first valid image is saved.
        }

        return Ok();
    }

    // GET: api/News/getListNewsApproved
    [HttpGet("getListNewsApproved")]
    public async Task<IActionResult> GetListNewsApproved(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.News.GetAllAvailable(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/News/getListNews
    [HttpGet("getListNews")]
    public async Task<IActionResult> GetListNews(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.News.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/News/getNewsById
    [HttpGet("getNewsById")]
    public async Task<IActionResult> GetNewsById(Guid idNews)
    {
        var templateApi = await _unitOfWork.News.GetById(idNews);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/News/insertNews
    [HttpPost("insertNews")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertNews([FromForm] NewsModel news)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var newsDto = news.Adapt<NewsDto>();
        newsDto.Id = Guid.NewGuid();
        newsDto.CreatedDate = DateTime.Now;
        newsDto.Status = 0;
        newsDto.IsHide = false;
        newsDto.IsDeleted = false;
        newsDto.IsApproved = false;
        newsDto.UserCreated = idUserCurrent;

        EnsureDirectoriesExist();

        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                if (!IsImageFile(file)) continue;

                var filename = Path.Combine(AppSettings.ServerFileNews,
                    Path.GetFileName($"{newsDto.Id}.jpg"));

                await using (var stream = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(stream);
                }

                newsDto.Avatar = file.FileName;
                newsDto.ExtensionFile = file.FileName.Split('.')[1];
                newsDto.FilePath = AppSettings.ServerFileNews;
                break;
            }
        }

        var result = await _unitOfWork.News.Insert(newsDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/News/updateNews
    [HttpPut("updateNews")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateNews([FromForm] NewsModel news)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var newsDto = news.Adapt<NewsDto>();
        newsDto.UserUpdated = idUserCurrent;
        newsDto.UpdateDate = DateTime.Now;

        EnsureDirectoriesExist();

        if (newsDto.IdFile is null)
        {
            var idFile = newsDto.Id + ".jpg";

            var filename = Path.Combine(AppSettings.ServerFileNews, Path.GetFileName(idFile));

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
                        var filename = Path.Combine(AppSettings.ServerFileNews,
                            Path.GetFileName($"{newsDto.Id}.jpg"));

                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }

                        await using (var stream = System.IO.File.Create(filename))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // set data document avatar
                        newsDto.Avatar = file.FileName;
                        newsDto.ExtensionFile = file.FileName.Split('.')[1];
                        newsDto.FilePath = AppSettings.ServerFileNews;
                        break;
                    }
                }
            }
        }

        var result = await _unitOfWork.News.Update(newsDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/News/deleteNews
    [HttpDelete("deleteNews")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteNews(List<Guid> idNews)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.News.RemoveByList(idNews, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: /api/News/hideNews
    [HttpPut("hideNews")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> HideNews(List<Guid> idNews, bool isHide)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.News.HideByList(idNews, isHide, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: /api/News/approveNews
    [HttpPut("approveNews")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> ApproveNews(List<Guid> idNews, bool isApprove)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.News.ApproveNews(idNews, isApprove, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: /api/News/increaseNumberView
    [HttpPut("increaseNumberView")]
    public async Task<IActionResult> IncreaseNumberView(Guid idNews)
    {
        var idUserCurrent = Guid.Empty;
        var nameUserCurrent = "";

        var result = await _unitOfWork.News.UpdateNumberView(idNews, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: /api/News/increaseNumberLike
    [HttpPut("increaseNumberLike")]
    public async Task<IActionResult> IncreaseNumberLike(Guid idNews)
    {
        var idUserCurrent = Guid.Empty;
        var nameUserCurrent = "";

        var result = await _unitOfWork.News.UpdateNumberLike(idNews, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    #endregion

    #region Private method

    private static bool IsImageFile(IFormFile file)
    {
        var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
        return allowedContentTypes.Contains(file.ContentType);
    }

    private static void EnsureDirectoriesExist()
    {
        Directory.CreateDirectory(AppSettings.Root);
        Directory.CreateDirectory(AppSettings.ServerFileNews);
    }

    #endregion
}