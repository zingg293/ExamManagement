namespace DPD.HR.Application.Queries.Queries
{
    public static class CategoryGovernmentManagementSqlQueries
    {
        public const string QueryInsertCategoryGovernmentManagement =
            @"INSERT INTO [dbo].[CategoryGovernmentManagement]
                       ([Id]
                       ,[CreatedDate]
                       ,[Status]
                       ,[NameGovernmentManagement])
                 VALUES
                       (@Id
                       ,@CreatedDate
                       ,@Status
                       ,@NameGovernmentManagement)";
        public const string QueryUpdateCategoryGovernmentManagement =
                        @"
                        UPDATE [dbo].[CategoryGovernmentManagement]
                           SET [NameGovernmentManagement] = @NameGovernmentManagement
                                  WHERE Id = @Id";
        public const string QueryGetByIdCategoryGovernmentManagement = @"select * from [dbo].[CategoryGovernmentManagement] where Id = @Id";
        public const string QueryGetCategoryGovernmentManagementByIds = @"select * from [dbo].[CategoryGovernmentManagement] where Id IN @Ids";
        public const string QueryDeleteCategoryGovernmentManagement = @"Delete from [dbo].[CategoryGovernmentManagement] where Id IN @Ids";
        public const string QueryGetAllCategoryGovernmentManagement = @"SELECT * FROM [dbo].[CategoryGovernmentManagement] order by NameGovernmentManagement";
        public const string QueryGetAllCategoryGovernmentManagementAvailable = @"";
        public const string QueryHideCategoryGovernmentManagement = @"";
        public const string QueryInsertCategoryGovernmentManagementDeleted =
           @"INSERT INTO [dbo].[Deleted_CategoryGovernmentManagement]
           ([Id]
                       ,[CreatedDate]
                       ,[Status]
                       ,[NameGovernmentManagement])
                 VALUES
                       (@Id
                       ,@CreatedDate
                       ,@Status
                       ,@NameGovernmentManagement)";
    }
}
