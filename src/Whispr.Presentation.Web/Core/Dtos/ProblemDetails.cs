namespace Whispr.Presentation.Web.Core.Dtos;

public sealed record ProblemDetails
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
    public Dictionary<string, object>? Extensions { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}