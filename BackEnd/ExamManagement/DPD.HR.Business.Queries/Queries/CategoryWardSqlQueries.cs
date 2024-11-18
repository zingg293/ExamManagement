namespace DPD.HR.Application.Queries.Queries;

public static class CategoryWardSqlQueries
{
    public const string QueryInsertCategoryWard = @"INSERT INTO [dbo].[CategoryWard]
                               ([Id]
                               ,[WardName]
                               ,[WardCode]
                               ,[DistrictCode]
                               ,[Status]
                               ,[IsHide]
                               ,[CreatedDate])
                         VALUES (@Id, @WardName, @WardCode, @DistrictCode,
                                @Status, @IsHide,@CreatedDate)";

    public const string QueryUpdateCategoryWard = @"UPDATE [dbo].[CategoryWard] SET WardName = @WardName,
                                        WardCode = @WardCode,
                                        DistrictCode = @DistrictCode
                                        WHERE Id = @Id";

    public const string QueryDeleteCategoryWard = "DELETE FROM [dbo].[CategoryWard] WHERE Id = @Ids";
    public const string QueryGetByIdCategoryWard = "select * from [dbo].[CategoryWard] where Id = @Id";
    public const string QueryCategoryWardByIds = "select * from [dbo].[CategoryWard] where Id IN @Ids";
    public const string QueryCategoryWardByDistrictCode = "select * from [dbo].[CategoryWard] where DistrictCode = @DistrictCode";
    public const string QueryGetAllCategoryWard = "select *from [dbo].[CategoryWard] order by CreatedDate desc";
    public const string QueryGetAllCategoryWardAvailable = "select *from [dbo].[CategoryWard] where IsHide = 0 order by CreatedDate desc";
    public const string QueryHideCategoryWard = "UPDATE [dbo].[CategoryWard] SET IsHide = @IsHide WHERE Id = @Ids";

    public const string QueryInsertCategoryWardDeleted = @"INSERT INTO [dbo].[Deleted_CategoryWard]
                               ([Id]
                               ,[WardName]
                               ,[WardCode]
                               ,[DistrictCode]
                               ,[Status]
                               ,[IsHide]
                               ,[CreatedDate])
                         VALUES (@Id, @WardName, @WardCode, @DistrictCode,
                                @Status, @IsHide,@CreatedDate)";

    public const string QueryGetAllIdCategoryWard = "select Id from [dbo].[CategoryWard]";
}