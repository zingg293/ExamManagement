using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class ExamFormSqlQueries
    {
        public const string QueryInsertExamForm = @"INSERT INTO [dbo].[ExamForm]
           ([Id]
           ,[ExamFormName]
           ,[IsDeleted]
           ,[TeamCode])
     VALUES
           (
           @Id,
           @ExamFormName,
           @IsDeleted,
           @TeamCode)";
        public const string QueryGetAllExamForm = @"select * from [dbo].[ExamForm] ";
        public const string QueryGetByIdExamForm = @"select * from [dbo].[ExamForm] where Id = @Id";
        public const string QueryUpdateExamForm = @"UPDATE [dbo].[ExamForm] SET ExamFormName = @ExamFormName WHERE Id = @Id";
        public const string QueryGetExamFormByIds = "select * from [dbo].[ExamForm] where Id IN @Ids";
        public const string QueryInsertExamFormDeleted = @"INSERT INTO [dbo].[Deleted_ExamForm]
           ([Id]
           ,[ExamFormName]
           ,[IsDeleted]
           ,[TeamCode])
     VALUES
           (
           @Id,
           @ExamFormName,
           @IsDeleted,
           @TeamCode)";
        public const string QueryDeleteExamForm = "DELETE FROM [dbo].[ExamForm] WHERE Id IN @Ids";

    }
}
