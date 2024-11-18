using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class CategoryEducationalDegreeSqlQueries
    {
        public const string QueryInsertCategoryEducationalDegree = @"
     INSERT INTO [dbo].[CategoryEducationalDegree]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[NameEducationalDegree])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @NameEducationalDegree)";
        public const string QueryUpdateCategoryEducationalDegree =
        @"UPDATE [dbo].[CategoryEducationalDegree]
        SET
          [CreatedDate] = @CreatedDate,
          [Status] = @Status,
          [NameEducationalDegree] = @NameEducationalDegree
        WHERE Id = @Id";
        public const string QueryGetByIdCategoryEducationalDegree = @"select * from [dbo].[CategoryEducationalDegree] where Id = @Id";
        public const string QueryGetCategoryEducationalDegreeByIds = @"select * from [dbo].[CategoryEducationalDegree] where Id IN @Ids";
        public const string QueryDeleteCategoryEducationalDegree = @"Delete from [dbo].[CategoryEducationalDegree] where Id IN @Ids";
        public const string QueryGetAllCategoryEducationalDegree = @"SELECT * FROM CategoryEducationalDegree order by CreatedDate";
        public const string QueryGetAllCategoryEducationalDegreeAvailable = @"";
        public const string QueryHideCategoryEducationalDegree = @"";
        public const string QueryInsertCategoryEducationalDegreeDeleted =
                    @"
                    INSERT INTO[dbo].[Deleted_CategoryEducationalDegree]
                            ([Id]
                               ,[CreatedDate]
                               ,[Status]
                               ,[NameEducationalDegree])
                         VALUES
                               (@Id,
                               @CreatedDate,
                               @Status,
                               @NameEducationalDegree)";
    }
}
