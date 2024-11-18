namespace DPD.HR.Application.Queries.Queries;

public static class CategoryCitySqlQueries
{
    public const string QueryInsertCategoryCity = @"INSERT INTO [dbo].[CategoryCity]
                               ([Id]
                               ,[CityName]
                               ,[CityCode]
                               ,[Status]
                               ,[IsHide]
                               ,[CreateDate])
                         VALUES (@Id, @CityName, @CityCode, @Status,
                                @IsHide, @CreateDate)";

    public const string QueryUpdateCategoryCity = @"UPDATE [dbo].[CategoryCity] SET CityName = @CityName,
                                        CityCode = @CityCode
                                        WHERE Id = @Id";

    public const string QueryDeleteCategoryCity = "DELETE FROM [dbo].[CategoryCity] WHERE Id IN @Ids";
    public const string QueryGetByIdCategoryCity = "select * from [dbo].[CategoryCity] where Id = @Id";
    public const string QueryGetCategoryCityByIds = "select * from [dbo].[CategoryCity] where Id IN @Ids";
    public const string QueryGetAllCategoryCity = "select *from [dbo].[CategoryCity] order by CreateDate desc";

    public const string QueryGetAllCategoryCityAvailable =
        "select *from [dbo].[CategoryCity] where IsHide = 0 order by CreateDate desc";

    public const string QueryHideCategoryCity = "UPDATE [dbo].[CategoryCity] SET IsHide = @IsHide WHERE Id IN @Ids";

    public const string QueryInsertCategoryCityDeleted = @"INSERT INTO [dbo].[Deleted_CategoryCity]
                               ([Id]
                               ,[CityName]
                               ,[CityCode]
                               ,[Status]
                               ,[IsHide]
                               ,[CreateDate])
                         VALUES (@Id, @CityName, @CityCode, @Status,
                                @IsHide, @CreateDate)";
}