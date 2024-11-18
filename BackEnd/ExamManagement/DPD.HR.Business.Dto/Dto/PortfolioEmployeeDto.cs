
namespace DPD.HumanResources.Dtos.Dto
{
    public class PortfolioEmployeeDto
    {

            public Guid Id { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? Status { get; set; }
            public Guid IdEmployee { get; set; }
            public int? Sort { get; set; }
            public bool? IsSubmit { get; set; }
            public string? Name { get; set; }
            public string? Type { get; set; }
            public DateTime? DateSubmit { get; set; }

    }
}
