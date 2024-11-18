namespace DPD.HR.Application.Queries.Queries;

public static class NavigationRoleSqlQueries
{
    public const string QueryInsertNavigationRole = @"INSERT INTO [dbo].[NavigationRole]
                                   ([Id]
                                   ,[IdRole]
                                   ,[IdNavigation])
                         VALUES (@Id, @IdRole, @IdNavigation)";
    
    public const string QueryDeleteNavigationRole = "DELETE FROM [dbo].[NavigationRole] WHERE Id IN @Ids";
    public const string QueryGetNavigationRoleByIdRole = "select * from NavigationRole where IdRole IN @IdRole";
}