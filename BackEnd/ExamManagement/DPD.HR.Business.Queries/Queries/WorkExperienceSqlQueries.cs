using System.Collections.Generic;
using System.Net;

namespace DPD.HR.Application.Queries.Queries
{
    public static class WorkExperienceSqlQueries
    {
        public const string QueryInsertWorkExperience = @"
INSERT INTO [dbo].[WorkExperience]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[UnitName]
           ,[Address]
           ,[JobTitle]
           ,[FromDate]
           ,[ToDate]
           ,[Job]
           ,[Reference]
           ,[PhoneReference])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @Sort,
           @UnitName,
           @Address,
           @JobTitle,
           @FromDate,
           @ToDate,
           @Job,
           @Reference,
           @PhoneReference)";
        public const string QueryUpdateWorkExperience = @"
UPDATE [dbo].[WorkExperience]
   SET [CreatedDate] = @CreatedDate,
      [Status] = @Status,
      [IdEmployee] = @IdEmployee,
      [Sort] = @Sort,
      [UnitName] = @UnitName,
      [Address] = @Address,
      [JobTitle] = @JobTitle,
      [FromDate] = @FromDate,
      [ToDate] = @ToDate,
      [Job] = @Job,
      [Reference] = @Reference,
      [PhoneReference] = @PhoneReference
          WHERE Id = @Id";
        public const string QueryGetByIdWorkExperience = @"select * from [dbo].[WorkExperience] where Id = @Id";
        public const string QueryGetWorkExperienceByIds = @"select * from [dbo].[WorkExperience] where Id IN @Ids";
        public const string QueryDeleteWorkExperience = @"Delete from [dbo].[WorkExperience] where Id IN @Ids";
        public const string QueryGetAllWorkExperience = @"SELECT * FROM WorkExperience order by CreatedDate";
        public const string QueryGetAllWorkExperienceAvailable = @"";
        public const string QueryHideWorkExperience = @"";
        public const string QueryInsertWorkExperienceDeleted = 
         @"
INSERT INTO[dbo].[Deleted_WorkExperience]
        ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[UnitName]
           ,[Address]
           ,[JobTitle]
           ,[FromDate]
           ,[ToDate]
           ,[Job]
           ,[Reference]
           ,[PhoneReference])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @Sort,
           @UnitName,
           @Address,
           @JobTitle,
           @FromDate,
           @ToDate,
           @Job,
           @Reference,
           @PhoneReference)";
    }
}
