namespace DPD.HR.Application.Queries.Queries;

public static class CategoryDistrictSqlQueries
{
    public const string QueryInsertCategoryDistrict = @"INSERT INTO [dbo].[CategoryDistrict]
                                                                   ([Id]
                                                                   ,[DistrictName]
                                                                   ,[DistrictCode]
                                                                   ,[CityCode]
                                                                   ,[Status]
                                                                   ,[IsHide]
                                                                   ,[CreatedDate])
                                                             VALUES (@Id, @DistrictName, @DistrictCode, @CityCode,
                                                                    @Status, @IsHide, @CreatedDate)";

    public const string QueryUpdateCategoryDistrict =
        @"UPDATE [dbo].[CategoryDistrict] SET DistrictName = @DistrictName,
                                        DistrictCode = @DistrictCode,
                                        CityCode = @CityCode
                                        WHERE Id = @Id";

    public const string QueryDeleteCategoryDistrict = "DELETE FROM [dbo].[CategoryDistrict] WHERE Id IN @Ids";
    public const string QueryGetByIdCategoryDistrict = "select * from [dbo].[CategoryDistrict] where Id = @Id";
    public const string QueryCategoryDistrictByIds = "select * from [dbo].[CategoryDistrict] where Id IN @Ids";
    public const string QueryCategoryDistrictByCityCode = "select * from [dbo].[CategoryDistrict] where CityCode = @CityCode";

    public const string QueryGetAllCategoryDistrict = "select *from [dbo].[CategoryDistrict] order by CreatedDate desc";

    public const string QueryGetAllCategoryDistrictAvailable =
        "select *from [dbo].[CategoryDistrict] WHERE IsHide = 0 order by CreatedDate desc";

    public const string QueryHideCategoryDistrict =
        "UPDATE [dbo].[CategoryDistrict] SET IsHide = @IsHide WHERE Id IN @Ids";

    public const string QueryInsertCategoryDistrictDeleted = @"INSERT INTO [dbo].[Deleted_CategoryDistrict]
                                                                   ([Id]
                                                                   ,[DistrictName]
                                                                   ,[DistrictCode]
                                                                   ,[CityCode]
                                                                   ,[Status]
                                                                   ,[IsHide]
                                                                   ,[CreatedDate])
                                                             VALUES (@Id, @DistrictName, @DistrictCode, @CityCode,
                                                                    @Status, @IsHide, @CreatedDate)";

    public const string QueryGetAllIdCategoryDistrict = "select Id from [dbo].[CategoryDistrict]";

    public const string QueryGetAllCategoryDistrictByIdCity =
        "select *from [dbo].[CategoryDistrict] where CityCode = @CityCode order by CreatedDate desc";
}