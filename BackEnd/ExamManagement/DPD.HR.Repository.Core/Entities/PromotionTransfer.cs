namespace DPD.HumanResources.Entities.Entities;

public class PromotionTransfer
{
    public Guid Id {get;set;}
    public DateTime? CreatedDate {get;set;}
    public int? Status {get;set;}
    public string? Description {get;set;}
    public Guid IdUserRequest {get;set;}
    public Guid IdUnit {get;set;}
    public Guid IdEmployee {get;set;}
    public string? UnitName {get;set;}
    public Guid IdPositionEmployeeCurrent {get;set;}
    public string? PositionNameCurrent {get;set;}
    public bool IsTransfer {get;set;}
    public bool IsPromotion {get;set;}
    public bool IsHeadCount {get;set;}
    public Guid IdCategoryPosition {get;set;}
    public string? NameCategoryPosition {get;set;}
}