using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class TestScheduleSqlQueries
    {
        public const string QueryInsertTestSchedule = @"INSERT INTO [dbo].[TestSchedule]
           ([Id]
           ,[TestScheduleName]
           ,[IsActive]
           ,[CreatedDate]
           ,[IdExam]
           ,[IdExamSubject]
           ,[FromDate]
           ,[ToDate]
           ,[IsDeleted]
           ,[OrganizeFinalExams]
           ,[Note])
     VALUES
           (
           @Id,
           @TestScheduleName,
           @IsActive,
           @CreatedDate,
           @IdExam,
           @IdExamSubject,
           @FromDate,
           @ToDate,
           @IsDeleted,
           @OrganizeFinalExams,
           @Note)";
        public const string QueryGetAllTestSchedule = @"select *from [dbo].[TestSchedule] ";
        public const string QueryGetByIdTestSchedule = @"select * from [dbo].[TestSchedule] where Id = @Id";
        public const string QueryUpdateTestSchedule = @"UPDATE [dbo].[TestSchedule] 
                                                        SET TestScheduleName = @TestScheduleName, 
                                                        IdExam =@IdExam, 
                                                        IdExamSubject = @IdExamSubject,
                                                        FromDate = @FromDate,
                                                        ToDate = @ToDate,
                                                        OrganizeFinalExams = @OrganizeFinalExams,
                                                        Note = @Note,
                                                        WHERE Id = @Id";
        public const string QueryGetTestScheduleByIds = "select * from [dbo].[TestSchedule] where Id IN @Ids";
        public const string QueryInsertTestScheduleDeleted = @"INSERT INTO [dbo].[Deleted_TestSchedule]
           ([Id]
           ,[TestScheduleName]
           ,[IsActive]
           ,[CreatedDate]
           ,[IdExam]
           ,[IdExamSubject]
           ,[FromDate]
           ,[ToDate]
           ,[IsDeleted]
           ,[OrganizeFinalExams]
           ,[Note])
     VALUES
           (
           @Id,
           @TestScheduleName,
           @IsActive,
           @CreatedDate,
           @IdExam,
           @IdExamSubject,
           @FromDate,
           @ToDate,
           @IsDeleted,
           @OrganizeFinalExams,
           @Note);
";
        public const string QueryDeleteTestSchedule = "DELETE FROM [dbo].[TestSchedule] WHERE Id IN @Ids";


    }
}
