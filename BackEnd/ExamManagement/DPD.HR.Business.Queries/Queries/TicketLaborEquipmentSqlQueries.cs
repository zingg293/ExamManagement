namespace DPD.HR.Application.Queries.Queries;

public static class TicketLaborEquipmentSqlQueries
{
    public const string QueryInsertTicketLaborEquipment = @"INSERT INTO [dbo].[TicketLaborEquipment]
                               ([Id]
                               ,[IdUserRequest]
                               ,[Type]
                               ,[Reason]
                               ,[FileAttachment]
                               ,[Description]
                               ,[CreatedDate]
                               ,[Status]
                               ,[IdUnit])   
                         VALUES (@Id, @IdUserRequest, @Type, @Reason, @FileAttachment, @Description, @CreatedDate, @Status, @IdUnit)";

    public const string QueryUpdateTicketLaborEquipment = @"UPDATE [dbo].[TicketLaborEquipment] SET 
                                        Type = @Type,
                                        Reason = @Reason,
                                        FileAttachment = @FileAttachment,
                                        Description = @Description
                                        WHERE Id = @Id";
    
    public const string QueryUpdateStatusTicketLaborEquipment = @"UPDATE [dbo].[TicketLaborEquipment] SET 
                                        Status = @Status
                                        WHERE Id = @Id";

    public const string QueryDeleteTicketLaborEquipment = "DELETE FROM [dbo].[TicketLaborEquipment] WHERE Id IN @Ids";
    public const string QueryGetByIdTicketLaborEquipment = "select * from [dbo].[TicketLaborEquipment] where Id = @Id";
    public const string QueryGetTicketLaborEquipmentByIds = "select * from [dbo].[TicketLaborEquipment] where Id IN @Ids";
    public const string QueryGetAllTicketLaborEquipment = "select *from [dbo].[TicketLaborEquipment] order by CreatedDate desc";

    public const string QueryInsertTicketLaborEquipmentDeleted = @"INSERT INTO [dbo].[Deleted_TicketLaborEquipment]
                               ([Id]
                               ,[IdUserRequest]
                               ,[Type]
                               ,[Reason]
                               ,[FileAttachment]
                               ,[Description]
                               ,[CreatedDate]
                               ,[Status]
                               ,[IdUnit])   
                         VALUES (@Id, @IdUserRequest, @Type, @Reason, @FileAttachment, @Description, @CreatedDate, @Status, @IdUnit)";
}