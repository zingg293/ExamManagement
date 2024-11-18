using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class ExaminationTypeSqlQueries
    {
        public const string QueryInsertExaminationType = @"INSERT INTO [dbo].[ExaminationType]
           ([Id]
           ,[ExamTypeName]
           ,[IsActive]
           ,[CreatedDate]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @ExamTypeName,
           @IsActive,
           @CreatedDate,
           @IsDeleted)";
        public const string QueryGetAllExaminationType = @"select * from [dbo].[ExaminationType] ";
        public const string QueryGetByIdExaminationType = @"select * from [dbo].[ExaminationType] where Id = @Id";
        public const string QueryUpdateExaminationType = @"UPDATE [dbo].[ExaminationType] SET ExamTypeName = @ExamTypeName, IsActive = @IsActive WHERE Id = @Id";
        public const string QueryGetExaminationTypeByIds = "select * from [dbo].[ExaminationType] where Id IN @Ids";
        public const string QueryInsertExaminationTypeDeleted = @"INSERT INTO [dbo].[Deleted_ExaminationType]
           ([Id]
           ,[ExamTypeName]
           ,[IsActive]
           ,[CreatedDate]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @ExamTypeName,
           @IsActive,
           @CreatedDate,
           @IsDeleted)";
        public const string QueryDeleteExaminationType = "DELETE FROM [dbo].[ExaminationType] WHERE Id IN @Ids";



    }
}
