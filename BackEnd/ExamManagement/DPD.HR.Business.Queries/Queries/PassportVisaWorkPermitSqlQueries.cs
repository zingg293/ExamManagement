using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class PassportVisaWorkPermitSqlQueries
    {
        public const string QueryInsertPassportVisaWorkPermit = @"
INSERT INTO [dbo].[PassportVisaWorkPermit]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[NumberVisa]
           ,[NumberPassport]
           ,[NumberWorkPermit]
           ,[EffectiveDateVisa]
           ,[ExpirerationDateVisa]
           ,[EffectiveDatePassport]
           ,[ExpirerationDatePassport]
           ,[EffectiveDateWorkPermit]
           ,[ExpirerationDateWorkPermit]
           ,[PlaceOfIssueVisa]
           ,[PlaceOfIssuePassport]
           ,[PlaceOfIssueWorkPermit])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @NumberVisa,
           @NumberPassport,
           @NumberWorkPermit,
           @EffectiveDateVisa,
           @ExpirerationDateVisa,
           @EffectiveDatePassport,
           @ExpirerationDatePassport,
           @EffectiveDateWorkPermit,
           @ExpirerationDateWorkPermit,
           @PlaceOfIssueVisa,
           @PlaceOfIssuePassport,
           @PlaceOfIssueWorkPermit)";
        public const string QueryUpdatePassportVisaWorkPermit = @"UPDATE [dbo].[PassportVisaWorkPermit]
          SET
            [CreatedDate]  = @CreatedDate,
            [Status]  = @Status,
            [IdEmployee]  = @IdEmployee,
            [NumberVisa]  = @NumberVisa,
            [NumberPassport]  = @NumberPassport,
            [NumberWorkPermit]  = @NumberWorkPermit,
            [EffectiveDateVisa]  = @EffectiveDateVisa,
            [ExpirerationDateVisa]  = @ExpirerationDateVisa,
            [EffectiveDatePassport]  = @EffectiveDatePassport,
            [ExpirerationDatePassport]  = @ExpirerationDatePassport,
            [EffectiveDateWorkPermit]  = @EffectiveDateWorkPermit,
            [ExpirerationDateWorkPermit]  = @ExpirerationDateWorkPermit,
            [PlaceOfIssueVisa]  = @PlaceOfIssueVisa,
            [PlaceOfIssuePassport]  = @PlaceOfIssuePassport,
            [PlaceOfIssueWorkPermit]  = @PlaceOfIssueWorkPermit                                                             
          WHERE Id = @Id";
        public const string QueryGetByIdPassportVisaWorkPermit = @"select * from [dbo].[PassportVisaWorkPermit] where Id = @Id";
        public const string QueryGetPassportVisaWorkPermitByIds = @"select * from [dbo].[PassportVisaWorkPermit] where Id IN @Ids";
        public const string QueryDeletePassportVisaWorkPermit = @"Delete from [dbo].[PassportVisaWorkPermit] where Id IN @Ids";
        public const string QueryGetAllPassportVisaWorkPermit = @"SELECT * FROM PassportVisaWorkPermit order by CreatedDate";
        public const string QueryGetAllPassportVisaWorkPermitAvailable = @"SELECT * FROM PassportVisaWorkPermit order by CreatedDate";
        public const string QueryHidePassportVisaWorkPermit = @"";
        public const string QueryInsertPassportVisaWorkPermitDeleted = 
                                                          @"INSERT INTO[dbo].[Deleted_PassportVisaWorkPermit]
                                                        ([Id]
                                                           ,[CreatedDate]
                                                           ,[Status]
                                                           ,[IdEmployee]
                                                           ,[NumberVisa]
                                                           ,[NumberPassport]
                                                           ,[NumberWorkPermit]
                                                           ,[EffectiveDateVisa]
                                                           ,[ExpirerationDateVisa]
                                                           ,[EffectiveDatePassport]
                                                           ,[ExpirerationDatePassport]
                                                           ,[EffectiveDateWorkPermit]
                                                           ,[ExpirerationDateWorkPermit]
                                                           ,[PlaceOfIssueVisa]
                                                           ,[PlaceOfIssuePassport]
                                                           ,[PlaceOfIssueWorkPermit])
                                                     VALUES
                                                           (@Id,
                                                           @CreatedDate,
                                                           @Status,
                                                           @IdEmployee,
                                                           @NumberVisa,
                                                           @NumberPassport,
                                                           @NumberWorkPermit,
                                                           @EffectiveDateVisa,
                                                           @ExpirerationDateVisa,
                                                           @EffectiveDatePassport,
                                                           @ExpirerationDatePassport,
                                                           @EffectiveDateWorkPermit,
                                                           @ExpirerationDateWorkPermit,
                                                           @PlaceOfIssueVisa,
                                                           @PlaceOfIssuePassport,
                                                           @PlaceOfIssueWorkPermit)";
    }
}
