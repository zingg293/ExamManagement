using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class PreviousSalaryInformationSqlQueries
    {
        public const string QueryInsertPreviousSalaryInformation = @"
INSERT INTO [dbo].[PreviousSalaryInformation]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IsCoefficient]
           ,[IdEmployee]
           ,[IdSalaryScale]
           ,[IdSalaryLevel]
           ,[IdTypeSalaryScale]
           ,[DateUpgradeSalaryLevel]
           ,[DateChangeSalary]
           ,[SocialSecuritySalary]
           ,[SocialSecurityContributionRate])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IsCoefficient,
           @IdEmployee,
           @IdSalaryScale,
           @IdSalaryLevel,
           @IdTypeSalaryScale,
           @DateUpgradeSalaryLevel,
           @DateChangeSalary,
           @SocialSecuritySalary,
           @SocialSecurityContributionRate)";
        public const string QueryUpdatePreviousSalaryInformation = @"
UPDATE [dbo].[PreviousSalaryInformation]
   SET
      [CreatedDate] = @CreatedDate,
      [Status] = @Status,
      [IsCoefficient] = @IsCoefficient,
      [IdEmployee] = @IdEmployee,
      [IdSalaryScale] = @IdSalaryScale,
      [IdSalaryLevel] = @IdSalaryLevel,
      [IdTypeSalaryScale] = @IdTypeSalaryScale,
      [DateUpgradeSalaryLevel] = @DateUpgradeSalaryLevel,
      [DateChangeSalary] = @DateChangeSalary,
      [SocialSecuritySalary] = @SocialSecuritySalary,
      [SocialSecurityContributionRate] = @SocialSecurityContributionRate
          WHERE Id = @Id";
        public const string QueryGetByIdPreviousSalaryInformation = @"select * from [dbo].[PreviousSalaryInformation] where Id = @Id";
        public const string QueryGetPreviousSalaryInformationByIds = @"select * from [dbo].[PreviousSalaryInformation] where Id IN @Ids";
        public const string QueryDeletePreviousSalaryInformation = @"Delete from [dbo].[PreviousSalaryInformation] where Id IN @Ids";
        public const string QueryGetAllPreviousSalaryInformation = @"SELECT * FROM PreviousSalaryInformation order by CreatedDate";
        public const string QueryGetAllPreviousSalaryInformationAvailable = @"";
        public const string QueryHidePreviousSalaryInformation = @"";
        public const string QueryInsertPreviousSalaryInformationDeleted = 
           @"
INSERT INTO[dbo].[Deleted_PreviousSalaryInformation]
        ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IsCoefficient]
           ,[IdEmployee]
           ,[IdSalaryScale]
           ,[IdSalaryLevel]
           ,[IdTypeSalaryScale]
           ,[DateUpgradeSalaryLevel]
           ,[DateChangeSalary]
           ,[SocialSecuritySalary]
           ,[SocialSecurityContributionRate])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IsCoefficient,
           @IdEmployee,
           @IdSalaryScale,
           @IdSalaryLevel,
           @IdTypeSalaryScale,
           @DateUpgradeSalaryLevel,
           @DateChangeSalary,
           @SocialSecuritySalary,
           @SocialSecurityContributionRate)";
    }
}
