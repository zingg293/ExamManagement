namespace DPD.HR.Application.Queries.Queries;

public static class InternRequestSqlQueries
{
    public const string QueryInsertInternRequest = @"INSERT INTO [dbo].[InternRequest]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdEmployee]
           ,[IdUnit]
           ,[UnitName]
           ,[IdPosition]
           ,[PositionName]
           ,[Attachments])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdEmployee, @IdUnit, @UnitName,
                           @IdPosition, @PositionName, @Attachments)";

    public const string QueryUpdateInternRequest = @"UPDATE [dbo].[InternRequest] SET 
                                        Description = @Description,
                                        IdEmployee = @IdEmployee,
                                        IdUnit = @IdUnit,
                                        UnitName = @UnitName,
                                        Attachments = @Attachments,
                                        IdPosition = @IdPosition,
                                        PositionName = @PositionName
                                        WHERE Id = @Id";
    
    public const string QueryUpdateStatusInternRequest = @"UPDATE [dbo].[InternRequest] SET 
                                 Status = @Status
                                        WHERE Id = @Id";

    public const string QueryDeleteInternRequest = "DELETE FROM [dbo].[InternRequest] WHERE Id IN @Ids";
    public const string QueryGetByIdInternRequest = "select * from [dbo].[InternRequest] where Id = @Id";
    public const string QueryGetInternRequestByIds = "select * from [dbo].[InternRequest] where Id IN @Ids";
    public const string QueryGetAllInternRequest = "select *from [dbo].[InternRequest] order by CreatedDate desc";

    public const string QueryInsertInternRequestDeleted = @"INSERT INTO [dbo].[Deleted_InternRequest]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdEmployee]
           ,[IdUnit]
           ,[UnitName]
           ,[IdPosition]
           ,[PositionName]
           ,[Attachments])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdEmployee, @IdUnit, @UnitName,
                           @IdPosition, @PositionName, @Attachments)";
}