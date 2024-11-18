namespace DPD.HR.Application.Queries.Queries;

public static class BusinessTripEmployeeSqlQueries
{
    public const string QueryInsertBusinessTripEmployee = @"INSERT INTO [dbo].[BusinessTripEmployee]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdBusinessTrip]
           ,[IdEmployee]
           ,[Captain])
                         VALUES (@Id, @CreatedDate, @Status, @IdBusinessTrip, @IdEmployee, @Captain)";

    public const string QueryUpdateBusinessTripEmployee = @"UPDATE [dbo].[BusinessTripEmployee] SET 
                                        IdBusinessTrip = @IdBusinessTrip,
                                        IdEmployee = @IdEmployee,
                                        Captain = @Captain
                                        WHERE Id = @Id";
    
    public const string QueryUpdateStatusBusinessTripEmployee = @"UPDATE [dbo].[BusinessTripEmployee] SET 
                                 Status = @Status
                                        WHERE Id = @Id";

    public const string QueryDeleteBusinessTripEmployee = "DELETE FROM [dbo].[BusinessTripEmployee] WHERE Id IN @Ids";
    public const string QueryGetByIdBusinessTripEmployee = "select * from [dbo].[BusinessTripEmployee] where Id = @Id";
    public const string QueryGetBusinessTripEmployeeByIdBusinessTrip = "select * from [dbo].[BusinessTripEmployee] where IdBusinessTrip = @IdBusinessTrip";
    public const string QueryGetBusinessTripEmployeeByIds = "select * from [dbo].[BusinessTripEmployee] where Id IN @Ids";
    public const string QueryGetAllBusinessTripEmployee = "select *from [dbo].[BusinessTripEmployee] order by CreatedDate desc";
    public const string QueryGetAllBusinessTripEmployeeNotInIds = "select * from [dbo].[BusinessTripEmployee] where Id not in @Ids and IdBusinessTrip = @IdBusinessTrip";

    public const string QueryInsertBusinessTripEmployeeDeleted = @"INSERT INTO [dbo].[Deleted_BusinessTripEmployee]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdBusinessTrip]
           ,[IdEmployee]
           ,[Captain])
                         VALUES (@Id, @CreatedDate, @Status, @IdBusinessTrip, @IdEmployee, @Captain)";
}