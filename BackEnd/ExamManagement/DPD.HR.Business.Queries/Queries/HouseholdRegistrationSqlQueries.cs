using System.Collections.Generic;
using System.Net;

namespace DPD.HR.Application.Queries.Queries
{
    public static class HouseholdRegistrationSqlQueries
    {
        public const string QueryInsertHouseholdRegistration = @"
                                                                INSERT INTO [dbo].[HouseholdRegistration]
                                                                           ([Id]
                                                                           ,[CreatedDate]
                                                                           ,[Status]
                                                                           ,[FamilyCode]
                                                                           ,[IdEmployee]
                                                                           ,[IdHouseholdRegistrationType]
                                                                           ,[NumberFamilyBook]
                                                                           ,[IsHouseholdHead]
                                                                           ,[NumberPhone]
                                                                           ,[Address]
                                                                           ,[HouseholderRelationship])
                                                                     VALUES
                                                                           (@Id,
                                                                           @CreatedDate,
                                                                           @Status,
                                                                           @FamilyCode,
                                                                           @IdEmployee,
                                                                           @IdHouseholdRegistrationType,
                                                                           @NumberFamilyBook,
                                                                           @IsHouseholdHead,
                                                                           @NumberPhone,
                                                                           @Address,
                                                                           @HouseholderRelationship)";
        public const string QueryUpdateHouseholdRegistration =
           @"UPDATE [dbo].[HouseholdRegistration]
          SET
            [CreatedDate]  = @CreatedDate,
            [Status]  = @Status,
            [FamilyCode]  = @FamilyCode,
            [IdEmployee]  = @IdEmployee,
            [IdHouseholdRegistrationType]  = @IdHouseholdRegistrationType,
            [NumberFamilyBook]  = @NumberFamilyBook,
            [IsHouseholdHead]  = @IsHouseholdHead,
            [NumberPhone]  = @NumberPhone,
            [Address]  = @Address,
            [HouseholderRelationship]  = @HouseholderRelationship
          WHERE Id = @Id";
        public const string QueryGetByIdHouseholdRegistration = @"select * from [dbo].[HouseholdRegistration] where Id = @Id";
        public const string QueryGetHouseholdRegistrationByIds = @"select * from [dbo].[HouseholdRegistration] where Id IN @Ids";
        public const string QueryDeleteHouseholdRegistration = @"Delete from [dbo].[HouseholdRegistration] where Id IN @Ids";
        public const string QueryGetAllHouseholdRegistration = @" SELECT * FROM HouseholdRegistration order by CreatedDate";
        public const string QueryGetAllHouseholdRegistrationAvailable = @"";
        public const string QueryHideHouseholdRegistration = @"";
        public const string QueryInsertHouseholdRegistrationDeleted =
                                                                    @"
                                                                    INSERT INTO[dbo].[Deleted_HouseholdRegistration]
                                                                            ([Id]
                                                                               ,[CreatedDate]
                                                                               ,[Status]
                                                                               ,[FamilyCode]
                                                                               ,[IdEmployee]
                                                                               ,[IdHouseholdRegistrationType]
                                                                               ,[NumberFamilyBook]
                                                                               ,[IsHouseholdHead]
                                                                               ,[NumberPhone]
                                                                               ,[Address]
                                                                               ,[HouseholderRelationship])
                                                                         VALUES
                                                                               (@Id,
                                                                               @CreatedDate,
                                                                               @Status,
                                                                               @FamilyCode,
                                                                               @IdEmployee,
                                                                               @IdHouseholdRegistrationType,
                                                                               @NumberFamilyBook,
                                                                               @IsHouseholdHead,
                                                                               @NumberPhone,
                                                                               @Address,
                                                                               @HouseholderRelationship)";
    }
}
