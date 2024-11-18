using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class EducationalBackgroundSqlQueries
    {
        public const string QueryInsertEducationalBackground = @"
INSERT INTO [dbo].[EducationalBackground]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[TrainingInstitution]
           ,[IdEducationalDegree]
           ,[FromDate]
           ,[ToDate]
           ,[GraduationDate]
           ,[TrainingFormat]
           ,[Degree]
           ,[Major]
           ,[GraduationResult]
           ,[CertificateNumber]
           ,[EffectiveDate]
           ,[IsMain]
           ,[VerificationDocumentNumber]
           ,[DateIssuanceReplyDocument]
           ,[AccreditedDegreeGrantingInstitution]
           ,[GraduationInstitution]
           ,[SendingOrganizationForEducation]
           ,[IsProfessionalCertificate]
           ,[CertificateIssuer])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @Sort,
           @TrainingInstitution,
           @IdEducationalDegree,
           @FromDate,
           @ToDate,
           @GraduationDate,
           @TrainingFormat,
           @Degree,
           @Major,
           @GraduationResult,
           @CertificateNumber,
           @EffectiveDate,
           @IsMain,
           @VerificationDocumentNumber,
           @DateIssuanceReplyDocument,
           @AccreditedDegreeGrantingInstitution,
           @GraduationInstitution,
           @SendingOrganizationForEducation,
           @IsProfessionalCertificate,
           @CertificateIssuer)";
        public const string QueryUpdateEducationalBackground = @"UPDATE [dbo].[EducationalBackground]
                                   SET
                                       [CreatedDate] = @CreatedDate,
                                      [Status] = @Status,
                                      [IdEmployee] = @IdEmployee,
                                      [Sort] = @Sort,
                                      [TrainingInstitution] = @TrainingInstitution,
                                      [IdEducationalDegree] = @IdEducationalDegree,
                                      [FromDate] = @FromDate,
                                      [ToDate] = @ToDate,
                                      [GraduationDate] = @GraduationDate,
                                      [TrainingFormat] = @TrainingFormat,
                                      [Degree] = @Degree,
                                      [Major] = @Major,
                                      [GraduationResult] = @GraduationResult,
                                      [CertificateNumber] = @CertificateNumber,
                                      [EffectiveDate] = @EffectiveDate,
                                      [IsMain] = @IsMain,
                                      [VerificationDocumentNumber] = @VerificationDocumentNumber,
                                      [DateIssuanceReplyDocument] = @DateIssuanceReplyDocument,
                                      [AccreditedDegreeGrantingInstitution] = @AccreditedDegreeGrantingInstitution,
                                      [GraduationInstitution] = @GraduationInstitution,
                                      [SendingOrganizationForEducation] = @SendingOrganizationForEducation,
                                      [IsProfessionalCertificate] = @IsProfessionalCertificate,
                                      [CertificateIssuer] = @CertificateIssuer
                                 WHERE [Id] = @Id";
        public const string QueryGetByIdEducationalBackground = @"select * from [dbo].[EducationalBackground] where Id = @Id";
        public const string QueryGetEducationalBackgroundByIds = @"select * from [dbo].[EducationalBackground] where Id IN @Ids";
        public const string QueryDeleteEducationalBackground = @"Delete from [dbo].[EducationalBackground] where Id IN @Ids";
        public const string QueryGetAllEducationalBackground = @"SELECT * FROM EducationalBackground order by CreatedDate";
        public const string QueryGetAllEducationalBackgroundAvailable = @"SELECT * FROM EducationalBackground order by CreatedDate";
        public const string QueryHideEducationalBackground = @"";
        public const string QueryInsertEducationalBackgroundDeleted =
         @"
INSERT INTO[dbo].[Deleted_EducationalBackground]
        ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[TrainingInstitution]
           ,[IdEducationalDegree]
           ,[FromDate]
           ,[ToDate]
           ,[GraduationDate]
           ,[TrainingFormat]
           ,[Degree]
           ,[Major]
           ,[GraduationResult]
           ,[CertificateNumber]
           ,[EffectiveDate]
           ,[IsMain]
           ,[VerificationDocumentNumber]
           ,[DateIssuanceReplyDocument]
           ,[AccreditedDegreeGrantingInstitution]
           ,[GraduationInstitution]
           ,[SendingOrganizationForEducation]
           ,[IsProfessionalCertificate]
           ,[CertificateIssuer])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @Sort,
           @TrainingInstitution,
           @IdEducationalDegree,
           @FromDate,
           @ToDate,
           @GraduationDate,
           @TrainingFormat,
           @Degree,
           @Major,
           @GraduationResult,
           @CertificateNumber,
           @EffectiveDate,
           @IsMain,
           @VerificationDocumentNumber,
           @DateIssuanceReplyDocument,
           @AccreditedDegreeGrantingInstitution,
           @GraduationInstitution,
           @SendingOrganizationForEducation,
           @IsProfessionalCertificate,
           @CertificateIssuer)";
    }
}
