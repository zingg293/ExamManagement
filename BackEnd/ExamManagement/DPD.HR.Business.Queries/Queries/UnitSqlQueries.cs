namespace DPD.HR.Application.Queries.Queries;

public static class UnitSqlQueries
{
    public const string GetAllUnit = "select *from [dbo].[Unit] order by CreatedDate desc";
    public const string GetAllUnitAvailable = "select * from Unit where IsHide = 0 order by CreatedDate desc";
    public const string GetByIdUnit = "select *from [dbo].[Unit] where Id = @Id";
    public const string GetUnitByIds = "select *from [dbo].[Unit] where Id IN @Ids";
    public const string GetUnitByUnitCode = "select * from [dbo].[Unit] where UnitCode = @UnitCode";
    public const string GetUnitByParent = "select * from [dbo].[Unit] where ParentId = @ParentId";
    public const string DeleteUnitByIds = "DELETE FROM [dbo].[Unit] WHERE Id IN @Ids";

    public const string UpdateUnit =
        "UPDATE Unit SET UnitName = @UnitName, ParentId = @ParentId WHERE Id = @Id";

    public const string HideUnit = "UPDATE Unit SET IsHide = @IsHide WHERE Id In @Ids";


    public const string QueryUnitParentAndChill = @"WITH ret AS(
                                            SELECT  *
                                            FROM    Unit
                                            WHERE   ID = @ID
                                            UNION ALL
                                            SELECT  t.*
                                            FROM    Unit t INNER JOIN
                                                    ret r ON t.ParentId = r.ID
                                    )
                                    SELECT  Id
                                    FROM ret";

    public const string InsertUnit = @"INSERT INTO [dbo].[Unit]
                               ([Id]
                               ,[UnitName]
                               ,[ParentId]
                               ,[Status]
                               ,[CreatedBy]
                               ,[CreatedDate]
                               ,[UnitCode]
                               ,[IsHide])
                         VALUES (@Id, @UnitName, @ParentId, @Status, @CreatedBy, @CreatedDate, @UnitCode, @IsHide)";
    
    public const string InsertUnitDeleted = @"INSERT INTO [dbo].[Deleted_Unit]
                               ([Id]
                               ,[UnitName]
                               ,[ParentId]
                               ,[Status]
                               ,[CreatedBy]
                               ,[CreatedDate]
                               ,[UnitCode]
                               ,[IsHide])
                         VALUES (@Id, @UnitName, @ParentId, @Status, @CreatedBy, @CreatedDate, @UnitCode, @IsHide)";
}