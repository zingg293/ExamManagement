namespace DPD.HR.Infrastructure.Validation.Models.News;

public class NewsModel
{
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedDateDisplay { get; set; }
    public string? NewsContent { get; set; }
    public string? NewContentDraft { get; set; }
    public Guid Author { get; set; }
    public Guid IdCategoryNews { get; set; }

    public Guid? IdFile { get; set; }
}