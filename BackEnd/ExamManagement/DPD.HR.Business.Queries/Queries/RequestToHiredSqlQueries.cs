namespace DPD.HR.Application.Queries.Queries;

public static class RequestToHiredSqlQueries
{
    public const string QueryInsertRequestToHired = @"INSERT INTO [dbo].[RequestToHired]
                           ([Id]
                           ,[Status]
                           ,[CreatedDate]
                           ,[Reason]
                           ,[Quantity]
                           ,[FilePath]
                           ,[IdCategoryVacancies]
                           ,[CreatedBy]
                           ,[IdUnit])
                         VALUES (@Id, @Status, @CreatedDate, @Reason, @Quantity, @FilePath, @IdCategoryVacancies, @CreatedBy, @IdUnit)";

    public const string QueryUpdateRequestToHired = @"UPDATE [dbo].[RequestToHired] SET 
                                        Reason = @Reason,
                                        Quantity = @Quantity,
                                        IdCategoryVacancies = @IdCategoryVacancies,
                                        IdUnit = @IdUnit,
                                        FilePath = @FilePath
                                        WHERE Id = @Id";
    
    public const string QueryUpdateStatusRequestToHired = @"UPDATE [dbo].[RequestToHired] SET 
                                        Status = @Status
                                        WHERE Id = @Id";

    public const string QueryDeleteRequestToHired = "DELETE FROM [dbo].[RequestToHired] WHERE Id IN @Ids";
    public const string QueryGetByIdRequestToHired = "select * from [dbo].[RequestToHired] where Id = @Id";
    public const string QueryRequestToHiredByIdCategoryVacancies = "select * from [dbo].[RequestToHired] where IdCategoryVacancies IN @IdCategoryVacancies";
    public const string QueryGetRequestToHiredByIds = "select * from [dbo].[RequestToHired] where Id IN @Ids";
    public const string QueryGetAllRequestToHired = "select *from [dbo].[RequestToHired] order by CreatedDate desc";

    public const string QueryInsertRequestToHiredDeleted = @"INSERT INTO [dbo].[Deleted_RequestToHired]
                           ([Id]
                           ,[Status]
                           ,[CreatedDate]
                           ,[Reason]
                           ,[Quantity]
                           ,[FilePath]
                           ,[IdCategoryVacancies]
                           ,[CreatedBy]
                           ,[IdUnit])
                         VALUES (@Id, @Status, @CreatedDate, @Reason, @Quantity, @FilePath, @IdCategoryVacancies, @CreatedBy, @IdUnit)";
}