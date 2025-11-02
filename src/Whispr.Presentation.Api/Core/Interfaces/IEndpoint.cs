using Asp.Versioning.Builder;

namespace Whispr.Presentation.Api.Core.Interfaces;

public interface IEndpoint
{
    void MapEndpoint(IVersionedEndpointRouteBuilder builder);
}