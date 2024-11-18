namespace DPD.HR.Application.Queries.Queries;

public static class EmployeeTypeSqlQueries
{
    public const string QueryInsertEmployeeType = @"INSERT INTO [dbo].[EmployeeType]
                               ([Id]
                               ,[TypeName]
                               ,[IsActive]
                               ,[Status]
                               ,[CreatedDate])
                         VALUES (@Id, @TypeName, @IsActive, @Status,
                                @CreatedDate)";

    public const string QueryUpdateEmployeeType = @"UPDATE [dbo].[EmployeeType] SET TypeName = @TypeName
                                        WHERE Id = @Id";

    public const string QueryDeleteEmployeeType = "DELETE FROM [dbo].[EmployeeType] WHERE Id IN @Ids";
    public const string QueryGetByIdEmployeeType = "select * from [dbo].[EmployeeType] where Id = @Id";
    public const string QueryEmployeeTypeByIds = "select * from [dbo].[EmployeeType] where Id IN @Ids";
    public const string QueryGetAllEmployeeType = "select *from [dbo].[EmployeeType] order by CreatedDate desc";

    public const string QueryGetAllEmployeeTypeAvailable =
        "select *from [dbo].[EmployeeType] where IsActive = 1 order by CreatedDate desc";

    public const string QueryHideEmployeeType = "UPDATE [dbo].[EmployeeType] SET IsActive = @IsActive WHERE Id IN @Ids";

    public const string QueryInsertEmployeeTypeDeleted = @"INSERT INTO [dbo].[Deleted_EmployeeType]
                               ([Id]
                               ,[TypeName]
                               ,[IsActive]
                               ,[Status]
                               ,[CreatedDate])
                         VALUES (@Id, @TypeName, @IsActive, @Status,
                                @CreatedDate)";
}