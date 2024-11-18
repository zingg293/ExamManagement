using System.Collections.Generic;
using System.Xml.Linq;

namespace DPD.HR.Application.Queries.Queries
{
    public static class HouseholdRegistrationTypeSqlQueries
    {
        public const string QueryInsertHouseholdRegistrationType = @"
INSERT INTO [dbo].[HouseholdRegistrationType]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Name])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @Name)";
        public const string QueryUpdateHouseholdRegistrationType = 
          @"UPDATE [dbo].[HouseholdRegistrationType]
          SET
          [CreatedDate]= @CreatedDate,
          [Status]= @Status
          WHERE Id = @Id";
        public const string QueryGetByIdHouseholdRegistrationType = @"select * from [dbo].[HouseholdRegistrationType] where Id = @Id";
        public const string QueryGetHouseholdRegistrationTypeByIds = @"select * from [dbo].[HouseholdRegistrationType] where Id IN @Ids";
        public const string QueryDeleteHouseholdRegistrationType = @"Delete from [dbo].[HouseholdRegistrationType] where Id IN @Ids";
        public const string QueryGetAllHouseholdRegistrationType = @"SELECT * FROM HouseholdRegistrationType order by CreatedDate";
        public const string QueryGetAllHouseholdRegistrationTypeAvailable = @"";
        public const string QueryHideHouseholdRegistrationType = @"";
        public const string QueryInsertHouseholdRegistrationTypeDeleted =
          @"
INSERT INTO[dbo].[Deleted_HouseholdRegistrationType]
        ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Name])
     VALUES
        (@Id,
           @CreatedDate,
           @Status,
           @Name)";
    }
}
