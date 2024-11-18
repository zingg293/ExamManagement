
namespace DPD.HR.Application.Queries.Queries
{
    public class CategoryPolicybeneficiarySqlQueries
    {
        public const string QueryInsertCategoryPolicybeneficiary = @"INSERT INTO [dbo].[CategoryPolicybeneficiary]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[NamePolicybeneficiary])
     VALUES
           (
		   @Id,
           @CreatedDate,
           @Status,
           @NamePolicybeneficiary)";
       
        public const string QueryGetAllCategoryPolicybeneficiary = @"SELECT * FROM [Humans].[dbo].[CategoryPolicybeneficiary] order by [NamePolicybeneficiary]";
       
        public const string QueryGetByIdCategoryPolicybeneficiary = @"select * from [dbo].[CategoryPolicybeneficiary] where Id = @Id";
       
        public const string QueryUpdateCategoryPolicybeneficiary = @"UPDATE [dbo].[CategoryPolicybeneficiary] SET NamePolicybeneficiary = @NamePolicybeneficiary, Status = @Status WHERE Id = @Id";
       
        public const string QueryGetCategoryPolicybebeficiaryByIds = "select * from [dbo].[CategoryPolicybeneficiary] where Id IN @Ids";
       
        public const string QueryInsertCategoryPolicybeneficiaryDeleted = @"INSERT INTO [dbo].[Deleted_CategoryPolicybeneficiary]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[NamePolicybeneficiary])
     VALUES
           (
		   @Id,
           @CreatedDate,
           @Status,
           @NamePolicybeneficiary)";
       
        public const string QueryDeleteCategoryPolicybeneficiary = "DELETE FROM [dbo].[CategoryPolicybeneficiary] WHERE Id IN @Ids";
    }
}