using System.Data;
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

public class NavigationRepository : INavigationRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public NavigationRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ NavigationRepository ]=================================================================

    public async Task<TemplateApi<NavigationDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var navigations = (await connection.QueryAsync<Navigation>(NavigationSqlQueries.QueryGetAllNavigation))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            navigations.Select(u => u.Adapt<NavigationDto>()),
            navigations.Count);
    }

    public async Task<TemplateApi<NavigationDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<Navigation>(
            NavigationSqlQueries.QueryGetByIdNavigation, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<NavigationDto>());
    }

    public Task<TemplateApi<NavigationDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<NavigationDto>> Update(NavigationDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var navigation = model.Adapt<Navigation>();
            await connection.ExecuteAsync(NavigationSqlQueries.QueryUpdateNavigation, navigation, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng Navigation",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "Navigation",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<NavigationDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<NavigationDto>> Insert(NavigationDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(NavigationSqlQueries.QueryInsertNavigation, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng Navigation",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "Navigation",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<NavigationDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<NavigationDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var navigations =
                (await connection.QueryAsync<Navigation>(NavigationSqlQueries.QueryNavigationByIds, new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(NavigationSqlQueries.QueryInsertNavigationDeleted, navigations, tran);

            await connection.ExecuteAsync(NavigationSqlQueries.QueryDeleteNavigation, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng Navigation",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "Navigation",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<NavigationDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<NavigationDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(NavigationSqlQueries.QueryHideNavigation,
                new { IsHide = isLock, Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng Navigation",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "Navigation",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();

            return new TemplateApi<NavigationDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<NavigationAndChild>> GetNavigationByIdUser(Guid idUser, int pageNumber,
        int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        // Original code before modification
        var navigations = (await connection.QueryAsync<Navigation>(NavigationSqlQueries.QueryGetAllNavigationByIdUser,
                new { IdUser = idUser }))
            .ToList();

        var result = new List<NavigationAndChild>();

        var navigationsParent = navigations.Where(e => e.IdParent == null).ToList();

        foreach (var item in navigationsParent)
        {
            var navigationChildren = GetNavigationChildren(item, navigations);
            result.Add(new NavigationAndChild()
            {
                Navigation = item,
                NavigationsChild = navigationChildren
            });
        }

        // Recursive function to get the children with their children
        List<NavigationChildOfChildren> GetNavigationChildren(Navigation parent, List<Navigation> allNavigations)
        {
            var navigationChildren = new List<NavigationChildOfChildren>();
            var itemChild = allNavigations.Where(e => e.IdParent == parent.Id && !e.IsHide)
                .OrderBy(e => e.Sort).ToList();

            if (itemChild.Any())
            {
                foreach (var data in itemChild)
                {
                    var navChildOfChildren = data.Adapt<NavigationChildOfChildren>();
                    var itemChildOfChild = allNavigations.Where(e => e.IdParent == data.Id).ToList();

                    if (itemChildOfChild.Any())
                    {
                        navChildOfChildren.NavigationsChildOfChild = GetNavigationChildren(data, allNavigations);
                    }

                    navigationChildren.Add(navChildOfChildren);
                }
            }

            return navigationChildren;
        }

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            result.Select(u => u.Adapt<NavigationAndChild>()),
            result.Count);
    }

    #endregion
}