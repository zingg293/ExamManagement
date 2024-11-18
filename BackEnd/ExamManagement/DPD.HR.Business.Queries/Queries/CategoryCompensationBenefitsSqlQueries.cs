namespace DPD.HR.Application.Queries.Queries;

public static class CategoryCompensationBenefitsSqlQueries
{
    public const string QueryInsertCategoryCompensationBenefits = @"INSERT INTO [dbo].[CategoryCompensationBenefits]
                               ([Id]
                               ,[Name]
                               ,[Status]
                               ,[CreatedDate]
                               ,[AmountMoney])
                         VALUES (@Id, @Name, @Status, @CreatedDate, @AmountMoney)";

    public const string QueryUpdateCategoryCompensationBenefits = @"UPDATE [dbo].[CategoryCompensationBenefits] SET 
                                        Name = @Name,
                                        AmountMoney = @AmountMoney
                                        WHERE Id = @Id";

    public const string QueryDeleteCategoryCompensationBenefits =
        "DELETE FROM [dbo].[CategoryCompensationBenefits] WHERE Id IN @Ids";

    public const string QueryGetByIdCategoryCompensationBenefits =
        "select * from [dbo].[CategoryCompensationBenefits] where Id = @Id";
    
    public const string QueryGetCategoryCompensationBenefitsByIds =
        "select * from [dbo].[CategoryCompensationBenefits] where Id IN @Ids";

    public const string QueryGetAllCategoryCompensationBenefits =
        "select *from [dbo].[CategoryCompensationBenefits] order by CreatedDate desc";

    public const string QueryInsertCategoryCompensationBenefitsDeleted =
        @"INSERT INTO [dbo].[Deleted_CategoryCompensationBenefits]
                               ([Id]
                               ,[Name]
                               ,[Status]
                               ,[CreatedDate]
                               ,[AmountMoney])
                         VALUES (@Id, @Name, @Status, @CreatedDate, @AmountMoney)";
}