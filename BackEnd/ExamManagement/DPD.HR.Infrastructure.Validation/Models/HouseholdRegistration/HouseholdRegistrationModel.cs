

namespace DPD.HR.Infrastructure.Validation.Models.HouseholdRegistration
{
    public class HouseholdRegistrationModel
    {
       
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public string? FamilyCode { get; set; }
            public Guid IdEmployee { get; set; }
            public Guid IdHouseholdRegistrationType { get; set; }
            public string? NumberFamilyBook { get; set; }
            public bool? IsHouseholdHead { get; set; }
            public string? NumberPhone { get; set; }
            public string? Address { get; set; }
            public string? HouseholderRelationship { get; set; }
        
    }
}
