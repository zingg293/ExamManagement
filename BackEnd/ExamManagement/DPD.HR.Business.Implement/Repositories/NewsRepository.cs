using System.Data;
using System.Globalization;
using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DPD.HR.Application.Implement.Repositories;

public class NewsRepository : INewsRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public NewsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ NewsRepository Methods ]==================================================

    public async Task<TemplateApi<NewsDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<News>(NewsSqlQueries.QueryGetAllNews))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<NewsDto>()),
            units.Count);
    }

    public async Task<TemplateApi<NewsDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<News>(
            NewsSqlQueries.QueryGetByIdNews, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<NewsDto>());
    }

    public async Task<TemplateApi<NewsDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<News>(NewsSqlQueries.QueryGetAllNewsAvailable,
                new { IsHide = false, IsDeleted = false, IsApproved = true }))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<NewsDto>()),
            units.Count);
    }

    public async Task<TemplateApi<NewsDto>> Update(NewsDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var newsById = await connection.QueryFirstOrDefaultAsync<News>(
                NewsSqlQueries.QueryGetByIdNews, new { Id = model.Id }, tran);

            var news = model.Adapt<News>();
            if (model.IdFile is not null)
            {
                news.Avatar = newsById.Avatar;
                news.ExtensionFile = newsById.ExtensionFile;
                news.FilePath = newsById.FilePath;
            }

            await connection.ExecuteAsync(NewsSqlQueries.QueryUpdateNews, news, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng News",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "News",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<NewsDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<NewsDto>> Insert(NewsDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(NewsSqlQueries.QueryInsertNews, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng News",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "News",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<NewsDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<NewsDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<News>(NewsSqlQueries.QueryGetNewsByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(NewsSqlQueries.QueryInsertNewsDeleted, categoryCities, tran);

            await connection.ExecuteAsync(NewsSqlQueries.QueryDeleteNews, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng News",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "News",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<NewsDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<NewsDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(NewsSqlQueries.QueryUpdateHideNews,
                new { IsHide = isLock, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng News",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "News",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<NewsDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<NewsDto>> ApproveNews(List<Guid> ids, bool isApprove, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(NewsSqlQueries.QueryUpdateApprovedNews,
                new { IsApproved = isApprove, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng News",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "News",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<NewsDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<NewsDto>> UpdateNumberView(Guid idNews, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var newsById = await connection.QueryFirstOrDefaultAsync<News>(
                NewsSqlQueries.QueryGetByIdNews, new { Id = idNews }, tran);

            await connection.ExecuteAsync(NewsSqlQueries.QueryUpdateNumberViewNews,
                new { NewsView = newsById.NewsView + 1, Id = idNews }, tran);

            var diaries = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng News",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "News",
                IsSuccess = true,
                WithId = idNews
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<NewsDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<NewsDto>> UpdateNumberLike(Guid idNews, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var newsById = await connection.QueryFirstOrDefaultAsync<News>(
                NewsSqlQueries.QueryGetByIdNews, new { Id = idNews }, tran);

            await connection.ExecuteAsync(NewsSqlQueries.QueryUpdateNumberLikeNews,
                new { NewsLike = newsById.NewsLike + 1, Id = idNews }, tran);

            var diaries = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng News",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "News",
                IsSuccess = true,
                WithId = idNews
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<NewsDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<NewsDto>> GetAllByIdCategoryNews(Guid idCategoryNews, int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<News>(NewsSqlQueries.QueryGetAllNewsByIdCategoryNews,
                new { IsHide = false, IsDeleted = false, IsApproved = true, IdCategoryNews = idCategoryNews }))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<NewsDto>()),
            units.Count);
    }

    public async Task<TemplateApi<NewsDto>> SearchNews(string filter, int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<News>(NewsSqlQueries.QueryGetNewsByKeyWord,
                new { KeyWord = $"%{filter}%" }))
            .ToList();

        units = units.Where(e => e.IsHide == false && e.IsDeleted == false && e.IsApproved == true).ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<NewsDto>()),
            units.Count);
    }

    public async Task<TemplateApi<NewsDto>> FilterSearchNews(FilterSearchNewsModel model, int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var news = (await connection.QueryAsync<News>(NewsSqlQueries.QueryGetAllNews))
            .ToList();

        if (model.Title is not null)
        {
            news = news.Where(e => e.Title == model.Title).ToList();
        }

        if (model.IdCategoryNews is not null)
        {
            news = news.Where(e => e.IdCategoryNews == model.IdCategoryNews).ToList();
        }
        if (model.CreatedDateDisplay is not null)
        {
            DateTime date = DateTime.ParseExact(model.CreatedDateDisplay, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            news = news.Where(e => e.CreatedDateDisplay != null &&
                                   e.CreatedDateDisplay.Value.Day == date.Day &&
                                   e.CreatedDateDisplay.Value.Month == date.Month &&
                                   e.CreatedDateDisplay.Value.Year == date.Year
            ).ToList();
        }
        if (model.IsHide is not null)
        {
            news = news.Where(e => e.IsHide == model.IsHide).ToList();
        }
        if (model.IsApproved is not null)
        {
            news = news.Where(e => e.IsApproved == model.IsApproved).ToList();
        }

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, news.Select(u => u.Adapt<NewsDto>()),
            news.Count);
    }

    #endregion
}