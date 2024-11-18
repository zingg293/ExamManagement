namespace DPD.HR.Application.Queries.Queries;

public static class RoleSqlQueries
{
    public const string GetAllRole = "select * from [dbo].[Role] where IsDeleted = 0";
    public const string GetByIdRole = "SELECT * FROM Role WHERE Id = @Id";
    public const string GetByRoleCode = "SELECT * FROM Role WHERE RoleCode = @RoleCode";

    public const string GetRoleByIdUser = @"
                        select r.*
                        from [dbo].[User] u
                                 join [dbo].[UserRole] ur on ur.IdUser = u.Id
                                 join [dbo].[Role] r on r.Id = ur.IdRole
                        where u.Id = @IdUser";

    public const string GetAllInformRoleOfUser =
        "SELECT r.Id 'IdRole', r.NumberRole, r.RoleName 'NameRole', u.Id 'IdUser', IIF(r.RoleCode = 'AD', 1, 0) 'IsAdmin'  \n" +
        "FROM [dbo].[User] u left join [dbo].[UserRole] ur on u.Id = ur.IdUser left join [dbo].[Role] r on r.Id = ur.IdRole where u.Id = @Id";

    public const string QueryInsertRole = @"INSERT INTO [dbo].[Role]
                                   ([Id]
                                   ,[RoleName]
                                   ,[Status]
                                   ,[IsDeleted]
                                   ,[IsAdmin]
                                   ,[NumberRole]
                                   ,[RoleCode])
                         VALUES (@Id, @RoleName, @Status, @IsDeleted, @IsAdmin, @NumberRole, @RoleCode)";

    public const string QueryUpdateRole = @"UPDATE [dbo].[Role] SET 
                                        RoleName = @RoleName,
                                        NumberRole = @NumberRole
                                        WHERE Id = @Id";

    public const string QueryDeleteRole = "DELETE FROM [dbo].[Role] WHERE Id IN @Ids";
}