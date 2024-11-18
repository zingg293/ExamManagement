

namespace DPD.HumanResources.Dtos.Dto
{
    public class PassportVisaWorkPermitDto
    {
    
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdEmployee { get; set; }
            public string? NumberVisa { get; set; }
            public string? NumberPassport { get; set; }
            public string? NumberWorkPermit { get; set; }
            public DateTime? EffectiveDateVisa { get; set; }
            public DateTime? ExpirerationDateVisa { get; set; }
            public DateTime? EffectiveDatePassport { get; set; }
            public DateTime? ExpirerationDatePassport { get; set; }
            public DateTime? EffectiveDateWorkPermit { get; set; }
            public DateTime? ExpirerationDateWorkPermit { get; set; }
            public string? PlaceOfIssueVisa { get; set; }
            public string? PlaceOfIssuePassport { get; set; }
            public string? PlaceOfIssueWorkPermit { get; set; }
        
    }
}
