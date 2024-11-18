namespace DPD.HumanResources.Dtos.Custom;

public class FilterOverTimeModel
{
    public Guid? IdEmployee { get; set; }
    public string? FromDate {get;set;}
    public string? ToDate {get;set;}
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
}