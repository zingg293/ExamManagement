using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class EducationProgramSqlQueries
    {
        public const string QueryInsertEducationProgram = @"INSERT INTO [dbo].[EducationProgram]
           ([Id]
           ,[EducationProgramName]
           ,[Status]
           ,[IsHide]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @EducationProgramName,
           @Status,
           @IsHide,
           @IsDeleted)";
        public const string QueryGetAllEducationProgram = @"select * from [dbo].[EducationProgram] ";
        public const string QueryGetByIdEducationProgram = @"select * from [dbo].[EducationProgram] where Id = @Id";
        public const string QueryUpdateEducationProgram = @"UPDATE [dbo].[EducationProgram] SET EducationProgramName = @EducationProgramName, Status = @Status WHERE Id = @Id";
        public const string QueryGetEducationProgramByIds = "select * from [dbo].[EducationProgram] where Id IN @Ids";
        public const string QueryInsertEducationProgramDeleted = @"INSERT INTO [dbo].[Deleted_EducationProgram]
           ([Id]
           ,[EducationProgramName]
           ,[Status]
           ,[IsHide]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @EducationProgramName,
           @Status,
           @IsHide,
           @IsDeleted)";
        public const string QueryDeleteEducationProgram = "DELETE FROM [dbo].[EducationProgram] WHERE Id IN @Ids";


    }
}
