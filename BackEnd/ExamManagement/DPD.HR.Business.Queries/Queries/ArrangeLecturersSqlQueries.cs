using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class ArrangeLecturersSqlQueries
    {
        public const string QueryInsertArrangeLecturers = @"INSERT INTO [dbo].[ArrangeLecturers]
           ([Id]
           ,[IdLecturer]
           ,[IdStudyGroup]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @IdLecturer,
           @IdStudyGroup,
           @IsDeleted)";
        public const string QueryGetAllArrangeLecturers = @"select * from [dbo].[ArrangeLecturers] ";
        public const string QueryGetByIdArrangeLecturers = @"select * from [dbo].[ArrangeLecturers] where Id = @Id";
        public const string QueryUpdateArrangeLecturers = @"UPDATE [dbo].[ArrangeLecturers] SET IdLecturer = @IdLecturer, IdStudyGroup = @IdStudyGroup WHERE Id = @Id";
        public const string QueryGetArrangeLecturersByIds = "select * from [dbo].[ArrangeLecturers] where Id IN @Ids";
        public const string QueryInsertArrangeLecturersDeleted = @"IINSERT INTO [dbo].[Deleted_ArrangeLecturers]
           ([Id]
           ,[IdLecturer]
           ,[IdStudyGroup]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @IdLecturer,
           @IdStudyGroup,
           @IsDeleted)";
        public const string QueryDeleteArrangeLecturers = "DELETE FROM [dbo].[ArrangeLecturers] WHERE Id IN @Ids";

    }
}
