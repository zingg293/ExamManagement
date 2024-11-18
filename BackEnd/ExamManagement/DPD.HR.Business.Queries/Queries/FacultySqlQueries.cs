using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class FacultySqlQueries
    {
        public const string QueryInsertFaculty = @"INSERT INTO [dbo].[Faculty]
           ([Id]
           ,[FacultyName]
           ,[Status]
           ,[IsHide]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @FacultyName,
           @Status,
           @IsHide,
           @IsDeleted)";
        public const string QueryGetAllFaculty = @"select * from [dbo].[Faculty] ";
        public const string QueryGetByIdFaculty = @"select * from [dbo].[Faculty] where Id = @Id";
        public const string QueryUpdateFaculty = @"UPDATE [dbo].[Faculty] SET FacultyName = @FacultyName, Status = @Status WHERE Id = @Id";
        public const string QueryGetFacultyByIds = "select * from [dbo].[Faculty] where Id IN @Ids";
        public const string QueryInsertFacultyDeleted = @"INSERT INTO [dbo].[Deleted_Faculty]
            ([Id]
           ,[FacultyName]
           ,[Status]
           ,[IsHide]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @FacultyName,
           @Status,
           @IsHide,
           @IsDeleted)";
        public const string QueryDeleteFaculty = "DELETE FROM [dbo].[Faculty] WHERE Id IN @Ids";

    }
}
