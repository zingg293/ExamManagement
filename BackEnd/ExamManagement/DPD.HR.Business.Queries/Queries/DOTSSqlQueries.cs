using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class DOTSSqlQueries
    {
        public const string QueryInsertDOTS = @"INSERT INTO [dbo].[DOTS]
           ([Id]
           ,[IdExam]
           ,[IdExamSubject]
           ,[IdTestSchedule]
           ,[IdExamForm]
           ,[ExamTime]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @IdExam,
           @IdExamSubject,
           @IdTestSchedule,
           @IdExamForm,
           @ExamTime,
           @IsDeleted)";
        public const string QueryGetAllDOTS = @"select * from [dbo].[DOTS]";
        public const string QueryGetByIdDOTS = @"select * from [dbo].[DOTS] where Id = @Id";
        public const string QueryUpdateDOTS = @"UPDATE [dbo].[DOTS] SET IdExamForm = @IdExamForm, ExamTime = @ExamTime WHERE Id = @Id";
        public const string QueryGetDOTSByIds = "select * from [dbo].[DOTS] where Id IN @Ids";
        public const string QueryInsertDOTSDeleted = @"INSERT INTO [dbo].[Deleted_DOTS]
           ([Id]
           ,[IdExam]
           ,[IdExamSubject]
           ,[IdTestSchedule]
           ,[IdExamForm]
           ,[ExamTime]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @IdExam,
           @IdExamSubject,
           @IdTestSchedule,
           @IdExamForm,
           @ExamTime,
           @IsDeleted)";
        public const string QueryDeleteDOTS = "DELETE FROM [dbo].[DOTS] WHERE Id IN @Ids";

    }
}
