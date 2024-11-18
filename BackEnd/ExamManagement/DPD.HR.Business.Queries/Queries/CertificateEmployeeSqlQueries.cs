using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class CertificateEmployeeSqlQueries
    {
        public const string QueryInsertCertificateEmployee = @"
INSERT INTO [dbo].[CertificateEmployee]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[CertificateName]
           ,[CertificateType]
           ,[Content]
           ,[IsProfessionalCertificate]
           ,[CertificateNumber]
           ,[Traininginstitution]
           ,[TrainingFormat]
           ,[FromDate]
           ,[ToDate])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @Sort,
           @CertificateName,
           @CertificateType,
           @Content,
           @IsProfessionalCertificate,
           @CertificateNumber,
           @Traininginstitution,
           @TrainingFormat,
           @FromDate,
           @ToDate)";
        public const string QueryUpdateCertificateEmployee = @"
UPDATE [dbo].[CertificateEmployee]
   SET[CreatedDate] = @CreatedDate,
      [Status] = @Status,
      [IdEmployee] = @IdEmployee,
      [Sort] = @Sort,
      [CertificateName] = @CertificateName,
      [CertificateType] = @CertificateType,
      [Content] = @Content,
      [IsProfessionalCertificate] = @IsProfessionalCertificate,
      [CertificateNumber] = @CertificateNumber,
      [Traininginstitution] = @Traininginstitution,
      [TrainingFormat] = @TrainingFormat,
      [FromDate] = @FromDate,
      [ToDate] = @ToDate
          WHERE Id = @Id";
        public const string QueryGetByIdCertificateEmployee = @"select * from [dbo].[CertificateEmployee] where Id = @Id";
        public const string QueryGetCertificateEmployeeByIds = @"select * from [dbo].[CertificateEmployee] where Id IN @Ids";
        public const string QueryDeleteCertificateEmployee = @"Delete from [dbo].[CertificateEmployee] where Id IN @Ids";
        public const string QueryGetAllCertificateEmployeeAvailable = @"SELECT * FROM CertificateEmployee order by CreatedDate";
        public const string QueryGetAllCertificateEmployee = @"SELECT * FROM CertificateEmployee order by CreatedDate";
        public const string QueryHideCertificateEmployee = @"";
        public const string QueryInsertCertificateEmployeeDeleted = 
@"
INSERT INTO[dbo].[Deleted_CertificateEmployee]
        ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[CertificateName]
           ,[CertificateType]
           ,[Content]
           ,[IsProfessionalCertificate]
           ,[CertificateNumber]
           ,[Traininginstitution]
           ,[TrainingFormat]
           ,[FromDate]
           ,[ToDate])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @Sort,
           @CertificateName,
           @CertificateType,
           @Content,
           @IsProfessionalCertificate,
           @CertificateNumber,
           @Traininginstitution,
           @TrainingFormat,
           @FromDate,
           @ToDate)";
    }
}
