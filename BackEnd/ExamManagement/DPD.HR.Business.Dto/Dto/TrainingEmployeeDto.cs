

namespace DPD.HumanResources.Dtos.Dto
{
    public class TrainingEmployeeDto
    {
      
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdEmployee { get; set; }
            public int? Sort { get; set; }
            public string? NameTrainingInstitution { get; set; }
            public string? Major { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public int? TrainingType { get; set; }
            public string? Certificate { get; set; }
        
    }
}
