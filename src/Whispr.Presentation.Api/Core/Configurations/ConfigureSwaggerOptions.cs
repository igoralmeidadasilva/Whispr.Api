using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Whispr.Presentation.Api.Core.Configurations;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
        options.DocInclusionPredicate((docName, apiDesc) =>
        {
            if (apiDesc.GroupName == null)
            {
                return false;
            }
            return apiDesc.GroupName.Equals(docName, StringComparison.OrdinalIgnoreCase);
        });
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "WhispR",
            Version = description.ApiVersion.ToString(),
            Description = "",
            Contact = new OpenApiContact()
            {
                Name = "Igor Almeida - Backend Developer",
                Url = new Uri("https://github.com/igoralmeidadasilva/")
            },
        };
        if (description.IsDeprecated)
        {
            info.Description += "\n⚠️ This api version has been deprecated!";
        }
        return info;
    }
}
