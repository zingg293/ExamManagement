namespace DPD.HR.Infrastructure.Validation.Models.EducationalBackground
{
    public class EducationalBackgroundModel
    {
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdEmployee { get; set; }
            public int? Sort { get; set; }
            public string? TrainingInstitution { get; set; }
            public Guid? IdEducationalDegree { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public DateTime? GraduationDate { get; set; }
            public string? TrainingFormat { get; set; }
            public string? Degree { get; set; }
            public string? Major { get; set; }
            public string? GraduationResult { get; set; }
            public string? CertificateNumber { get; set; }
            public DateTime? EffectiveDate { get; set; }
            public bool? IsMain { get; set; }
            public string? VerificationDocumentNumber { get; set; }
            public DateTime? DateIssuanceReplyDocument { get; set; }
            public string? AccreditedDegreeGrantingInstitution { get; set; }
            public string? GraduationInstitution { get; set; }
            public bool? SendingOrganizationForEducation { get; set; }
            public bool? IsProfessionalCertificate { get; set; }
            public string? CertificateIssuer { get; set; }
        
    }
}
