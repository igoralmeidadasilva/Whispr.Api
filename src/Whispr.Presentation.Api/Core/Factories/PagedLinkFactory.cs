using Whispr.Presentation.Api.Core.Models;
using Whispr.SharedKernel.Pagination;

namespace Whispr.Presentation.Api.Core.Factories;

public sealed class PagedModelFactory
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PagedModelFactory(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    public PagedModel<T> Create<T>(PagedList<T> page)
    {
        HttpContext httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext unavailable.");

        string? endpointName = httpContext.GetEndpoint()
            ?.Metadata.GetMetadata<EndpointNameMetadata>()
            ?.EndpointName;

        string? next = BuildUrl(httpContext, endpointName, page.HasNext, page.PageNumber + 1, page.PageSize);
        string? previous = BuildUrl(httpContext, endpointName, page.HasPrevious, page.PageNumber - 1, page.PageSize);
        
        return PagedModel<T>.From(page, next, previous);
    }

    private string? BuildUrl(HttpContext httpContext, string? endpointName, bool hasPage, int targetPage, int pageSize)
    {
        if (!hasPage || endpointName is null)
        {
            return null;
        }

        var routeValues = new RouteValueDictionary();

        foreach (var (key, value) in httpContext.Request.Query)
        {
            routeValues[key] = value.ToString();
        }

        routeValues["pageNumber"] = targetPage;
        routeValues["pageSize"] = pageSize;

        return _linkGenerator.GetUriByName(httpContext, endpointName, new RouteValueDictionary(routeValues));
    }
}