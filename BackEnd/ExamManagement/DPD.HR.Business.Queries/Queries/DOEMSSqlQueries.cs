using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class DOEMSSqlQueries
    {
        public const string QueryInsertDOEMS = @"INSERT INTO [dbo].[DOEMS]
           ([Id]
           ,[IsActive]
           ,[CreatedDate]
           ,[IdEMS]
           ,[IdLecturer]
           ,[IdRoom]
           ,[FromDate]
           ,[ToDate]
           ,[IdStudyGroup]
           ,[IdDOTS]
           ,[IsDeleted]
           ,[DoUseLabRoom]
           ,[Note]
           ,[IdTeamcode])
     VALUES
           (
           @Id,
           @IsActive,
           @CreatedDate,
           @IdEMS,
           @IdLecturer,
           @IdRoom,
           @FromDate,
           @ToDate,
           @IdStudy)";
        public const string QueryGetAllDOEMS = @"select * from [dbo].[DOEMS]";
        public const string QueryGetByIdDOEMS = @"select * from [dbo].[DOEMS] where Id = @Id";
        public const string QueryUpdateDOEMS = @"UPDATE [dbo].[DOEMS] SET IdLecturer = @IdLecturer WHERE Id = @Id";
        public const string QueryGetDOEMSByIds = "select * from [dbo].[DOEMS] where Id IN @Ids";
        public const string QueryInsertDOEMSDeleted = @"INSERT INTO [dbo].[Deleted_DOEMS]
           ([Id]
           ,[IsActive]
           ,[CreatedDate]
           ,[IdEMS]
           ,[IdLecturer]
           ,[IdRoom]
           ,[FromDate]
           ,[ToDate]
           ,[IdStudyGroup]
           ,[IdDOTS]
           ,[IsDeleted]
           ,[DoUseLabRoom]
           ,[Note]
           ,[IdTeamcode])
     VALUES
           (
           @Id,
           @IsActive,
           @CreatedDate,
           @IdEMS,
           @IdLecturer,
           @IdRoom,
           @FromDate,
           @ToDate,
           @IdStudy)";
        public const string QueryDeleteDOEMS = "DELETE FROM [dbo].[DOEMS] WHERE Id IN @Ids";


    }
}
