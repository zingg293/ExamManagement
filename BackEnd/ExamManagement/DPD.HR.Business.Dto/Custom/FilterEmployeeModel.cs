namespace DPD.HumanResources.Dtos.Custom;

public class FilterEmployeeModel
{
    public string? Code {get;set;}
    public string? Phone {get;set;}
    public Guid? IdUnit {get;set;}
    public Guid? TypeOfEmployee {get;set;}
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
}