using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class ExaminationSqlQueries
    {
        public const string QueryInsertExamination = @"INSERT INTO [dbo].[Examination]
           ([Id]
           ,[ExamName]
           ,[IsActive]
           ,[CreatedDate]
           ,[IdExamType]
           ,[SchoolYear]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @ExamName,
           @IsActive,
           @CreatedDate,
           @IdExamType,
           @SchoolYear,
           @IsDeleted)";
        public const string QueryGetAllExamination = @"select *from [dbo].[Examination]";
        public const string QueryGetByIdExamination = @"select * from [dbo].[Examination] where Id = @Id";
        public const string QueryUpdateExamination = @"UPDATE [dbo].[Examination] SET ExamName = @ExamName, SchoolYear = @SchoolYear  WHERE Id = @Id";
        public const string QueryGetExaminationByIds = "select * from [dbo].[Examination] where Id IN @Ids";
        public const string QueryInsertExaminationDeleted = @"INSERT INTO [dbo].[Deleted_Examination]
           ([Id]
           ,[ExamName]
           ,[IsActive]
           ,[CreatedDate]
           ,[IdExamType]
           ,[SchoolYear]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @ExamName,
           @IsActive,
           @CreatedDate,
           @IdExamType,
           @SchoolYear,
           @IsDeleted)
";
        public const string QueryDeleteExamination = "DELETE FROM [dbo].[Examination] WHERE Id IN @Ids";

    }
}
