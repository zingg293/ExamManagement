namespace DPD.HR.Application.Queries.Queries;

public static class AllowanceSqlQueries
{
    public const string QueryInsertAllowance = @"INSERT INTO [dbo].[Allowance]
                                                               ([Id]
                                                               ,[Name]
                                                               ,[Amount]
                                                               ,[CreatedDate]
                                                               ,[Status])
                                                      VALUES (@Id, @Name, @Amount, @CreatedDate, @Status)";

    public const string QueryUpdateAllowance = @"UPDATE [dbo].[Allowance] SET Name = @Name,
                                        Amount = @Amount
                                        WHERE Id = @Id";
    
    public const string QueryGetAllAllowanceByIdEmployee = @"
                            select a.*
                            from Employee e
                                     join EmployeeAllowance ea on ea.IdEmployee = e.Id
                                     join Allowance a on a.Id = ea.IdAllowance
                            where e.Id = @IdEmployee";

    public const string QueryDeleteListAllowance = "DELETE FROM [dbo].[Allowance] WHERE Id in @Ids";
    public const string QueryGetByIdAllowance = "select * from [dbo].[Allowance] where Id = @Id";

    public const string QueryGetAllAllowance = "select *from [dbo].[Allowance] order by CreatedDate desc";
    public const string QueryGetAllAllowanceByIds = "select *from [dbo].[Allowance] where Id IN @Ids";
    
    public const string InsertAllowanceDelete = @"INSERT INTO [dbo].[Deleted_Allowance]
                                                               ([Id]
                                                               ,[Name]
                                                               ,[Amount]
                                                               ,[CreatedDate]
                                                               ,[Status])
                                                      VALUES (@Id, @Name, @Amount, @CreatedDate, @Status)";
}