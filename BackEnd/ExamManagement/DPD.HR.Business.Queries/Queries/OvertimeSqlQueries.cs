namespace DPD.HR.Application.Queries.Queries;

public static class OvertimeSqlQueries
{
    public const string QueryInsertOvertime = @"INSERT INTO [dbo].[Overtime]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdEmployee]
           ,[IdUnit]
           ,[UnitName]
           ,[FromDate]
           ,[ToDate])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdEmployee, @IdUnit, @UnitName, @FromDate, @ToDate)";

    public const string QueryUpdateOvertime = @"UPDATE [dbo].[Overtime] SET 
                                        Description = @Description,
                                        IdEmployee = @IdEmployee,
                                        IdUnit = @IdUnit,
                                        FromDate = @FromDate,
                                        ToDate = @ToDate,
                                        UnitName = @UnitName
                                        WHERE Id = @Id";
    
    public const string QueryUpdateStatusOvertime = @"UPDATE [dbo].[Overtime] SET 
                                 Status = @Status
                                        WHERE Id = @Id";

    public const string QueryDeleteOvertime = "DELETE FROM [dbo].[Overtime] WHERE Id IN @Ids";
    public const string QueryGetByIdOvertime = "select * from [dbo].[Overtime] where Id = @Id";
    public const string QueryGetOvertimeByIds = "select * from [dbo].[Overtime] where Id IN @Ids";
    public const string QueryGetAllOvertime = "select *from [dbo].[Overtime] order by CreatedDate desc";
    
    public const string QueryGetOvertimeByEmployeeByCondition = @"select *
                    from Overtime
                    where IdEmployee = @IdEmployee
                      and Id in (select ItemId
                                 from WorkflowInstances
                                 where IsCompleted = 1
                                   and TemplateId in (select Id from WorkflowTemplate where WorkflowCode = @WorkflowCode))
                      and MONTH(FromDate) >= @Month and YEAR(FromDate) = @Year
                      and MONTH(ToDate) <= @Month and YEAR(ToDate) = @Year
                    order by CreatedDate desc";

    public const string QueryInsertOvertimeDeleted = @"INSERT INTO [dbo].[Deleted_Overtime]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdEmployee]
           ,[IdUnit]
           ,[UnitName]
           ,[FromDate]
           ,[ToDate])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdEmployee, @IdUnit, @UnitName, @FromDate, @ToDate)";
}