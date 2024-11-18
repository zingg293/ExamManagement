using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class ExamDutyRegistrationDetailsSqlQueries
    {
        public const string QueryInsertExamDutyRegistrationDetails = @"
INSERT INTO [dbo].[ExamDutyRegistrationDetails]
      ([Id]
      ,[IdExamScheduleRegistration]
      ,[IdLecturer]
      ,[IdExamShift]
      ,[IsDeleted])
VALUES
      (@Id, @IdExamScheduleRegistration, @IdLecturer, @IdExamShift, @IsDeleted)";

        public const string QueryGetAllExamDutyRegistrationDetails = @"
SELECT * FROM [dbo].[ExamDutyRegistrationDetails] 
ORDER BY IdExamScheduleRegistration";

        public const string QueryGetByIdExamDutyRegistrationDetails = @"
SELECT * FROM [dbo].[ExamDutyRegistrationDetails] 
WHERE Id = @Id";

        public const string QueryUpdateExamDutyRegistrationDetails = @"
UPDATE [dbo].[ExamDutyRegistrationDetails] 
SET IdExamScheduleRegistration = @IdExamScheduleRegistration, 
    IdLecturer = @IdLecturer, 
    IdExamShift = @IdExamShift, 
    IsDeleted = @IsDeleted
WHERE Id = @Id";

        public const string QueryDeleteExamDutyRegistrationDetails = @"
DELETE FROM [dbo].[ExamDutyRegistrationDetails] 
WHERE Id IN @Ids";

    }
}
