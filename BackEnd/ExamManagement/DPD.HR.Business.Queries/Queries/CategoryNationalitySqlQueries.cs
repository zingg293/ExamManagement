using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DPD.HR.Application.Queries.Queries
{
    public class CategoryNationalitySqlQueries
    {
       public const string QueryInsertCategoryNationality = @"INSERT INTO [dbo].[CategoryNationality]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[NameNationality])
     VALUES
           (
		   @Id,
           @CreatedDate,
           @Status,
           @NameNationality)";      
       public const string QueryGetAllCategoryNationality = @"select *from [dbo].[CategoryNationality] order by NameNationality";
       public const string QueryGetByIdCategoryNationality = @"select * from [dbo].[CategoryNationality] where Id = @Id";
       public const string QueryUpdateCategoryNationality = @"UPDATE [dbo].[CategoryNationality] SET NameNationality = @NameNationality, Status = @Status WHERE Id = @Id";
       public const string QueryGetCategoryNationalityByIds = "select * from [dbo].[CategoryNationality] where Id IN @Ids";
       public const string QueryInsertCategoryNationalityDeleted = @"INSERT INTO [dbo].[Deleted_CategoryNationality]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[NameNationality])
     VALUES
       (@Id,@CreatedDate,@Status,@NameNationality)";
       public const string QueryDeleteCategoryNationality = "DELETE FROM [dbo].[CategoryNationality] WHERE Id IN @Ids";


    }
}
