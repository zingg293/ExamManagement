

namespace DPD.HumanResources.Dtos.Dto
{
    public class AllowancePreviousSalaryInformationDto
    {
        
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdPreviousSalaryInformation { get; set; }
            public Guid? IdAllowance { get; set; }
   
    }
}
