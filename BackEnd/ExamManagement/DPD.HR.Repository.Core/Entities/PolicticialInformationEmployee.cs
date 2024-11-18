

namespace DPD.HumanResources.Entities.Entities
{
    public class PolicticialInformationEmployee
    {
     
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdEmployee { get; set; }
            public int? Sort { get; set; }
            public int? TypeOfPolitical { get; set; }
            public DateTime? DatePartyAdmission { get; set; }
            public DateTime? DatePrePartyAdmission { get; set; }
            public string? PlacePrePartyAdmission { get; set; }
            public DateTime? DatePartyAdmissionEnd { get; set; }
            public string? PoliticalSocialOrganization { get; set; }
            public string? Position { get; set; }
            public string PositionSecond { get; set; }
            public string PositionThird { get; set; }
            public DateTime? DateJoinOrganization { get; set; }
            public DateTime? YouthUnionEnrollmentDate { get; set; }
            public string? YouthUnionEnrollmentPlace { get; set; }
            public string? CurrentYouthUnionActivitiesLocation { get; set; }
            public string? PartyCommittee { get; set; }
            public string? PartyCell { get; set; }
            public DateTime? PartyMembershipCardDate { get; set; }
            public string? PartyMembershipCardComittee { get; set; }
            public DateTime? DatePartyAdmissionSecond { get; set; }
            public string? YouthOrganization { get; set; }
            public string? Note { get; set; }
        
    }
}
