namespace DPD.HumanResources.Dtos.Custom;

public class FilterMarkWorkPointsModel
{
    public string? FromDate {get;set;}
    //public string? ToDate {get;set;}
    public Guid? IdEmployee {get;set;}
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
}