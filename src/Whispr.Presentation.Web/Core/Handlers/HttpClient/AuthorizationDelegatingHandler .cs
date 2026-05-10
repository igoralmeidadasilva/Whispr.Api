using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http.Headers;
using Whispr.Presentation.Web.Core.Dtos;

namespace Whispr.Presentation.Web.Core.Handlers.HttpClient;

public sealed class AuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;

    public AuthorizationDelegatingHandler(ILocalStorageService localStorage, NavigationManager navigation)
    {
        _localStorage = localStorage;
        _navigation = navigation;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync<AuthTokenDto>(Constants.LocalStorageKeys.AuthKey, cancellationToken);

        if (token?.AccessToken is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var url = QueryHelpers.AddQueryString(Routes.Web.Login, new Dictionary<string, string?>
            {
                ["returnUrl"] = _navigation.Uri
            });

            _navigation.NavigateTo(url, forceLoad: false);
        }

        return response;
    }
}