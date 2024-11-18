

namespace DPD.HumanResources.Dtos.Dto

{
    public class BankAccountInformationDto
    {
      
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public string? ProvincialBank { get; set; }
            public string? NameOfBank { get; set; }
            public string? AccountNumber { get; set; }
            public string? AccountHolder { get; set; }
            public Guid IdEmployee { get; set; }
            public bool IsDefault { get; set; }
        
    }
}
