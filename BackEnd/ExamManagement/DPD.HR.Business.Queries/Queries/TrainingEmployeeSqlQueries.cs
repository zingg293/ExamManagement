using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class TrainingEmployeeSqlQueries
    {
        public const string QueryInsertTrainingEmployee = @"
INSERT INTO [dbo].[TrainingEmployee]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[NameTrainingInstitution]
           ,[Major]
           ,[FromDate]
           ,[ToDate]
           ,[TrainingType]
           ,[Certificate])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @Sort,
           @NameTrainingInstitution,
           @Major,
           @FromDate,
           @ToDate,
           @TrainingType,
           @Certificate)";
        public const string QueryUpdateTrainingEmployee = @"
UPDATE [dbo].[TrainingEmployee]
   SET
      [CreatedDate] = @CreatedDate,
      [Status] = @Status,
      [IdEmployee] = @IdEmployee,
      [Sort] = @Sort,
      [NameTrainingInstitution] = @NameTrainingInstitution,
      [Major] = @Major,
      [FromDate] = @FromDate,
      [ToDate] = @ToDate,
      [TrainingType] = @TrainingType,
      [Certificate] = @Certificate
          WHERE Id = @Id";
        public const string QueryGetByIdTrainingEmployee = @"select * from [dbo].[TrainingEmployee] where Id = @Id";
        public const string QueryGetTrainingEmployeeByIds = @"select * from [dbo].[TrainingEmployee] where Id IN @Ids";
        public const string QueryDeleteTrainingEmployee = @"Delete from [dbo].[TrainingEmployee] where Id IN @Ids";
        public const string QueryGetAllTrainingEmployee = @"SELECT * FROM TrainingEmployee order by CreatedDate";
        public const string QueryHideTrainingEmployee = @"";
        public const string QueryGetAllTrainingEmployeeAvailable = @"";
        public const string QueryInsertTrainingEmployeeDeleted =
         @"
INSERT INTO[dbo].[Deleted_TrainingEmployee]
        ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[NameTrainingInstitution]
           ,[Major]
           ,[FromDate]
           ,[ToDate]
           ,[TrainingType]
           ,[Certificate])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @Sort,
           @NameTrainingInstitution,
           @Major,
           @FromDate,
           @ToDate,
           @TrainingType,
           @Certificate)";
    }
}
