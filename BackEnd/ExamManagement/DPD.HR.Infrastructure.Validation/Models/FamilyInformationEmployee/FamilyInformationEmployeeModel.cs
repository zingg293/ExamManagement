
namespace DPD.HR.Infrastructure.Validation.Models.FamilyInformationEmployee
{
    public class FamilyInformationEmployeeModel
    {
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdEmployee { get; set; }
            public int? Sort { get; set; }
            public int? Type { get; set; }
            public string? Relationship { get; set; }
            public string? Fullname { get; set; }
            public int Sex { get; set; }
            public string? SocialSecurityCode { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string? TaxCode { get; set; }
            public string? PhoneNumber { get; set; }
            public string? CCCDNumber { get; set; }
            public string? DateCCCD { get; set; }
            public string? PlaceCCCD { get; set; }
            public string? Nationality { get; set; }
            public string? Ethnicity { get; set; }
            public bool? IsStudent { get; set; }
            public string? SchoolName { get; set; }
            public string? Job { get; set; }
            public string? Note { get; set; }
            public string? WorkPlace { get; set; }
            public string? Position { get; set; }
            public bool? IsTaxDeductionEligibility { get; set; }
            public DateTime? FromDateTaxDeductionEligibility { get; set; }
            public DateTime? ToDateTaxDeductionEligibility { get; set; }
            public bool? MedicalRecord { get; set; }
            public string? PlaceOfBirth { get; set; }
            public string? PermanentAddress { get; set; }
            public string? TemporaryAddress { get; set; }
            public string? HometownAddress { get; set; }
        
    }
    
}
