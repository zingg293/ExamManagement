namespace DPD.HR.Application.Queries.Queries;

public static class PromotionTransferSqlQueries
{
    public const string QueryInsertPromotionTransfer = @"INSERT INTO [dbo].[PromotionTransfer]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdUnit]
           ,[IdEmployee]
           ,[UnitName]
           ,[IdPositionEmployeeCurrent]
           ,[PositionNameCurrent]
           ,[IsTransfer]
           ,[IsPromotion]
           ,[IdCategoryPosition]
           ,[NameCategoryPosition]
           ,[IsHeadCount])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdUnit, @IdEmployee, @UnitName,
                              @IdPositionEmployeeCurrent, @PositionNameCurrent, @IsTransfer, @IsPromotion, @IdCategoryPosition, @NameCategoryPosition, @IsHeadCount)";

    public const string QueryUpdatePromotionTransfer = @"UPDATE [dbo].[PromotionTransfer] SET 
                                        Status = @Status
                                        WHERE Id = @Id";
    
    public const string QueryUpdateStatusPromotionTransfer = @"UPDATE [dbo].[PromotionTransfer] SET 
                                        Description = @Description,
                                        IdUnit = @IdUnit,
                                        IdEmployee = @IdEmployee,
                                        UnitName = @UnitName,
                                        IdPosition = @IdPosition,
                                        PositionName = @PositionName
                                        WHERE Id = @Id";

    public const string QueryDeletePromotionTransfer = "DELETE FROM [dbo].[PromotionTransfer] WHERE Id IN @Ids";
    public const string QueryGetByIdPromotionTransfer = "select * from [dbo].[PromotionTransfer] where Id = @Id";
    public const string QueryGetPromotionTransferByIds = "select * from [dbo].[PromotionTransfer] where Id IN @Ids";
    public const string QueryGetAllPromotionTransfer = "select *from [dbo].[PromotionTransfer] order by CreatedDate desc";
    
    public const string QueryGetPromotionTransferByIdEmployee = "select * from [dbo].[PromotionTransfer] where IdEmployee = @IdEmployee";
    public const string QueryGetPromotionTransferByIdCategoryPosition = "select * from [dbo].[PromotionTransfer] where IdCategoryPosition IN @IdCategoryPosition";


    public const string QueryInsertPromotionTransferDeleted = @"INSERT INTO [dbo].[Deleted_PromotionTransfer]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdUnit]
           ,[IdEmployee]
           ,[UnitName]
           ,[IdPositionEmployeeCurrent]
           ,[PositionNameCurrent]
           ,[IsTransfer]
           ,[IsPromotion]
           ,[IdCategoryPosition]
           ,[NameCategoryPosition]
            ,[IsHeadCount])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdUnit, @IdEmployee, @UnitName,
                              @IdPositionEmployeeCurrent, @PositionNameCurrent, @IsTransfer, @IsPromotion, @IdCategoryPosition, @NameCategoryPosition, @IsHeadCount)";
}