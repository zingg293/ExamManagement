using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class AllowancePreviousSalaryInformationSqlQueries
    {
        public const string QueryInsertAllowancePreviousSalaryInformation = @"
INSERT INTO [dbo].[AllowancePreviousSalaryInformation]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdPreviousSalaryInformation]
           ,[IdAllowance])
     VALUES
           (@Id
           ,@CreatedDate
           ,@Status
           ,@IdPreviousSalaryInformation
           ,@IdAllowance)";
        public const string QueryUpdateAllowancePreviousSalaryInformation = @"
            UPDATE [dbo].[AllowancePreviousSalaryInformation]
               SET
                  [CreatedDate] = @CreatedDate,
                  [Status] = @Status,
                  [IdPreviousSalaryInformation] = @IdPreviousSalaryInformation,
                  [IdAllowance] = @IdAllowance
          WHERE Id = @Id";
        public const string QueryGetByIdAllowancePreviousSalaryInformation = @"select * from [dbo].[AllowancePreviousSalaryInformation] where Id = @Id";
        public const string QueryGetAllowancePreviousSalaryInformationByIds = @"select * from [dbo].[AllowancePreviousSalaryInformation] where Id IN @Ids";
        public const string QueryDeleteAllowancePreviousSalaryInformation = @"Delete from [dbo].[AllowancePreviousSalaryInformation] where Id IN @Ids";
        public const string QueryInsertAllowancePreviousSalaryInformationDeleted =
        @"INSERT INTO[dbo].[Deleted_AllowancePreviousSalaryInformation]
                ([Id]
                   ,[CreatedDate]
                   ,[Status]
                   ,[IdPreviousSalaryInformation]
                   ,[IdAllowance])
             VALUES
                   (@Id
                   , @CreatedDate
                   , @Status
                   , @IdPreviousSalaryInformation
                   , @IdAllowance)";
        public const string QueryHideAllowancePreviousSalaryInformation = @"";
        public const string QueryGetAllAllowancePreviousSalaryInformationRepository = @"SELECT * FROM AllowancePreviousSalaryInformation order by CreatedDate";
    }
}
