namespace DPD.HumanResources.Dtos.Dto
{
    public class SalaryNonCoefficientDto
    {
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdEmployee { get; set; }
            public Guid? IdSalaryScale { get; set; }
            public Guid? IdSalaryLevel { get; set; }
            public DateTime? DateUpgradeSalaryLevel { get; set; }
            public DateTime? DateChangeSalary { get; set; }
            public float? NetSalary { get; set; }
            public float? BasicSalary { get; set; }
            public float? SocialSecuritySalary { get; set; }
            public Guid? IdTypeSalaryScale { get; set; }
        
    }
}
