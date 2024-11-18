using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class TrainingSystemSqlQueries
    {
        public const string QueryInsertTrainingSystem = @"INSERT INTO [dbo].[TrainingSystem]
           ([Id]
           ,[TrainingSystemName]
           ,[Status]
           ,[IsHide]
           ,[IdEduProgram]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @TrainingSystemName,
           @Status,
           @IsHide,
           @IdEduProgram,
           @IsDeleted)";
        public const string QueryGetAllTrainingSystem = @"select * from [dbo].[TrainingSystem] ";
        public const string QueryGetByIdTrainingSystem = @"select * from [dbo].[TrainingSystem] where Id = @Id";
        public const string QueryUpdateTrainingSystem = @"UPDATE [dbo].[TrainingSystem] SET TrainingSystemName = @TrainingSystemName, Status = @Status WHERE Id = @Id";
        public const string QueryGetTrainingSystemByIds = "select * from [dbo].[TrainingSystem] where Id IN @Ids";
        public const string QueryInsertTrainingSystemDeleted = @"INSERT INTO [dbo].[Deleted_TrainingSystem]
           ([Id]
           ,[TrainingSystemName]
           ,[Status]
           ,[IsHide]
           ,[IdEduProgram]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @TrainingSystemName,
           @Status,
           @IsHide,
           @IdEduProgram,
           @IsDeleted)";
        public const string QueryDeleteTrainingSystem = "DELETE FROM [dbo].[TrainingSystem] WHERE Id IN @Ids";

    }
}
