using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class ExamSubjectSqlQueries
    {
        public const string QueryInsertExamSubject = @"INSERT INTO [dbo].[ExamSubject]
           ([Id]
           ,[ExamSubjectName]
           ,[IsActive]
           ,[CreatedDate]
           ,[IdExam]
           ,[IdExamType]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @ExamSubjectName,
           @IsActive,
           @CreatedDate,
           @IdExam,
           @IdExamType,
           @IsDeleted)";
        public const string QueryGetAllExamSubject = @"select *from [dbo].[ExamSubject]";
        public const string QueryGetByIdExamSubject = @"select * from [dbo].[ExamSubject] where Id = @Id";
        public const string QueryUpdateExamSubject = @"UPDATE [dbo].[ExamSubject] SET ExamSubjectName = @ExamSubjectName WHERE Id = @Id";
        public const string QueryGetExamSubjectByIds = "select * from [dbo].[ExamSubject] where Id IN @Ids";
        public const string QueryInsertExamSubjectDeleted = @"INSERT INTO [dbo].[Deleted_ExamSubject]
           ([Id]
           ,[ExamSubjectName]
           ,[IsActive]
           ,[CreatedDate]
           ,[IdExam]
           ,[IdExamType]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @ExamSubjectName,
           @IsActive,
           @CreatedDate,
           @IdExam,
           @IdExamType,
           @IsDeleted)";
        public const string QueryDeleteExamSubject = "DELETE FROM [dbo].[ExamSubject] WHERE Id IN @Ids";

    }
}
