namespace DPD.HR.Application.Queries.Queries
{
    public static class CategorySalaryScaleSqlQueries
    {
        public const string QueryInsertCategorySalaryScale = @"
INSERT INTO [dbo].[CategorySalaryScale]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[NameSalaryScale]
           ,[Code])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @NameSalaryScale,
           @Code)";
        public const string QueryUpdateCategorySalaryScale =
@"
UPDATE [dbo].[CategorySalaryScale]
   SET
      [CreatedDate] = @CreatedDate,
      [Status] = @Status,
      [NameSalaryScale] = @NameSalaryScale,
      [Code] = @Code
          WHERE Id = @Id";
        public const string QueryGetByIdCategorySalaryScale = @"select * from [dbo].[CategorySalaryScale] where Id = @Id";
        public const string QueryGetCategorySalaryScaleByIds = @"select * from [dbo].[CategorySalaryScale] where Id IN @Ids";
        public const string QueryDeleteCategorySalaryScale = @"Delete from [dbo].[CategorySalaryScale] where Id IN @Ids";
        public const string QueryGetAllCategorySalaryScale = @"SELECT * FROM CategorySalaryScale order by NameSalaryScale";
        public const string QueryHideCategorySalaryScale = @"";
        public const string QueryGetAllCategorySalaryScaleAvailable = @"";
        public const string QueryInsertCategorySalaryScaleDeleted = @"INSERT INTO [dbo].[Deleted_CategorySalaryScale]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[NameSalaryScale]
           ,[Code])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @NameSalaryScale,
           @Code)";
    }
}
