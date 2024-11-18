namespace DPD.HumanResources.Entities.Entities;

public class News
{
    public Guid Id {get;set;}
    public string? Title {get;set;}
    public string? Description {get;set;}
    public bool? IsHide {get;set;}
    public bool? IsDeleted {get;set;}
    public bool? IsApproved {get;set;}
    public Guid UserCreated {get;set;}
    public Guid UserUpdated {get;set;}
    public int? Status {get;set;}
    public DateTime? CreatedDate {get;set;}
    public DateTime? CreatedDateDisplay {get;set;}
    public DateTime? UpdateDate {get;set;}
    public string? NewsContent {get;set;}
    public string? NewContentDraft {get;set;}
    public Guid Author {get;set;}
    public int? NewsLike {get;set;}
    public int? NewsView {get;set;}
    public string? Avatar {get;set;}
    public string? ExtensionFile {get;set;}
    public string? FilePath {get;set;}
    public Guid IdCategoryNews {get;set;}
}