

namespace DPD.HR.Infrastructure.Validation.Models.MilitaryInformationEmployee
{
    public class MilitaryInformationEmployeeModel
    {
       
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdEmployee { get; set; }
            public int? Sort { get; set; }
            public int? TypeOfMilirtary { get; set; }
            public DateTime? EnlistmentDate { get; set; }
            public DateTime? DischargeDate { get; set; }
            public int? MilitaryRank { get; set; }
            public int? HighestMilitaryRank { get; set; }
        
    }
}
