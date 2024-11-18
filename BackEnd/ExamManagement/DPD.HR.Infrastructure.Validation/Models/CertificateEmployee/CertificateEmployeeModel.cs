namespace DPD.HR.Infrastructure.Validation.Models.CertificateEmployee
{
    public class CertificateEmployeeModel
    {
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdEmployee { get; set; }
            public int? Sort { get; set; }
            public string? CertificateName { get; set; }
            public string? CertificateType { get; set; }
            public string? Content { get; set; }
            public bool? IsProfessionalCertificate { get; set; }
            public string? CertificateNumber { get; set; }
            public string? TrainingInstitution { get; set; }
            public string? TrainingFormat { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
      
    }
}
