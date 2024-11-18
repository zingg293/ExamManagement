using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class FamilyInformationEmployeeSqlQueries
    {
        public const string QueryInsertFamilyInformationEmployee = @"
INSERT INTO [dbo].[FamilyInformationEmployee]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[Type]
           ,[Relationship]
           ,[Fullname]
           ,[Sex]
           ,[SocialSecurityCode]
           ,[DateofBirth]
           ,[TaxCode]
           ,[PhoneNumber]
           ,[CCCDNumber]
           ,[DateCCCD]
           ,[PlaceCCCD]
           ,[Nationality]
           ,[Ethnicity]
           ,[IsStudent]
           ,[SchoolName]
           ,[Job]
           ,[Note]
           ,[WorkPlace]
           ,[Position]
           ,[IsTaxDeductionEligibility]
           ,[FromDateTaxDeductionEligibility]
           ,[ToDateTaxDeductionEligibility]
           ,[MedicalRecord]
           ,[PlaceOfBirth]
           ,[PermanentAddress]
           ,[TemporaryAddress]
           ,[HometownAddress])
     VALUES
           (@Id, @CreatedDate, @Status, @IdEmployee, @Sort, @Type, @Relationship, @Fullname, @Sex,
            @SocialSecurityCode, @DateofBirth, @TaxCode, @PhoneNumber, @CCCDNumber, @DateCCCD,
            @PlaceCCCD, @Nationality, @Ethnicity, @IsStudent, @SchoolName, @Job, @Note, @WorkPlace,
            @Position, @IsTaxDeductionEligibility, @FromDateTaxDeductionEligibility,
            @ToDateTaxDeductionEligibility, @MedicalRecord, @PlaceOfBirth, @PermanentAddress,
            @TemporaryAddress, @HometownAddress)";
        public const string QueryUpdateFamilyInformationEmployee = @"
UPDATE [dbo].[FamilyInformationEmployee]
   SET [CreatedDate] = @CreatedDate
      ,[Status] = @Status
      ,[IdEmployee] = @IdEmployee
      ,[Sort] = @Sort
      ,[Type] = @Type
      ,[Relationship] = @Relationship
      ,[Fullname] = @Fullname
      ,[Sex] = @Sex
      ,[SocialSecurityCode] = @SocialSecurityCode
      ,[DateofBirth] = @DateofBirth
      ,[TaxCode] = @TaxCode
      ,[PhoneNumber] = @PhoneNumber
      ,[CCCDNumber] = @CCCDNumber
      ,[DateCCCD] = @DateCCCD
      ,[PlaceCCCD] = @PlaceCCCD
      ,[Nationality] = @Nationality
      ,[Ethnicity] = @Ethnicity
      ,[IsStudent] = @IsStudent
      ,[SchoolName] = @SchoolName
      ,[Job] = @Job
      ,[Note] = @Note
      ,[WorkPlace] = @WorkPlace
      ,[Position] = @Position
      ,[IsTaxDeductionEligibility] = @IsTaxDeductionEligibility
      ,[FromDateTaxDeductionEligibility] = @FromDateTaxDeductionEligibility
      ,[ToDateTaxDeductionEligibility] = @ToDateTaxDeductionEligibility
      ,[MedicalRecord] = @MedicalRecord
      ,[PlaceOfBirth] = @PlaceOfBirth
      ,[PermanentAddress] = @PermanentAddress
      ,[TemporaryAddress] = @TemporaryAddress
      ,[HometownAddress] = @HometownAddress
          WHERE Id = @Id";
        public const string QueryGetByIdFamilyInformationEmployee = @"select * from [dbo].[FamilyInformationEmployee] where Id = @Id";
        public const string QueryGetFamilyInformationEmployeeByIds = @"select * from [dbo].[FamilyInformationEmployee] where Id IN @Ids";
        public const string QueryDeleteFamilyInformationEmployee = @"Delete from [dbo].[FamilyInformationEmployee] where Id IN @Ids";
        public const string QueryGetAllFamilyInformationEmployee = @"SELECT * FROM FamilyInformationEmployee order by CreatedDate";
        public const string QueryHideFamilyInformationEmployee = @"SELECT * FROM FamilyInformationEmployee order by CreatedDate";
        public const string QueryGetAllFamilyInformationEmployeeAvailable = @"";
        public const string QueryInsertFamilyInformationEmployeeDeleted =
@"
INSERT INTO[dbo].[Deleted_FamilyInformationEmployee]
        ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[Type]
           ,[Relationship]
           ,[Fullname]
           ,[Sex]
           ,[SocialSecurityCode]
           ,[DateofBirth]
           ,[TaxCode]
           ,[PhoneNumber]
           ,[CCCDNumber]
           ,[DateCCCD]
           ,[PlaceCCCD]
           ,[Nationality]
           ,[Ethnicity]
           ,[IsStudent]
           ,[SchoolName]
           ,[Job]
           ,[Note]
           ,[WorkPlace]
           ,[Position]
           ,[IsTaxDeductionEligibility]
           ,[FromDateTaxDeductionEligibility]
           ,[ToDateTaxDeductionEligibility]
           ,[MedicalRecord]
           ,[PlaceOfBirth]
           ,[PermanentAddress]
           ,[TemporaryAddress]
           ,[HometownAddress])
     VALUES
           (@Id, @CreatedDate, @Status, @IdEmployee, @Sort, @Type, @Relationship, @Fullname, @Sex,
            @SocialSecurityCode, @DateofBirth, @TaxCode, @PhoneNumber, @CCCDNumber, @DateCCCD,
            @PlaceCCCD, @Nationality, @Ethnicity, @IsStudent, @SchoolName, @Job, @Note, @WorkPlace,
            @Position, @IsTaxDeductionEligibility, @FromDateTaxDeductionEligibility,
            @ToDateTaxDeductionEligibility, @MedicalRecord, @PlaceOfBirth, @PermanentAddress,
            @TemporaryAddress, @HometownAddress)";
    }
}
