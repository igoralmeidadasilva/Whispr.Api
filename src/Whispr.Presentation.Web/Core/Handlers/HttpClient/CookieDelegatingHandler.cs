using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Whispr.Presentation.Web.Core.Handlers.HttpClient;

public sealed class CookieDelegatingHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        return base.SendAsync(request, cancellationToken);
    }
}