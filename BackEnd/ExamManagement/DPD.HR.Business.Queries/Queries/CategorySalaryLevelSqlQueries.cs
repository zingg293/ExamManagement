using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class CategorySalaryLevelSqlQueries
    {
        public const string QueryInsertCategorySalaryLevel = @"
INSERT INTO [dbo].[CategorySalaryLevel]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[NameSalaryLevel]
           ,[Amount]
           ,[IdSalaryScale]
           ,[IsCoefficient]
           ,[SocialSecurityContributionRate])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @NameSalaryLevel,
           @Amount,
           @IdSalaryScale,
           @IsCoefficient,
           @SocialSecurityContributionRate)";
        public const string QueryUpdateCategorySalaryLevel = @"UPDATE [dbo].[CategorySalaryLevel]
                                                           SET
                                                              [CreatedDate] = @CreatedDate,
                                                              [Status] = @Status,
                                                              [NameSalaryLevel] = @NameSalaryLevel,
                                                              [Amount] = @Amount,
                                                              [IdSalaryScale] = @IdSalaryScale,
                                                              [IsCoefficient] = @IsCoefficient,
                                                              [SocialSecurityContributionRate] = @SocialSecurityContributionRate
                                                                  WHERE Id = @Id";
        public const string QueryGetByIdCategorySalaryLevel = @"select * from [dbo].[CategorySalaryLevel] where Id = @Id";
        public const string QueryGetCategorySalaryLevelByIds = @"select * from [dbo].[CategorySalaryLevel] where Id IN @Ids";
        public const string QueryDeleteCategorySalaryLevel = @"Delete from [dbo].[CategorySalaryLevel] where Id IN @Ids";
        public const string QueryGetAllCategorySalaryLevel = @"SELECT * FROM CategorySalaryLevel order by CreatedDate";
        public const string QueryGetAllCategorySalaryLevelAvailable = @"";
        public const string QueryHideCategorySalaryLevel = @"";
        public const string QueryInsertCategorySalaryLevelDeleted =
            @"
            INSERT INTO[dbo].[Deleted_CategorySalaryLevel]
                    ([Id]
                       ,[CreatedDate]
                       ,[Status]
                       ,[NameSalaryLevel]
                       ,[Amount]
                       ,[IdSalaryScale]
                       ,[IsCoefficient]
                       ,[SocialSecurityContributionRate])
                 VALUES
                       (@Id,
                       @CreatedDate,
                       @Status,
                       @NameSalaryLevel,
                       @Amount,
                       @IdSalaryScale,
                       @IsCoefficient,
                       @SocialSecurityContributionRate)";
    }
}
