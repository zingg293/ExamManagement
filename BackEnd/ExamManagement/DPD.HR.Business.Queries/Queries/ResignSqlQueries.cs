namespace DPD.HR.Application.Queries.Queries;

public class ResignSqlQueries
{
    public const string QueryInsertResign = @"INSERT INTO [dbo].[Resign]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdEmployee]
           ,[IdUnit]
           ,[UnitName]
           ,[ResignForm])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdEmployee, @IdUnit, @UnitName, @ResignForm)";

    public const string QueryUpdateResign = @"UPDATE [dbo].[Resign] SET 
                                        Description = @Description,
                                        IdEmployee = @IdEmployee,
                                        IdUnit = @IdUnit,
                                        ResignForm = @ResignForm,
                                        UnitName = @UnitName
                                        WHERE Id = @Id";
    
    public const string QueryUpdateStatusResign = @"UPDATE [dbo].[Resign] SET 
                                 Status = @Status
                                        WHERE Id = @Id";

    public const string QueryDeleteResign = "DELETE FROM [dbo].[Resign] WHERE Id IN @Ids";
    public const string QueryGetByIdResign = "select * from [dbo].[Resign] where Id = @Id";
    public const string QueryGetResignByIds = "select * from [dbo].[Resign] where Id IN @Ids";
    public const string QueryGetAllResign = "select *from [dbo].[Resign] order by CreatedDate desc";

    public const string QueryInsertResignDeleted = @"INSERT INTO [dbo].[Deleted_Resign]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdEmployee]
           ,[IdUnit]
           ,[UnitName]
           ,[ResignForm])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdEmployee, @IdUnit, @UnitName, @ResignForm)";
}