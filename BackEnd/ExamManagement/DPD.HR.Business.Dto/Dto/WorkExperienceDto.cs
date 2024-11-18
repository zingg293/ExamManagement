

namespace DPD.HumanResources.Dtos.Dto
{
    public class WorkExperienceDto
    {
      
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdEmployee { get; set; }
            public int? Sort { get; set; }
            public string? UnitName { get; set; }
            public string? Address { get; set; }
            public string? JobTitle { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public string? Job { get; set; }
            public string? Reference { get; set; }
            public string? PhoneReference { get; set; }
        
    }
}
