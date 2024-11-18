namespace DPD.HR.Application.Queries.Queries;

public static class UserTypeSqlQueries
{
    public const string GetAllUserType = "select * from UserType order by CreatedDate desc";
    public const string GetAllUserTypeByTypeCode = "select * from [dbo].[UserType] where TypeCode = @TypeCode";
    public const string GetUserTypeById = "select * from [dbo].[UserType] where Id = @Id";
    public const string GetUserTypeByIds = "select * from [dbo].[UserType] where Id IN @Ids";
    
    public const string QueryInsertUserType = @"INSERT INTO [dbo].[UserType]
                                           ([Id]
                                           ,[TypeName]
                                           ,[Status]
                                           ,[CreateBy]
                                           ,[CreatedDate]
                                           ,[TypeCode])
                         VALUES (@Id, @TypeName, @Status, @CreateBy, @CreatedDate, @TypeCode)";
    
    public const string QueryInsertUserTypeDelete = @"INSERT INTO [dbo].[Deleted_UserType]
                                           ([Id]
                                           ,[TypeName]
                                           ,[Status]
                                           ,[CreateBy]
                                           ,[CreatedDate]
                                           ,[TypeCode])
                         VALUES (@Id, @TypeName, @Status, @CreateBy, @CreatedDate, @TypeCode)";

    public const string QueryUpdateUserType = @"UPDATE [dbo].[UserType] SET 
                                        TypeName = @TypeName
                                        WHERE Id = @Id";

    public const string QueryDeleteUserType = "DELETE FROM [dbo].[UserType] WHERE Id IN @Ids";
}