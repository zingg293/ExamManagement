namespace DPD.HR.Application.Queries.Queries
{
    public static class CategoryTypeSalaryScaleSqlQueries
    {
        public const string QueryInsertCategoryTypeSalaryScale = @"INSERT INTO [dbo].[CategoryTypeSalaryScale]
                                                                               ([Id]
                                                                               ,[CreatedDate]
                                                                               ,[Status]
                                                                               ,[NameTypeSalaryScale])
                                                                         VALUES
                                                                               (@Id,
                                                                               @CreatedDate,
                                                                               @Status,
                                                                               @NameTypeSalaryScale)";
        public const string QueryUpdateCategoryTypeSalaryScale =
                                                             @"UPDATE [dbo].[CategoryTypeSalaryScale]
                                                               SET[CreatedDate] = @CreatedDate,
                                                                  [Status] = @Status,
                                                                  [NameTypeSalaryScale] = @NameTypeSalaryScale
                                                                      WHERE Id = @Id";
        public const string QueryGetByIdCategoryTypeSalaryScale = @"select * from [dbo].[CategoryTypeSalaryScale] where Id = @Id";
        public const string QueryGetCategoryTypeSalaryScaleByIds = @"select * from [dbo].[CategoryTypeSalaryScale] where Id IN @Ids";
        public const string QueryDeleteCategoryTypeSalaryScale = @"Delete from [dbo].[CategoryTypeSalaryScale] where Id IN @Ids";
        public const string QueryInsertCategoryTypeSalaryScaleDeleted = @"INSERT INTO [dbo].[Deleted_CategoryTypeSalaryScale]
           ([Id]
                                                                               ,[CreatedDate]
                                                                               ,[Status]
                                                                               ,[NameTypeSalaryScale])
                                                                         VALUES
                                                                               (@Id,
                                                                               @CreatedDate,
                                                                               @Status,
                                                                               @NameTypeSalaryScale)";
        public const string QueryGetAllCategoryTypeSalaryScale = @"SELECT * FROM [dbo].[CategoryTypeSalaryScale] ORDER BY NameTypeSalaryScale";
        public const string QueryGetAllCategoryTypeSalaryScaleAvailable = @"";
        public const string QueryHideCategoryTypeSalaryScale = @"";
    }
}
