namespace DPD.HR.Application.Queries.Queries;

public class CategoryPositionSqlQueries
{
    public const string QueryInsertCategoryPosition = @"INSERT INTO [dbo].[CategoryPosition]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[PositionName]
           ,[Description]
           ,[IsActive])
                         VALUES (@Id, @CreatedDate, @Status, @PositionName, @Description, @IsActive)";

    public const string QueryUpdateCategoryPosition = @"UPDATE [dbo].[CategoryPosition] SET
                                        PositionName = @PositionName,
                                        Description = @Description
                                        WHERE Id = @Id";

    public const string QueryDeleteCategoryPosition = "DELETE FROM [dbo].[CategoryPosition] WHERE Id IN @Ids";
    public const string QueryGetByIdCategoryPosition = "select * from [dbo].[CategoryPosition] where Id = @Id";
    public const string QueryGetCategoryPositionByIds = "select * from [dbo].[CategoryPosition] where Id IN @Ids";
    public const string QueryGetAllCategoryPosition = "select *from [dbo].[CategoryPosition] order by CreatedDate desc";

    public const string QueryGetAllCategoryPositionAvailable =
        "select *from [dbo].[CategoryPosition] where IsActive = 1 order by CreatedDate desc";

    public const string QueryHideCategoryPosition =
        "UPDATE [dbo].[CategoryPosition] SET IsActive = @IsActive WHERE Id IN @Ids";

    public const string QueryInsertCategoryPositionDeleted = @"INSERT INTO [dbo].[Deleted_CategoryPosition]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[PositionName]
           ,[Description]
           ,[IsActive])
                         VALUES (@Id, @CreatedDate, @Status, @PositionName, @Description, @IsActive)";
}