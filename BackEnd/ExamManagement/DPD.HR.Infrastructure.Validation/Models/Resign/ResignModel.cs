namespace DPD.HR.Infrastructure.Validation.Models.Resign;

public class ResignModel
{
    public Guid? Id {get;set;}
    public string? Description {get;set;}
    public Guid IdEmployee {get;set;}
    public Guid IdUnit {get;set;}
    public string? UnitName {get;set;}
    public string? ResignForm {get;set;}
    public Guid? IdFile {get;set;}
}