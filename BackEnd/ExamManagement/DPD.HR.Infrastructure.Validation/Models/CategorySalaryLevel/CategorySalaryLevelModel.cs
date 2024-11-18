namespace DPD.HR.Infrastructure.Validation.Models.CategorySalaryLevel


{
    public class CategorySalaryLevelModel
    {
      
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status   { get; set; }
            public string? NameSalaryLevel { get; set; }
            public float? Amount { get; set; }
            public Guid IdSalaryScale { get; set; }
            public bool IsCoefficient { get; set; }
            public float? SocialSecurityContributionRate { get; set; }
       
    }
}
