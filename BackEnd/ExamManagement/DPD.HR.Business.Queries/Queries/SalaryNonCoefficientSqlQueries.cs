namespace DPD.HR.Application.Queries.Queries
{
    public static class SalaryNonCoefficientSqlQueries
    {
        public const string QueryInsertSalaryNonCoefficient = 
            @"INSERT INTO [dbo].[SalaryNonCoefficient]
                           ([Id]
                           ,[CreatedDate]
                           ,[Status]
                           ,[IdEmployee]
                           ,[IdSalaryScale]
                           ,[IdSalaryLevel]
                           ,[DateUpgradeSalaryLevel]
                           ,[DateChangeSalary]
                           ,[NetSalary]
                           ,[BasicSalary]
                           ,[SocialSecuritySalary]
                           ,[IdTypeSalaryScale])
                     VALUES
                           (@Id
                           ,@CreatedDate
                           ,@Status
                           ,@IdEmployee
                           ,@IdSalaryScale
                           ,@IdSalaryLevel
                           ,@DateUpgradeSalaryLevel
                           ,@DateChangeSalary
                           ,@NetSalary
                           ,@BasicSalary
                           ,@SocialSecuritySalary
                           ,@IdTypeSalaryScale)";
        public const string QueryUpdateSalaryNonCoefficient =
                            @"UPDATE [dbo].[SalaryNonCoefficient]
                               SET [Id] = @Id
                                  ,[CreatedDate] = @CreatedDate
                                  ,[Status] = @Status
                                  ,[IdEmployee] = @IdEmployee
                                  ,[IdSalaryScale] = @IdSalaryScale
                                  ,[IdSalaryLevel] = @IdSalaryLevel
                                  ,[DateUpgradeSalaryLevel] = @DateUpgradeSalaryLevel
                                  ,[DateChangeSalary] = @DateChangeSalary
                                  ,[NetSalary] = @NetSalary
                                  ,[BasicSalary] = @BasicSalary
                                  ,[SocialSecuritySalary] = @SocialSecuritySalary
                                  ,[IdTypeSalaryScale] = @IdTypeSalaryScale
                                   WHERE Id = @Id";
        public const string QueryGetByIdSalaryNonCoefficient = @"select * from [dbo].[SalaryNonCoefficient] where Id = @Id";
        public const string QueryGetSalaryNonCoefficientByIds = @"select * from [dbo].[SalaryNonCoefficient] where Id IN @Ids";
        public const string QueryDeleteSalaryNonCoefficient = @"Delete from [dbo].[SalaryNonCoefficient] where Id IN @Ids";
        public const string QueryGetAllSalaryNonCoefficient = @"select * from [dbo].[SalaryNonCoefficient] order by CreatedDate";
        public const string QueryGetAllSalaryNonCoefficientAvailable = @"";
        public const string QueryHideSalaryNonCoefficient = @"";
        public const string QueryInsertSalaryNonCoefficientDeleted =
                             @"INSERT INTO [dbo].[Deleted_SalaryNonCoefficient]
                                                       ([Id]
                                                       ,[CreatedDate]
                                                       ,[Status]
                                                       ,[IdEmployee]
                                                       ,[IdSalaryScale]
                                                       ,[IdSalaryLevel]
                                                       ,[DateUpgradeSalaryLevel]
                                                       ,[DateChangeSalary]
                                                       ,[NetSalary]
                                                       ,[BasicSalary]
                                                       ,[SocialSecuritySalary]
                                                       ,[IdTypeSalaryScale])
                                                 VALUES
                                                       (@Id
                                                       ,@CreatedDate
                                                       ,@Status
                                                       ,@IdEmployee
                                                       ,@IdSalaryScale
                                                       ,@IdSalaryLevel
                                                       ,@DateUpgradeSalaryLevel
                                                       ,@DateChangeSalary
                                                       ,@NetSalary
                                                       ,@BasicSalary
                                                       ,@SocialSecuritySalary
                                                       ,@IdTypeSalaryScale)";
    }
}
