using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class PolicticialInformationEmployeeSqlQueries
    {
        public const string QueryInsertPolicticialInformationEmployee = @"
INSERT INTO [dbo].[PolicticialInformationEmployee]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[TypeOfPolitical]
           ,[DatePartyAdmission]
           ,[DatePrePartyAdmission]
           ,[PlacePrePartyAdmission]
           ,[DatePartyAdmissionEnd]
           ,[PoliticalSocialOrganization]
           ,[Position]
           ,[PositionSecond]
           ,[PositionThird]
           ,[DateJoinOrganization]
           ,[YouthUnionEnrollmentDate]
           ,[YouthUnionEnrollmentPlace]
           ,[CurrentYouthUnionActivitiesLocation]
           ,[PartyCommittee]
           ,[PartyCell]
           ,[PartyMembershipCardDate]
           ,[PartyMembershipCardComittee]
           ,[DatePartyAdmissionSecond]
           ,[YouthOrganization]
           ,[Note])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @Sort,
           @TypeOfPolitical,
           @DatePartyAdmission,
           @DatePrePartyAdmission,
           @PlacePrePartyAdmission,
           @DatePartyAdmissionEnd,
           @PoliticalSocialOrganization,
           @Position,
           @PositionSecond,
           @PositionThird,
           @DateJoinOrganization,
           @YouthUnionEnrollmentDate,
           @YouthUnionEnrollmentPlace,
           @CurrentYouthUnionActivitiesLocation,
           @PartyCommittee,
           @PartyCell,
           @PartyMembershipCardDate,
           @PartyMembershipCardComittee,
           @DatePartyAdmissionSecond,
           @YouthOrganization,
           @Note)";
        public const string QueryUpdatePolicticialInformationEmployee = @"
UPDATE [dbo].[PolicticialInformationEmployee]
   SET
      [CreatedDate] = @CreatedDate,
      [Status] = @Status,
      [IdEmployee] = @IdEmployee,
      [Sort] = @Sort,
      [TypeOfPolitical] = @TypeOfPolitical,
      [DatePartyAdmission] = @DatePartyAdmission,
      [DatePrePartyAdmission] = @DatePrePartyAdmission,
      [PlacePrePartyAdmission] = @PlacePrePartyAdmission,
      [DatePartyAdmissionEnd] = @DatePartyAdmissionEnd,
      [PoliticalSocialOrganization] = @PoliticalSocialOrganization,
      [Position] = @Position,
      [PositionSecond] = @PositionSecond,
      [PositionThird] = @PositionThird,
      [DateJoinOrganization] = @DateJoinOrganization,
      [YouthUnionEnrollmentDate] = @YouthUnionEnrollmentDate,
      [YouthUnionEnrollmentPlace] = @YouthUnionEnrollmentPlace,
      [CurrentYouthUnionActivitiesLocation] = @CurrentYouthUnionActivitiesLocation,
      [PartyCommittee] = @PartyCommittee,
      [PartyCell] = @PartyCell,
      [PartyMembershipCardDate] = @PartyMembershipCardDate,
      [PartyMembershipCardComittee] = @PartyMembershipCardComittee,
      [DatePartyAdmissionSecond] = @DatePartyAdmissionSecond,
      [YouthOrganization] = @YouthOrganization,
      [Note] = @Note
          WHERE Id = @Id";
        public const string QueryGetByIdPolicticialInformationEmployee = @"select * from [dbo].[PolicticialInformationEmployee] where Id = @Id";
        public const string QueryGetPolicticialInformationEmployeeByIds = @"select * from [dbo].[PolicticialInformationEmployee] where Id IN @Ids";
        public const string QueryDeletePolicticialInformationEmployee = @"Delete from [dbo].[PolicticialInformationEmployee] where Id IN @Ids";
        public const string QueryGetAllPolicticialInformationEmployee = @"SELECT * FROM PolicticialInformationEmployee order by CreatedDate";
        public const string QueryGetAllPolicticialInformationEmployeeAvailable = @"";
        public const string QueryHidePolicticialInformationEmployee = @"";
        public const string QueryInsertPolicticialInformationEmployeeDeleted =
          @"
INSERT INTO[dbo].[Deleted_PolicticialInformationEmployee]
        ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[TypeOfPolitical]
           ,[DatePartyAdmission]
           ,[DatePrePartyAdmission]
           ,[PlacePrePartyAdmission]
           ,[DatePartyAdmissionEnd]
           ,[PoliticalSocialOrganization]
           ,[Position]
           ,[PositionSecond]
           ,[PositionThird]
           ,[DateJoinOrganization]
           ,[YouthUnionEnrollmentDate]
           ,[YouthUnionEnrollmentPlace]
           ,[CurrentYouthUnionActivitiesLocation]
           ,[PartyCommittee]
           ,[PartyCell]
           ,[PartyMembershipCardDate]
           ,[PartyMembershipCardComittee]
           ,[DatePartyAdmissionSecond]
           ,[YouthOrganization]
           ,[Note])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @IdEmployee,
           @Sort,
           @TypeOfPolitical,
           @DatePartyAdmission,
           @DatePrePartyAdmission,
           @PlacePrePartyAdmission,
           @DatePartyAdmissionEnd,
           @PoliticalSocialOrganization,
           @Position,
           @PositionSecond,
           @PositionThird,
           @DateJoinOrganization,
           @YouthUnionEnrollmentDate,
           @YouthUnionEnrollmentPlace,
           @CurrentYouthUnionActivitiesLocation,
           @PartyCommittee,
           @PartyCell,
           @PartyMembershipCardDate,
           @PartyMembershipCardComittee,
           @DatePartyAdmissionSecond,
           @YouthOrganization,
           @Note)";
    }
}
