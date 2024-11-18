namespace DPD.HumanResources.Dtos.Dto;

public class CompanyInformationDto
{
    public Guid Id { get; set; }
    public string? CompanyName { get; set; }
    public string? TaxNumber { get; set; }
    public string? AccountNumber { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? OpeningHours { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? Status { get; set; }
    public string? Copyright { get; set; }
    public string? Logo { get; set; }
    public string? Fax { get; set; }

    public Guid? IdFile { get; set; }
}