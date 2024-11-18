namespace DPD.HR.Application.Queries.Queries
{
    public static class SalaryCoefficientSqlQueries
    {
        public const string QueryInsertSalaryCoefficient = @"INSERT INTO [dbo].[SalaryCoefficient]
                                                                       ([Id]
                                                                       ,[CreatedDate]
                                                                       ,[Status]
                                                                       ,[IdEmployee]
                                                                       ,[IdSalaryScale]
                                                                       ,[IdSalaryLevel]
                                                                       ,[IdTypeSalaryScale]
                                                                       ,[DateUpgradeSalaryLevel]
                                                                       ,[DateChangeSalary]
                                                                       ,[NetSalary]
                                                                       ,[BasicCoefficient]
                                                                       ,[SocialSecurityContributionRate])
                                                                 VALUES
                                                                       (@Id
                                                                       ,@CreatedDate
                                                                       ,@Status
                                                                       ,@IdEmployee
                                                                       ,@IdSalaryScale
                                                                       ,@IdSalaryLevel
                                                                       ,@IdTypeSalaryScale
                                                                       ,@DateUpgradeSalaryLevel
                                                                       ,@DateChangeSalary
                                                                       ,@NetSalary
                                                                       ,@BasicCoefficient
                                                                       ,@SocialSecurityContributionRate)";
        public const string QueryUpdateSalaryCoefficient = @"
                    UPDATE [dbo].[SalaryCoefficient]
                       SET
                          [CreatedDate] = @CreatedDate,
                          [Status] = @Status,
                          [IdEmployee] = @IdEmployee,
                          [IdSalaryScale] = @IdSalaryScale,
                          [IdSalaryLevel] = @IdSalaryLevel,
                          [IdTypeSalaryScale] = @IdTypeSalaryScale,
                          [DateUpgradeSalaryLevel] = @DateUpgradeSalaryLevel,
                          [DateChangeSalary] = @DateChangeSalary,
                          [NetSalary] = @NetSalary,
                          [BasicCoefficient] = @BasicCoefficient,
                          [SocialSecurityContributionRate] = @SocialSecurityContributionRate
                              WHERE Id = @Id";
        public const string QueryGetByIdSalaryCoefficient = @"select * from [dbo].[SalaryCoefficient] where Id = @Id";
        public const string QueryGetSalaryCoefficientByIds = @"select * from [dbo].[SalaryCoefficient] where Id IN @Ids";
        public const string QueryDeleteSalaryCoefficient = @"Delete from [dbo].[SalaryCoefficient] where Id IN @Ids";
        public const string QueryGetAllSalaryCoefficient = @"SELECT * FROM SalaryCoefficient order by CreatedDate";
        public const string QueryGetAllSalaryCoefficientAvailable = @"";
        public const string QueryHideSalaryCoefficient = @"";
        public const string QueryInsertSalaryCoefficientDeleted = @"INSERT INTO [dbo].[Deleted_SalaryCoefficient]
                               ([Id]
                               ,[CreatedDate]
                               ,[Status]
                               ,[IdEmployee]
                               ,[IdSalaryScale]
                               ,[IdSalaryLevel]
                               ,[IdTypeSalaryScale]
                               ,[DateUpgradeSalaryLevel]
                               ,[DateChangeSalary]
                               ,[NetSalary]
                               ,[BasicCoefficient]
                               ,[SocialSecurityContributionRate])
                         VALUES
                               (@Id
                               ,@CreatedDate
                               ,@Status
                               ,@IdEmployee
                               ,@IdSalaryScale
                               ,@IdSalaryLevel
                               ,@IdTypeSalaryScale
                               ,@DateUpgradeSalaryLevel
                               ,@DateChangeSalary
                               ,@NetSalary
                               ,@BasicCoefficient
                               ,@SocialSecurityContributionRate)";
    }
}
