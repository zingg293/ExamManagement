namespace DPD.HR.Application.Queries.Queries;

public static class CategoryVacanciesSqlQueries
{
    public const string QueryInsertCategoryVacancies = @"INSERT INTO [dbo].[CategoryVacancies]
                           ([Id]
                           ,[Status]
                           ,[CreatedDate]
                           ,[PositionName]
                           ,[NumExp]
                           ,[Degree]
                           ,[JobRequirements]
                           ,[JobDescription])
                         VALUES (@Id, @Status, @CreatedDate, @PositionName, @NumExp, @Degree, @JobRequirements, @JobDescription)";

    public const string QueryUpdateCategoryVacancies = @"UPDATE [dbo].[CategoryVacancies] SET 
                                        PositionName = @PositionName,
                                        NumExp = @NumExp,
                                        Degree = @Degree,
                                        JobRequirements = @JobRequirements,
                                        JobDescription = @JobDescription
                                        WHERE Id = @Id";
    
    public const string QueryUpdateStatusCategoryVacancy = @"UPDATE [dbo].[CategoryVacancies] SET 
                                        Status = @Status
                                        WHERE Id = @Id";

    public const string QueryDeleteCategoryVacancies = "DELETE FROM [dbo].[CategoryVacancies] WHERE Id IN @Ids";
    public const string QueryGetByIdCategoryVacancies = "select * from [dbo].[CategoryVacancies] where Id = @Id";
    public const string QueryCategoryVacanciesByIds = "select * from [dbo].[CategoryVacancies] where Id IN @Ids";

    public const string QueryCategoryVacanciesByIdRequestToHire = @"
                            select p.*
                                from CategoryVacancies p
                            where p.Id In (select IdCategoryVacancies from RequestToHired where Id = @Id)
                            ";

    public const string QueryGetAllCategoryVacancies = "select *from [dbo].[CategoryVacancies] order by CreatedDate desc";

    public const string QueryInsertCategoryVacanciesDeleted = @"INSERT INTO [dbo].[Deleted_CategoryVacancies]
                           ([Id]
                           ,[Status]
                           ,[CreatedDate]
                           ,[PositionName]
                           ,[NumExp]
                           ,[Degree]
                           ,[JobRequirements]
                           ,[JobDescription])
                         VALUES (@Id, @Status, @CreatedDate, @PositionName, @NumExp, @Degree, @JobRequirements, @JobDescription)";
}