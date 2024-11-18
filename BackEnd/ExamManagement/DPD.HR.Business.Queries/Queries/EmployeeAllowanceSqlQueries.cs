namespace DPD.HR.Application.Queries.Queries;

public static class EmployeeAllowanceSqlQueries
{
    public const string QueryInsertEmployeeAllowance = @"INSERT INTO [dbo].[EmployeeAllowance]
                               ([Id]
                               ,[IdAllowance]
                               ,[IdEmployee]
                               ,[CreatedDate])
                         VALUES (@Id, @IdAllowance, @IdEmployee, @CreatedDate)";

    public const string QueryDeleteEmployeeAllowance = "DELETE FROM [dbo].[EmployeeAllowance] WHERE Id IN @Ids";
    public const string QueryGetByIdEmployeeAllowance = "select * from [dbo].[EmployeeAllowance] where Id = @Id";
    public const string QueryEmployeeAllowanceByIdAllowance = "select * from [dbo].[EmployeeAllowance] where IdAllowance IN @IdAllowance";

    public const string QueryGetAllEmployeeAllowanceByIdEmployee =
        "select *from [dbo].[EmployeeAllowance] where IdEmployee = @IdEmployee order by CreatedDate desc";

    public const string QueryInsertEmployeeAllowanceDeleted = @"INSERT INTO [dbo].[Deleted_EmployeeAllowance]
                               ([Id]
                               ,[IdAllowance]
                               ,[IdEmployee]
                               ,[CreatedDate])
                         VALUES (@Id, @IdAllowance, @IdEmployee, @CreatedDate)";
}