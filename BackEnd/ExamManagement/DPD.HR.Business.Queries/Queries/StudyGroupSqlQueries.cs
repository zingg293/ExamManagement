using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class StudyGroupSqlQueries
    {
        public const string QueryInsertStudyGroup = @"INSERT INTO[dbo].[StudyGroup]
        ([Id]
           ,[IdExamSubject]
           ,[Status]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @IdExamSubject,
           @Status,
           @IsDeleted)";
        public const string QueryGetAllStudyGroup = @"select * from [dbo].[StudyGroup] ";
        public const string QueryGetByIdStudyGroup = @"select * from [dbo].[StudyGroup] where Id = @Id";
        public const string QueryUpdateStudyGroup = @"UPDATE [dbo].[StudyGroup] SET IdExamSubject = @IdExamSubject, Status = @Status WHERE Id = @Id";
        public const string QueryGetStudyGroupByIds = "select * from [dbo].[StudyGroup] where Id IN @Ids";
        public const string QueryInsertStudyGroupDeleted = @"INSERT INTO [dbo].[Deleted_StudyGroup]
           ([Id]
           ,[IdExamSubject]
           ,[Status]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @IdExamSubject,
           @Status,
           @IsDeleted)";
        public const string QueryDeleteStudyGroup = "DELETE FROM [dbo].[StudyGroup] WHERE Id IN @Ids";


    }
}
