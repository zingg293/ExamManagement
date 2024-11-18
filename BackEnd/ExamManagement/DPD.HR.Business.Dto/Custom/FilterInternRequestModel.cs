namespace DPD.HumanResources.Dtos.Custom;

public class FilterInternRequestModel
{
    public Guid? IdEmployee { get; set; }
    public Guid? IdUnit { get; set; }
    public Guid? IdPosition { get; set; }
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
}