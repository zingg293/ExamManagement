namespace DPD.HR.Application.Queries.Queries;

public static class CategoryNewsSqlQueries
{
    public const string QueryInsertCategoryNews = @"INSERT INTO [dbo].[CategoryNews]
           ([Id]
           ,[NameCategory]
           ,[CategoryGroup]
           ,[ParentId]
           ,[IsHide]
           ,[IsDeleted]
           ,[UserCreated]
           ,[ShowChild]
           ,[Sort]
           ,[CreatedDate]
           ,[Status]
            ,[Avatar])
            VALUES (@Id, @NameCategory, @CategoryGroup, @ParentId, @IsHide, @IsDeleted, @UserCreated, @ShowChild, @Sort,
                          @CreatedDate, @Status, @Avatar)";

    public const string QueryUpdateCategoryNews = @"UPDATE [dbo].[CategoryNews] SET 
                            NameCategory = @NameCategory,
                            CategoryGroup = @CategoryGroup,
                            ParentId = @ParentId,
                            UserCreated = @UserCreated,
                            Sort = @Sort
                            WHERE Id = @Id";
    
    public const string QueryUpdateHideCategoryNews = @"UPDATE [dbo].[CategoryNews] SET 
                            IsHide = @IsHide
                            WHERE Id IN @Ids";
    
    public const string QueryUpdateShowChildCategoryNews = @"UPDATE [dbo].[CategoryNews] SET 
                            ShowChild = @ShowChild
                            WHERE Id IN @Ids";

    public const string QueryDeleteCategoryNews = "DELETE FROM [dbo].[CategoryNews] WHERE Id IN @Ids";
    public const string QueryGetByIdCategoryNews = "select * from [dbo].[CategoryNews] where Id = @Id";
    public const string QueryGetCategoryNewsByIds = "select * from [dbo].[CategoryNews] where Id IN @Ids";
    public const string QueryGetAllCategoryNews = "select *from [dbo].[CategoryNews] order by CreatedDate desc";
    public const string QueryGetAllCategoryNewsAvailable = "select *from [dbo].[CategoryNews] where IsHide = @IsHide and IsDeleted = @IsDeleted order by CreatedDate desc";
    public const string QueryGetAllCategoryNewsByParentId = "select *from [dbo].[CategoryNews] where ParentId = @ParentId order by CreatedDate desc";


    public const string QueryInsertCategoryNewsDeleted = @"INSERT INTO [dbo].[Deleted_CategoryNews]
           ([Id]
           ,[NameCategory]
           ,[CategoryGroup]
           ,[ParentId]
           ,[IsHide]
           ,[IsDeleted]
           ,[UserCreated]
           ,[ShowChild]
           ,[Sort]
           ,[CreatedDate]
           ,[Status]
            ,[Avatar])
            VALUES (@Id, @NameCategory, @CategoryGroup, @ParentId, @IsHide, @IsDeleted, @UserCreated, @ShowChild, @Sort,
                          @CreatedDate, @Status, @Avatar)";
}