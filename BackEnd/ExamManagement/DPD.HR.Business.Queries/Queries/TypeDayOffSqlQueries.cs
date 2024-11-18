namespace DPD.HR.Application.Queries.Queries;

public class TypeDayOffSqlQueries
{
    public const string QueryInsertTypeDayOff = @"INSERT INTO [dbo].[TypeDayOff]
           ([Id]
           ,[NameDayOff]
           ,[CreatedDate]
           ,[Status])
                         VALUES (@Id, @NameDayOff, @CreatedDate, @Status)";

    public const string QueryUpdateTypeDayOff = @"UPDATE [dbo].[TypeDayOff] SET
                                        NameDayOff = @NameDayOff
                                        WHERE Id = @Id";

    public const string QueryDeleteTypeDayOff = "DELETE FROM [dbo].[TypeDayOff] WHERE Id IN @Ids";
    public const string QueryGetByIdTypeDayOff = "select * from [dbo].[TypeDayOff] where Id = @Id";
    public const string QueryGetTypeDayOffByIds = "select * from [dbo].[TypeDayOff] where Id IN @Ids";
    public const string QueryGetAllTypeDayOff = "select *from [dbo].[TypeDayOff] order by CreatedDate desc";

    public const string QueryInsertTypeDayOffDeleted = @"INSERT INTO [dbo].[Deleted_TypeDayOff]
           ([Id]
           ,[NameDayOff]
           ,[CreatedDate]
           ,[Status])
                         VALUES (@Id, @NameDayOff, @CreatedDate, @Status)";
}