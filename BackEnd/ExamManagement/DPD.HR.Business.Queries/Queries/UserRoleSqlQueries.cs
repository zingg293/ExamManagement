namespace DPD.HR.Application.Queries.Queries;

public static class UserRoleSqlQueries
{
    public const string QueryGetAllUserRoleByIdUser =
        @"select * from UserRole where IdUser = @IdUser";
    public const string QueryDeleteUserRoleByIdUser = @"delete from UserRole where IdUser = @IdUser";
                        //and IdRole not in (select Id from Role where RoleCode = 'AD'

    public const string InsertUserRole =
        "insert into dbo.UserRole (Id, IdRole, IdUser) values (@Id, @IdRole, @IdUser)";
}