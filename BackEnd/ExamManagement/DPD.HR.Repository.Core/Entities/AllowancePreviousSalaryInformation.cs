

namespace DPD.HumanResources.Entities.Entities
{
    public class AllowancePreviousSalaryInformation
    {

        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public Guid IdPreviousSalaryInformation { get; set; }
        public Guid? IdAllowance { get; set; }

    }
}
