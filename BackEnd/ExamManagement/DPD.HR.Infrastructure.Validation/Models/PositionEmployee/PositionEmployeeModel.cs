namespace DPD.HR.Infrastructure.Validation.Models.PositionEmployee;

public class PositionEmployeeModel
{
    public Guid? Id {get;set;}
    public Guid IdEmployee {get;set;}
    public Guid IdPosition {get;set;}
    public bool IsHeadcount {get;set;}
    public Guid IdUnit {get;set;}
}