
namespace DPD.HR.Application.Queries.Queries
{
    public static class CurriculumViteaSqlQueries
    {
        public const string QueryInsertCurriculumVitae = @"INSERT INTO [dbo].[CurriculumVitae]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[SocialSecurityNumber]
           ,[AnotherName]
           ,[CitizenCardNumber]
           ,[CitizenCardFile]
           ,[Religion]
           ,[Ethnicity]
           ,[IdNationality]
           ,[PlaceOfBirth]
           ,[Award]
           ,[DisabledVeteranRank]
           ,[HealthStatus]
           ,[IdPolicybeneficiary]
           ,[Height]
           ,[Weight]
           ,[BloodType]
           ,[IdProfessionalQualification]
           ,[PoliticalReasoning]
           ,[IdGovernmentManagement]
           ,[GovernmentAward]
           ,[DisciplinaryAction]
           ,[Profession]
           ,[PrimaryJob]
           ,[ForteOfWork]
           ,[TitleConcurrent]
           ,[SpecializedProfessionalQualifications]
           ,[AcademicDegree]
           ,[AcademicDegreeDate]
           ,[GovernmentOfficialNumber]
           ,[GeneralEducationLevel])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @SocialSecurityNumber,
           @AnotherName,
           @CitizenCardNumber,
           @CitizenCardFile,
           @Religion,
           @Ethnicity,
           @IdNationality,
           @PlaceOfBirth,
           @Award,
           @DisabledVeteranRank,
           @HealthStatus,
           @IdPolicybeneficiary,
           @Height,
           @Weight,
           @BloodType,
           @IdProfessionalQualification,
           @PoliticalReasoning,
           @IdGovernmentManagement,
           @GovernmentAward,
           @DisciplinaryAction,
           @Profession,
           @PrimaryJob,
           @ForteOfWork,
           @TitleConcurrent,
           @SpecializedProfessionalQualifications,
           @AcademicDegree,
           @AcademicDegreeDate,
           @GovernmentOfficialNumber,
           @GeneralEducationLevel)";
        public const string QueryUpdateCurriculumVitae = @"UPDATE [dbo].[CurriculumVitae]
          SET
          [CreatedDate]= @CreatedDate,
          [Status]= @Status, 
          [IdEmployee]= @IdEmployee,
          [SocialSecurityNumber]= @SocialSecurityNumber, 
          [AnotherName]= @AnotherName, 
          [CitizenCardNumber]= @CitizenCardNumber, 
          [CitizenCardFile]= @CitizenCardFile, 
          [Religion]= @Religion, 
          [Ethnicity]= @Ethnicity, 
          [IdNationality]= @IdNationality,
          [PlaceOfBirth]= @PlaceOfBirth, 
          [Award]= @Award, 
          [DisabledVeteranRank]= @DisabledVeteranRank, 
          [HealthStatus]= @HealthStatus, 
          [IdPolicybeneficiary]= @IdPolicybeneficiary,
          [Height]= @Height, 
          [Weight]= @Weight, 
          [BloodType]= @BloodType, 
          [IdProfessionalQualification]= @IdProfessionalQualification,
          [PoliticalReasoning]= @PoliticalReasoning, 
          [IdGovernmentManagement]= @IdGovernmentManagement,
          [GovernmentAward]= @GovernmentAward, 
          [DisciplinaryAction]= @DisciplinaryAction, 
          [Profession]= @Profession, 
          [PrimaryJob]= @PrimaryJob, 
          [ForteOfWork]= @ForteOfWork, 
          [TitleConcurrent]= @TitleConcurrent, 
          [SpecializedProfessionalQualifications]= @SpecializedProfessionalQualifications, 
          [AcademicDegree]= @AcademicDegree, 
          [AcademicDegreeDate]= @AcademicDegreeDate,
          [GovernmentOfficialNumber]= @GovernmentOfficialNumber, 
          [GeneralEducationLevel] = @GeneralEducationLevel WHERE Id = @Id";
        public const string QueryGetByIdCurriculumVitae = @"select * from [dbo].[CurriculumVitae] where Id = @Id";
        public const string QueryGetCurriculumVitaeByIds = @"select * from [dbo].[CurriculumVitae] where Id IN @Ids";
        public const string QueryDeleteCurriculumVitae = @"Delete from [dbo].[CurriculumVitae] where Id IN @Ids";
        public const string QueryInsertCurriculumVitaeDeleted = @"INSERT INTO [dbo].[Deleted_CurriculumVitae]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[SocialSecurityNumber]
           ,[AnotherName]
           ,[CitizenCardNumber]
           ,[CitizenCardFile]
           ,[Religion]
           ,[Ethnicity]
           ,[IdNationality]
           ,[PlaceOfBirth]
           ,[Award]
           ,[DisabledVeteranRank]
           ,[HealthStatus]
           ,[IdPolicybeneficiary]
           ,[Height]
           ,[Weight]
           ,[BloodType]
           ,[IdProfessionalQualification]
           ,[PoliticalReasoning]
           ,[IdGovernmentManagement]
           ,[GovernmentAward]
           ,[DisciplinaryAction]
           ,[Profession]
           ,[PrimaryJob]
           ,[ForteOfWork]
           ,[TitleConcurrent]
           ,[SpecializedProfessionalQualifications]
           ,[AcademicDegree]
           ,[AcademicDegreeDate]
           ,[GovernmentOfficialNumber]
           ,[GeneralEducationLevel])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @SocialSecurityNumber,
           @AnotherName,
           @CitizenCardNumber,
           @CitizenCardFile,
           @Religion,
           @Ethnicity,
           @IdNationality,
           @PlaceOfBirth,
           @Award,
           @DisabledVeteranRank,
           @HealthStatus,
           @IdPolicybeneficiary,
           @Height,
           @Weight,
           @BloodType,
           @IdProfessionalQualification,
           @PoliticalReasoning,
           @IdGovernmentManagement,
           @GovernmentAward,
           @DisciplinaryAction,
           @Profession,
           @PrimaryJob,
           @ForteOfWork,
           @TitleConcurrent,
           @SpecializedProfessionalQualifications,
           @AcademicDegree,
           @AcademicDegreeDate,
           @GovernmentOfficialNumber,
           @GeneralEducationLevel)";
    }

}
