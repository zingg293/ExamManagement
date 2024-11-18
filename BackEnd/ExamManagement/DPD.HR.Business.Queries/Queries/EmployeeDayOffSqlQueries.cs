namespace DPD.HR.Application.Queries.Queries;

public static class EmployeeDayOffSqlQueries
{
    public const string QueryInsertEmployeeDayOff = @"INSERT INTO [dbo].[EmployeeDayOff]
                               ([Id]
                               ,[IdEmployee]
                               ,[DayOff]
                               ,[TypeOfDayOff]
                               ,[CreatedDate]
                               ,[Status]
                               ,[OnLeave])
                         VALUES (@Id, @IdEmployee, @DayOff, @TypeOfDayOff, @CreatedDate, @Status, @OnLeave)";

    public const string QueryUpdateEmployeeDayOff = @"UPDATE [dbo].[EmployeeDayOff] SET 
                                        IdEmployee = @IdEmployee,
                                        DayOff = @DayOff,
                                        TypeOfDayOff = @TypeOfDayOff,
                                        OnLeave = @OnLeave
                                        WHERE Id = @Id";

    public const string QueryDeleteEmployeeDayOff = "DELETE FROM [dbo].[EmployeeDayOff] WHERE Id IN @Ids";
    public const string QueryGetByIdEmployeeDayOff = "select * from [dbo].[EmployeeDayOff] where Id = @Id";
    public const string QueryEmployeeDayOffByIds = "select * from [dbo].[EmployeeDayOff] where Id IN @Ids";
    public const string QueryGetAllEmployeeDayOff = "select *from [dbo].[EmployeeDayOff] order by CreatedDate desc";
    public const string QueryEmployeeDayOffByDate = @"SELECT *
                            FROM EmployeeDayOff
                            where FORMAT(DayOff, 'dd/MM/yyyy') = @DayOff
                            and IdEmployee = @IdEmployee";
    
    public const string QueryEmployeeDayOffByIdEmployee = "select * from [dbo].[EmployeeDayOff] where IdEmployee IN @IdEmployee";


    public const string QueryInsertEmployeeDayOffDeleted = @"INSERT INTO [dbo].[Deleted_EmployeeDayOff]
                               ([Id]
                               ,[IdEmployee]
                               ,[DayOff]
                               ,[TypeOfDayOff]
                               ,[CreatedDate]
                               ,[Status]
                               ,[OnLeave])
                         VALUES (@Id, @IdEmployee, @DayOff, @TypeOfDayOff, @CreatedDate, @Status, @OnLeave)";
}