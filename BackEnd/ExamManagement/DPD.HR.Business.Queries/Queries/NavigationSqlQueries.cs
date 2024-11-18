namespace DPD.HR.Application.Queries.Queries;

public static class NavigationSqlQueries
{
    public const string QueryInsertNavigation = @"INSERT INTO [dbo].[Navigation]
                               ([Id]
                               ,[MenuName]
                               ,[IdParent]
                               ,[Status]
                               ,[CreatedDate]
                               ,[Path]
                               ,[IconLink]
                               ,[MenuCode]
                               ,[Sort])
                         VALUES (@Id, @MenuName, @IdParent, @Status, @CreatedDate, @Path, @IconLink, @MenuCode, @Sort)";

    public const string QueryUpdateNavigation = @"UPDATE [dbo].[Navigation] SET MenuName = @MenuName,
                                        IdParent = @IdParent,
                                        Path = @Path,
                                        IconLink = @IconLink,
                                        MenuCode = @MenuCode,
                                        Sort = @Sort
                                        WHERE Id = @Id";

    public const string QueryDeleteNavigation = "DELETE FROM [dbo].[Navigation] WHERE Id IN @Ids";
    public const string QueryGetByIdNavigation = "select * from [dbo].[Navigation] where Id = @Id";
    public const string QueryNavigationByIds = "select * from [dbo].[Navigation] where Id IN @Ids";
    public const string QueryGetAllNavigation = "select *from [dbo].[Navigation] order by CreatedDate desc";

    public const string QueryGetAllNavigationByIdRole = @"select n.*
                                                    from [dbo].[Role] r
                                                        join[dbo].[NavigationRole]
                                                    nr on nr.IdRole = r.Id
                                                        right join [dbo].[Navigation]
                                                    n on n.Id = nr.IdNavigation
                                                        where r.Id = @IdRole";

    public const string QueryGetAllNavigationByIdUser = @"select distinct n.*
        from [dbo].[NavigationRole] nr
                 join [dbo].[Navigation] n on nr.IdNavigation = n.Id
        where nr.IdRole IN (select r.Id
                            from [dbo].[User] u
                                     join [dbo].[UserRole] ur on ur.IdUser = u.Id
                                     join [dbo].[Role] r on r.Id = ur.IdRole
                            where u.Id = @IdUser)";
    //
    // // public const string QueryGetAllChildNavigation = @"
    // //         WITH rel AS (SELECT *
    // //                      FROM Navigation
    // //                      WHERE Id = @IdNavigation
    // //                      UNION ALL
    // //                      SELECT t.*
    // //                      FROM Navigation t
    // //                               INNER JOIN
    // //                           rel r ON t.IdParent = r.ID)
    // //         SELECT rel.*
    // //         FROM rel
    // //         where rel.Id <> @IdNavigation";
    
    public const string QueryNavigationByParentId = "select *from [dbo].[Navigation] where IdParent = @IdParent order by CreatedDate desc";

    public const string QueryHideNavigation = "UPDATE [dbo].[Navigation] SET IsHide = @IsHide WHERE Id IN @Id";

    public const string QueryInsertNavigationDeleted = @"INSERT INTO [dbo].[Deleted_Navigation]
                               ([Id]
                               ,[MenuName]
                               ,[IdParent]
                               ,[Status]
                               ,[CreatedDate]
                               ,[Path]
                               ,[IconLink]
                               ,[MenuCode]
                               ,[Sort])
                         VALUES (@Id, @MenuName, @IdParent, @Status, @CreatedDate, @Path, @IconLink, @MenuCode, @Sort
                                )";
}