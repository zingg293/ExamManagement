using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class ExamShiftSqlQueries
    {
        public const string QueryInsertExamShift = @"
INSERT INTO [dbo].[ExamShift]
      ([Id]
      ,[ExamShiftName]
      ,[TimeStart]
      ,[TimeEnd]
      ,[IsDeleted])
VALUES
      (@Id, @ExamShiftName, @TimeStart, @TimeEnd, @IsDeleted)";

        public const string QueryGetAllExamShift = @"
SELECT * FROM [dbo].[ExamShift] 
ORDER BY ExamShiftName";

        public const string QueryGetByIdExamShift = @"
SELECT * FROM [dbo].[ExamShift] 
WHERE Id = @Id";

        public const string QueryUpdateExamShift = @"
UPDATE [dbo].[ExamShift] 
SET ExamShiftName = @ExamShiftName, 
    TimeStart = @TimeStart, 
    TimeEnd = @TimeEnd, 
    IsDeleted = @IsDeleted
WHERE Id = @Id";

        public const string QueryDeleteExamShift = @"
DELETE FROM [dbo].[ExamShift] 
WHERE Id IN @Ids";

    }
}
