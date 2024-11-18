namespace DPD.HR.Application.Queries.Queries;

public static class LaborEquipmentUnitSqlQueries
{
    public const string QueryInsertLaborEquipmentUnit = @"INSERT INTO [dbo].[LaborEquipmentUnit]
           ([Id]
           ,[Status]
           ,[CreatedDate]
           ,[IdEmployee]
           ,[IdTicketLaborEquipment]
           ,[IdCategoryLaborEquipment]
           ,[IdUnit]
           ,[EquipmentCode])
                         VALUES (@Id, @Status, @CreatedDate, @IdEmployee, @IdTicketLaborEquipment, @IdCategoryLaborEquipment,
                                @IdUnit, @EquipmentCode)";

    public const string QueryUpdateLaborEquipmentUnit = "";

    public const string QueryUpdateStatusLaborEquipmentUnit = @"UPDATE [dbo].[LaborEquipmentUnit] SET
                                       Status = @Status
                                        where Id = @Id";

    public const string QueryUpdateStatusLaborEquipmentUnitByCode = @"UPDATE [dbo].[LaborEquipmentUnit] SET
                                       Status = @Status
                                       where EquipmentCode = @EquipmentCode";

    public const string QueryUpdateStatusLaborEquipmentUnitByIds = @"UPDATE [dbo].[LaborEquipmentUnit] SET
                                       Status = @Status
                                        where Id IN @Id";

    public const string QueryUpdateStatusToUsingLaborEquipmentUnitByIds = @"UPDATE [dbo].[LaborEquipmentUnit] SET
                                       Status = @Status,
                                       IdEmployee = @IdEmployee,
                                       IdUnit = @IdUnit
                                        where Id IN @Id";

    public const string QueryDeleteLaborEquipmentUnit = "DELETE FROM [dbo].[LaborEquipmentUnit] WHERE Id IN @Ids";
    public const string QueryGetByIdLaborEquipmentUnit = "select * from [dbo].[LaborEquipmentUnit] where Id = @Id";
    public const string QueryGetByCodeLaborEquipmentUnit = "select * from [dbo].[LaborEquipmentUnit] where EquipmentCode = @EquipmentCode";
    public const string QueryGetLaborEquipmentUnitByIds = "select * from [dbo].[LaborEquipmentUnit] where Id IN @Ids";

    public const string QueryGetLaborEquipmentUnitByEquipmentCodes =
        "select * from [dbo].[LaborEquipmentUnit] where EquipmentCode IN @EquipmentCode";

    public const string QueryGetAllLaborEquipmentUnit =
        "select *from [dbo].[LaborEquipmentUnit] order by CreatedDate desc";

    public const string QueryGetAllLaborEquipmentUnitByStatus =
        "select *from [dbo].[LaborEquipmentUnit] where Status = @Status order by CreatedDate desc";

    public const string QueryGetAllLaborEquipmentUnitByIdUnitAndIdEmployee =
        "select *from [dbo].[LaborEquipmentUnit] where IdEmployee = @IdEmployee and IdUnit = @IdUnit order by CreatedDate desc";

    public const string QueryInsertLaborEquipmentUnitDeleted = @"INSERT INTO [dbo].[Deleted_LaborEquipmentUnit]
           ([Id]
           ,[Status]
           ,[CreatedDate]
           ,[IdEmployee]
           ,[IdTicketLaborEquipment]
           ,[IdCategoryLaborEquipment]
           ,[IdUnit]
           ,[EquipmentCode])
                         VALUES (@Id, @Status, @CreatedDate, @IdEmployee, @IdTicketLaborEquipment, @IdCategoryLaborEquipment,
                                @IdUnit, @EquipmentCode)";
}