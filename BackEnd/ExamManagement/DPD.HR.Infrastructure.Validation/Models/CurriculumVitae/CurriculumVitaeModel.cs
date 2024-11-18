using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HR.Infrastructure.Validation.Models.CurriculumVitae
{
    public class CurriculumVitaeModel
    {
        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public Guid IdEmployee { get; set; }
        public string? SocialSecurityNumber { get; set; }
        public string? AnotherName { get; set; }
        public string? CitizenCardNumber { get; set; }
        public string? CitizenCardFile { get; set; }
        public string? Religion { get; set; }
        public string? Ethnicity { get; set; }
        public Guid IdNationality { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? Award { get; set; }
        public string? DisabledVeteranRank { get; set; }
        public string? HealthStatus { get; set; }
        public Guid? IdPolicyBeneficiary { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public int? BloodType { get; set; }
        public Guid? IdProfessionalQualification { get; set; }
        public int? PoliticalReasoning { get; set; }
        public Guid? IdGovernmentManagement { get; set; }
        public string? GovernmentAward { get; set; }
        public string? DisciplinaryAction { get; set; }
        public string? Profession { get; set; }
        public string? PrimaryJob { get; set; }
        public string? ForteOfWork { get; set; }
        public string? TitleConcurrent { get; set; }
        public string? SpecializedProfessionalQualifications { get; set; }
        public string? AcademicDegree { get; set; }
        public DateTime? AcademicDegreeDate { get; set; }
        public string? GovernmentOfficialNumber { get; set; }
        public int? GeneralEducationLevel { get; set; }
    }
}
