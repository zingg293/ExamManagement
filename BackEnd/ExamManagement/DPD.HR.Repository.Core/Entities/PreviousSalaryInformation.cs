namespace DPD.HumanResources.Entities.Entities
{
    public class PreviousSalaryInformation
    {

        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public bool IsCoefficient { get; set; }
        public Guid IdEmployee { get; set; }
        public Guid? IdSalaryScale { get; set; }
        public Guid? IdSalaryLevel { get; set; }
        public Guid? IdTypeSalaryScale { get; set; }
        public DateTime? DateUpgradeSalaryLevel { get; set; }
        public DateTime? DateChangeSalary { get; set; }
        public float? SocialSecuritySalary { get; set; }
        public float? SocialSecurityContributionRate { get; set; }

    }
}
