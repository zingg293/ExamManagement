namespace DPD.HR.Application.Queries.Queries;

public static class TicketLaborEquipmentDetailSqlQueries
{
    public const string QueryInsertTicketLaborEquipmentDetail = @"INSERT INTO [dbo].[TicketLaborEquipmentDetail]
                           ([Id]
                           ,[IdTicketLaborEquipment]
                           ,[IdCategoryLaborEquipment]
                           ,[Quantity]
                           ,[CreatedDate]
                           ,[Status]
                           ,[IdEmployee]
                           ,[EquipmentCode]
                           ,[IsCheck])
                         VALUES (@Id, @IdTicketLaborEquipment, @IdCategoryLaborEquipment, @Quantity, @CreatedDate,
                                        @Status, @IdEmployee, @EquipmentCode, @IsCheck)";

    public const string QueryUpdateTicketLaborEquipmentDetail = @"UPDATE [dbo].[TicketLaborEquipmentDetail] SET 
                                        IdTicketLaborEquipment = @IdTicketLaborEquipment,
                                        IdCategoryLaborEquipment = @IdCategoryLaborEquipment,
                                        Quantity = @Quantity,
                                        IdEmployee = @IdEmployee,
                                        EquipmentCode = @EquipmentCode
                                        WHERE Id = @Id";

    public const string QueryUpdateIsCheckTicketLaborEquipmentDetail = @"UPDATE [dbo].[TicketLaborEquipmentDetail] SET 
                                        IsCheck = @IsCheck
                                        WHERE Id IN @Id";
    
    public const string QueryGetByEquipmentCodeTicketLaborEquipmentDetail =
        "select * from [dbo].[TicketLaborEquipmentDetail] where EquipmentCode = @EquipmentCode and IsCheck = @IsCheck";
    
    public const string QueryDeleteTicketLaborEquipmentDetail =
        "DELETE FROM [dbo].[TicketLaborEquipmentDetail] WHERE Id IN @Ids";

    public const string QueryGetByIdTicketLaborEquipmentDetail =
        "select * from [dbo].[TicketLaborEquipmentDetail] where Id = @Id";
    
    public const string QueryTicketLaborEquipmentDetailByIdTicketLaborEquipment =
        "select * from [dbo].[TicketLaborEquipmentDetail] where IdTicketLaborEquipment = @IdTicketLaborEquipment";

    public const string QueryTicketLaborEquipmentDetailByIdCategoryLaborEquipment =
        "select * from [dbo].[TicketLaborEquipmentDetail] where IdCategoryLaborEquipment IN @IdCategoryLaborEquipment";
    
    public const string QueryGetTicketLaborEquipmentDetailNotInIds =
        "select * from [dbo].[TicketLaborEquipmentDetail] where Id not in @Ids and IdTicketLaborEquipment = @IdTicketLaborEquipment";

    public const string QueryGetAllTicketLaborEquipmentDetail =
        "select *from [dbo].[TicketLaborEquipmentDetail] order by CreatedDate desc";

    public const string QueryInsertTicketLaborEquipmentDetailDeleted =
        @"INSERT INTO [dbo].[Deleted_TicketLaborEquipmentDetail]
                           ([Id]
                           ,[IdTicketLaborEquipment]
                           ,[IdCategoryLaborEquipment]
                           ,[Quantity]
                           ,[CreatedDate]
                           ,[Status]
                           ,[IdEmployee]
                           ,[EquipmentCode]
                           ,[IsCheck])
                         VALUES (@Id, @IdTicketLaborEquipment, @IdCategoryLaborEquipment, @Quantity, @CreatedDate,
                                        @Status, @IdEmployee, @EquipmentCode, @IsCheck)";
}