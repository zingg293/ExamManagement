using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class EMSSqlQueries
    {
        public const string QueryInsertEMS = @"INSERT INTO [dbo].[EMS]
           ([Id]
           ,[EMSName]
           ,[IsActive]
           ,[CreatedDate]
           ,[IdExam]
           ,[IdExamSubject]
           ,[IdTestSchedule]
           ,[NumberOfStudents]
           ,[NumberOfLecturers]
           ,[IdStudyGroup]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @EMSName,
           @IsActive,
           @CreatedDate,
           @IdExam,
           @IdExamSubject,
           @IdTestSchedule,
           @NumberOfStudents,
           @NumberOfLecturers,
           @IdStudyGroup,
           @IsDeleted)";
        public const string QueryGetAllEMS = @"select *from [dbo].[EMS]";
        public const string QueryGetByIdEMS = @"select * from [dbo].[EMS] where Id = @Id";
        public const string QueryUpdateEMS = @"UPDATE [dbo].[EMS]
                                                SET 
                                                EMSName = @EMSName, 
                                                IsActive = @IsActive, 
                                                IdExam = @IdExam, 
                                                IdExamSubject = @IdExamSubject, 
                                                IdTestSchedule = @IdTestSchedule, 
                                                NumberOfStudents = @NumberOfStudents, 
                                                NumberOfLecturers = @NumberOfLecturers
                                                WHERE Id = @Id";
        public const string QueryGetEMSByIds = "select * from [dbo].[EMS] where Id IN @Ids";
        public const string QueryInsertEMSDeleted = @"INSERT INTO [dbo].[Deleted_EMS]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[NameNationality])
     VALUES
       (@Id,@CreatedDate,@Status,@NameNationality)";
        public const string QueryDeleteEMS = "DELETE FROM [dbo].[EMS] WHERE Id IN @Ids";

    }
}
