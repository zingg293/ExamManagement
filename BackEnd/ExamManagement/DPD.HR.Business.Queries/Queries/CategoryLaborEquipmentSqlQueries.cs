namespace DPD.HR.Application.Queries.Queries;

public class CategoryLaborEquipmentSqlQueries
{
    public const string QueryInsertCategoryLaborEquipment = @"INSERT INTO [dbo].[CategoryLaborEquipment]
                               ([Id]
                               ,[Name]
                               ,[Unit]
                               ,[Code]
                               ,[Description]
                               ,[CreatedDate]
                               ,[Status])
                         VALUES (@Id, @Name, @Unit, @Code, @Description, @CreatedDate, @Status)";

    public const string QueryUpdateCategoryLaborEquipment = @"UPDATE [dbo].[CategoryLaborEquipment] SET 
                                        Name = @Name,
                                        Unit = @Unit,
                                        Code = @Code,
                                        Description = @Description
                                        WHERE Id = @Id";

    public const string QueryDeleteCategoryLaborEquipment =
        "DELETE FROM [dbo].[CategoryLaborEquipment] WHERE Id IN @Ids";

    public const string QueryGetByIdCategoryLaborEquipment =
        "select * from [dbo].[CategoryLaborEquipment] where Id = @Id";
    
    public const string QueryCategoryLaborEquipmentByIds =
        "select * from [dbo].[CategoryLaborEquipment] where Id IN @Ids";

    public const string QueryGetAllCategoryLaborEquipment =
        "select *from [dbo].[CategoryLaborEquipment] order by CreatedDate desc";

    public const string QueryInsertCategoryLaborEquipmentDeleted = @"INSERT INTO [dbo].[Deleted_CategoryLaborEquipment]
                               ([Id]
                               ,[Name]
                               ,[Unit]
                               ,[Code]
                               ,[Description]
                               ,[CreatedDate]
                               ,[Status])
                         VALUES (@Id, @Name, @Unit, @Code, @Description, @CreatedDate, @Status)";
}