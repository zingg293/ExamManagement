

namespace DPD.HumanResources.Dtos.Dto
{
    public class MilitaryInformationEmployeeDto
    {
       
            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdEmployee { get; set; }
            public int? Sort { get; set; }
            public int? TypeOfMilitary { get; set; }
            public DateTime? EnlistmentDate { get; set; }
            public DateTime? DischargeDate { get; set; }
            public int? MilitaryRank { get; set; }
            public int? HighestMilitaryRank { get; set; }
        
    }
}
