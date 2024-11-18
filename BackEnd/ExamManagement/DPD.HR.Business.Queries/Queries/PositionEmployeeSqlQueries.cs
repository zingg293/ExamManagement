namespace DPD.HR.Application.Queries.Queries;

public static class PositionEmployeeSqlQueries
{
    public const string QueryInsertPositionEmployee = @"INSERT INTO [dbo].[PositionEmployee]
           ([Id]
           ,[IdEmployee]
           ,[IdPosition]
           ,[IsHeadcount]
           ,[IdUnit]
           ,[CreatedDate]
           ,[Status])
                         VALUES (@Id, @IdEmployee, @IdPosition, @IsHeadcount, @IdUnit, @CreatedDate, @Status)";

    public const string QueryUpdatePositionEmployee = @"UPDATE [dbo].[PositionEmployee] SET 
                                        IdEmployee = @IdEmployee,
                                        IdPosition = @IdPosition,
                                        IsHeadcount = @IsHeadcount,
                                        IdUnit = @IdUnit
                                WHERE Id = @Id";

    public const string QueryDeletePositionEmployee = "DELETE FROM [dbo].[PositionEmployee] WHERE Id IN @Ids";
    public const string QueryDeletePositionEmployeeByIdPosition = "DELETE FROM [dbo].[PositionEmployee] WHERE IdPosition = @IdPosition";
    public const string QueryGetByIdPositionEmployee = "select * from [dbo].[PositionEmployee] where Id = @Id";
    public const string QueryGetPositionEmployeeByIds = "select * from [dbo].[PositionEmployee] where Id IN @Ids";
    public const string QueryGetAllPositionEmployee = "select *from [dbo].[PositionEmployee] order by CreatedDate desc";
    public const string QueryGetPositionEmployeeByIdEmployee = "select *from [dbo].[PositionEmployee] where IdEmployee = @IdEmployee order by CreatedDate desc";

    public const string QueryInsertPositionEmployeeDeleted = @"INSERT INTO [dbo].[Deleted_PositionEmployee]
           ([Id]
           ,[IdEmployee]
           ,[IdPosition]
           ,[IsHeadcount]
           ,[IdUnit]
           ,[CreatedDate]
           ,[Status])
                         VALUES (@Id, @IdEmployee, @IdPosition, @IsHeadcount, @IdUnit, @CreatedDate, @Status)";
}