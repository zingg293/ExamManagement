using Dapper;
using DPD.HR.Application.Queries.Queries;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace DPD.HR.Application.Implement.Repositories
{
    public class CategoryPolicybeneficiaryRepository : ICategoryPolicybeneficiaryRepository
    {
        #region ===[ Private Members ]=============================================================

        private readonly IConfiguration _configuration;

        #endregion

        #region ===[ Constructor ]=================================================================

        public CategoryPolicybeneficiaryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region ===[ CategoryPolicybeneficiaryRepository Methods ]==================================================
        public async Task<TemplateApi<CategoryPolicybeneficiaryDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var units = (await connection.QueryAsync<CategoryPolicybeneficiaryDto>(CategoryPolicybeneficiarySqlQueries.QueryGetAllCategoryPolicybeneficiary))
                .ToList();
            
            return new Pagination().HandleGetAllRespond(pageNumber, pageSize, units.Select(u => u.Adapt<CategoryPolicybeneficiaryDto>()),
                units.Count);
        }

        public async Task<TemplateApi<CategoryPolicybeneficiaryDto>> GetById(Guid id)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();

            var unit = await connection.QueryFirstOrDefaultAsync<CategoryPolicybeneficiaryDto>(
                CategoryPolicybeneficiarySqlQueries.QueryGetByIdCategoryPolicybeneficiary, new { Id = id });

            return new Pagination().HandleGetByIdRespond(unit.Adapt<CategoryPolicybeneficiaryDto>());
        }

        Task<TemplateApi<CategoryPolicybeneficiaryDto>> IRepository<CategoryPolicybeneficiaryDto>.GetAllAvailable(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<TemplateApi<CategoryPolicybeneficiaryDto>> Update(CategoryPolicybeneficiaryDto model, Guid idUserCurrent, string fullName)
        {

            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var categoryCity = model.Adapt<CategoryPolicybeneficiaryDto>();
                await connection.ExecuteAsync(CategoryPolicybeneficiarySqlQueries.QueryUpdateCategoryPolicybeneficiary, categoryCity, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã cập nhật bảng CategoryPolicybeneficiary",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Cập nhật CSDL",
                    Operation = "Update",
                    Table = "CategoryPolicybeneficiary",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();

                return new TemplateApi<CategoryPolicybeneficiaryDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        public async Task<TemplateApi<CategoryPolicybeneficiaryDto>> Insert(CategoryPolicybeneficiaryDto model, Guid idUserCurrent, string fullName)
        {

            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(CategoryPolicybeneficiarySqlQueries.QueryInsertCategoryPolicybeneficiary, model, tran);

                var diary = new Diary()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã thêm mới bảng CategoryPolicybeneficiary",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Thêm mới CSDL",
                    Operation = "Create",
                    Table = "CategoryPolicybeneficiary",
                    IsSuccess = true,
                    WithId = model.Id
                };

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

                tran.Commit();
                return new TemplateApi<CategoryPolicybeneficiaryDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

      public async  Task<TemplateApi<CategoryPolicybeneficiaryDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
        {

            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                var categoryPolicybeneficiary =
                    (await connection.QueryAsync<CategoryPolicybeneficiaryDto>(CategoryPolicybeneficiarySqlQueries.QueryGetCategoryPolicybebeficiaryByIds, new { Ids = ids },
                        tran))
                    .ToList();

                await connection.ExecuteAsync(CategoryPolicybeneficiarySqlQueries.QueryInsertCategoryPolicybeneficiaryDeleted, categoryPolicybeneficiary, tran);

                await connection.ExecuteAsync(CategoryPolicybeneficiarySqlQueries.QueryDeleteCategoryPolicybeneficiary, new { Ids = ids }, tran);

                var diaries = ids.Select(id => new Diary
                {
                    Id = Guid.NewGuid(),
                    Content = $"{fullName} đã xóa từ bảng CategoryPolicybeneficiary",
                    UserId = idUserCurrent,
                    UserName = fullName,
                    DateCreate = DateTime.Now,
                    Title = "Xóa từ CSDL",
                    Operation = "Delete",
                    Table = "CategoryPolicybeneficiary",
                    IsSuccess = true,
                    WithId = id
                }).ToList();

                await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

                tran.Commit();
                return new TemplateApi<CategoryPolicybeneficiaryDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                // roll the transaction back
                tran.Rollback();
                throw;
            }
        }

        Task<TemplateApi<CategoryPolicybeneficiaryDto>> IRepository<CategoryPolicybeneficiaryDto>.HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
