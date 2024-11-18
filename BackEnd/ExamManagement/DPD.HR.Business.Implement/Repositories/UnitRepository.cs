using System.Data;
using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DPD.HR.Application.Implement.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        #region ===[ Private Members ]=============================================================

        private readonly IConfiguration _configuration;

        #endregion

        #region ===[ Constructor ]=================================================================

        public UnitRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region ===[ UnitRepository Methods ]==================================================

        public async Task<TemplateApi<UnitDto>> GetAllUnitByIdParent(Guid idUnitParent, int pageNumber, int pageSize)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var units = (await connection.QueryAsync<Unit>(UnitSqlQueries.GetUnitByParent,
                new { ParentId = idUnitParent })).ToList();

            return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<UnitDto>()),
                units.Count);
        }

        public async Task<UnitDto> GetUnitByUnitCode(string unitCode)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var unit = await connection.QueryFirstOrDefaultAsync<Unit>(UnitSqlQueries.GetUnitByUnitCode,
                new { UnitCode = unitCode });

            return unit.Adapt<UnitDto>();
        }

        public async Task<TemplateApi<UnitDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var units = (await connection.QueryAsync<Unit>(UnitSqlQueries.GetAllUnit)).ToList();

            return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<UnitDto>()),
                units.Count);
        }

        public async Task<TemplateApi<UnitDto>> GetById(Guid id)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var unit = await connection.QueryFirstOrDefaultAsync<Unit>(UnitSqlQueries.GetByIdUnit, new { Id = id });

            return new Pagination().HandleGetByIdRespond(unit.Adapt<UnitDto>());
        }

        public async Task<TemplateApi<UnitDto>> GetAllAvailable(int pageNumber, int pageSize)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var units = (await connection.QueryAsync<Unit>(UnitSqlQueries.GetAllUnitAvailable)).ToList();

            return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<UnitDto>()),
                units.Count);
        }

        public async Task<TemplateApi<UnitDto>> Update(UnitDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var unit = model.Adapt<Unit>();
                await connection.ExecuteAsync(UnitSqlQueries.UpdateUnit, unit, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã cập nhật bảng Unit",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Cập nhật CSDL",
                    Operation = "Update",
                    Table = "Unit",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();

                return new TemplateApi<UnitDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<UnitDto>> Insert(UnitDto model, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(UnitSqlQueries.InsertUnit, model, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã thêm mới bảng Unit",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Thêm mới CSDL",
                    Operation = "Create",
                    Table = "Unit",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();
                return new TemplateApi<UnitDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<UnitDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var unitsParentAndChild = (await connection.QueryAsync<Guid>(UnitSqlQueries.QueryUnitParentAndChill,
                    new { ID = ids }, tran)).ToList();

                var units =
                    (await connection.QueryAsync<Unit>(UnitSqlQueries.DeleteUnitByIds,
                        new { Ids = unitsParentAndChild },
                        tran))
                    .ToList();

                await connection.ExecuteAsync(UnitSqlQueries.InsertUnitDeleted, units, tran);

                await connection.ExecuteAsync(UnitSqlQueries.DeleteUnitByIds, new { Ids = unitsParentAndChild }, tran);

                var diaries = unitsParentAndChild.Select(id => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảng Unit",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "Unit",
                    IsSuccess = true,
                    WithId = id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

                tran.Commit();
                return new TemplateApi<UnitDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<UnitDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
            string fullName)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(UnitSqlQueries.HideUnit, new { IsHide = isLock, Ids = ids }, tran);

                var diaries = ids.Select(id => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã cập nhật bảng Unit",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Cập nhật CSDL",
                    Operation = "Update",
                    Table = "Unit",
                    IsSuccess = true,
                    WithId = id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

                tran.Commit();

                return new TemplateApi<UnitDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
        }

        #endregion
    }
}