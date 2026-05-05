namespace Whispr.Presentation.Web.Core.Http;

public sealed record NoContent
{
    public static NoContent Create()
    {
        return new NoContent();
    }
}