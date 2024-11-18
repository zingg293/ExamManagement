using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
   public class ExamScheduleRegistrationSqlQueries
    {
        public const string QueryInsertExamScheduleRegistration = @"
INSERT INTO [dbo].[ExamScheduleRegistration]
      ([Id]
      ,[IsActive]
      ,[CreatedDate]
      ,[IdEMS]
      ,[IdStudyGroup]
      ,[IdDOTS]
      ,[NumberOfRegistrations]
      ,[IsDeleted])
VALUES
      (@Id, @IsActive, @CreatedDate, @IdEMS, @IdStudyGroup, @IdDOTS, @NumberOfRegistrations, @IsDeleted)";

        public const string QueryGetAllExamScheduleRegistration = @"
SELECT * FROM [dbo].[ExamScheduleRegistration] 
ORDER BY CreatedDate";

        public const string QueryGetByIdExamScheduleRegistration = @"
SELECT * FROM [dbo].[ExamScheduleRegistration] 
WHERE Id = @Id";

        public const string QueryUpdateExamScheduleRegistration = @"
UPDATE [dbo].[ExamScheduleRegistration] 
SET IsActive = @IsActive, 
    IdEMS = @IdEMS, 
    IdStudyGroup = @IdStudyGroup, 
    IdDOTS = @IdDOTS, 
    NumberOfRegistrations = @NumberOfRegistrations,
    IsDeleted = @IsDeleted
WHERE Id = @Id";

        public const string QueryDeleteExamScheduleRegistration = @"
DELETE FROM [dbo].[ExamScheduleRegistration] 
WHERE Id IN @Ids";

    }
}
