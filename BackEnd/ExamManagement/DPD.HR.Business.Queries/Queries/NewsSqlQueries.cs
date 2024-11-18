namespace DPD.HR.Application.Queries.Queries;

public static class NewsSqlQueries
{
    public const string QueryInsertNews = @"INSERT INTO [dbo].[News]
           ([Id]
           ,[Title]
           ,[Description]
           ,[IsHide]
           ,[IsDeleted]
           ,[IsApproved]
           ,[UserCreated]
           ,[UserUpdated]
           ,[Status]
           ,[CreatedDate]
           ,[CreatedDateDisplay]
           ,[UpdateDate]
           ,[NewsContent]
           ,[NewContentDraft]
           ,[Author]
           ,[NewsLike]
           ,[NewsView]
           ,[Avatar]
           ,[ExtensionFile]
           ,[FilePath]
           ,[IdCategoryNews])
            VALUES (@Id, @Title, @Description, @IsHide, @IsDeleted, @IsApproved, @UserCreated, @UserUpdated, @Status, @CreatedDate,
                  @CreatedDateDisplay, @UpdateDate, @NewsContent, @NewContentDraft, @Author, @NewsLike, @NewsView, @Avatar,
                  @ExtensionFile, @FilePath, @IdCategoryNews)";

    public const string QueryUpdateNews = @"UPDATE [dbo].[News] SET 
                            Title = @Title,
                            Description = @Description,
                            UserUpdated = @UserUpdated,
                            CreatedDateDisplay = @CreatedDateDisplay,
                            UpdateDate = @UpdateDate,
                            NewsContent = @NewsContent,
                            NewContentDraft = @NewContentDraft,
                            Author = @Author,
                            Avatar = @Avatar,
                            ExtensionFile = @ExtensionFile,
                            FilePath = @FilePath,
                            IdCategoryNews = @IdCategoryNews
                            WHERE Id = @Id";
    
    public const string QueryUpdateHideNews = @"UPDATE [dbo].[News] SET 
                            IsHide = @IsHide
                            WHERE Id IN @Ids";

    public const string QueryUpdateApprovedNews = @"UPDATE [dbo].[News] SET 
                            IsApproved = @IsApproved
                            WHERE Id IN @Ids";
    
    public const string QueryUpdateNumberViewNews = @"UPDATE [dbo].[News] SET 
                            NewsView = @NewsView
                            WHERE Id = @Id";
    
    public const string QueryUpdateNumberLikeNews = @"UPDATE [dbo].[News] SET 
                            NewsLike = @NewsLike
                            WHERE Id = @Id";

    public const string QueryDeleteNews = "DELETE FROM [dbo].[News] WHERE Id IN @Ids";
    public const string QueryGetByIdNews = "select * from [dbo].[News] where Id = @Id";
    public const string QueryGetNewsByIds = "select * from [dbo].[News] where Id IN @Ids";
    public const string QueryGetAllNews = "select *from [dbo].[News] order by CreatedDate desc";
    public const string QueryGetAllNewsByIdCategoryNews = "select *from [dbo].[News] where IdCategoryNews = @IdCategoryNews and IsHide = @IsHide and IsDeleted = @IsDeleted and IsApproved = @IsApproved order by CreatedDate desc";
    public const string QueryGetAllNewsAvailable = "select *from [dbo].[News] where IsHide = @IsHide and IsDeleted = @IsDeleted and IsApproved = @IsApproved order by CreatedDate desc";
    public const string QueryGetNewsByKeyWord = @"SELECT *
                                                FROM News
                                                WHERE Title LIKE @KeyWord
                                                OR NewsContent LIKE @KeyWord
                                                order by CreatedDate desc";

    public const string QueryInsertNewsDeleted = @"INSERT INTO [dbo].[Deleted_News]
           ([Id]
           ,[Title]
           ,[Description]
           ,[IsHide]
           ,[IsDeleted]
           ,[IsApproved]
           ,[UserCreated]
           ,[UserUpdated]
           ,[Status]
           ,[CreatedDate]
           ,[CreatedDateDisplay]
           ,[UpdateDate]
           ,[NewsContent]
           ,[NewContentDraft]
           ,[Author]
           ,[NewsLike]
           ,[NewsView]
           ,[Avatar]
           ,[ExtensionFile]
           ,[FilePath]
           ,[IdCategoryNews])
            VALUES (@Id, @Title, @Description, @IsHide, @IsDeleted, @IsApproved, @UserCreated, @UserUpdated, @Status, @CreatedDate,
                  @CreatedDateDisplay, @UpdateDate, @NewsContent, @NewContentDraft, @Author, @NewsLike, @NewsView, @Avatar,
                  @ExtensionFile, @FilePath, @IdCategoryNews)";
}