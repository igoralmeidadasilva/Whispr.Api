using Whispr.Presentation.Web.Components.Features.ProblemModals;
using Whispr.Presentation.Web.Core.Enums;
using Whispr.Presentation.Web.Services.Ui.Modal;

namespace Whispr.Presentation.Web.Core.Handlers.HttpClient;

public class GlobalErrorDelegatingHandler : DelegatingHandler
{
    private readonly IModalService _modalService;
    private readonly ILogger<GlobalErrorDelegatingHandler> _logger;

    public GlobalErrorDelegatingHandler(IModalService modalService, ILogger<GlobalErrorDelegatingHandler> logger)
    {
        _modalService = modalService;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
            //var response = await base.SendAsync(request, cancellationToken);

            //if ((int)response.StatusCode >= 500)
            //{
            //    var correlationId = ExtractCorrelationId(response);

            //    var problem = new ProblemModalParameters
            //    {
            //        HeaderColor = Colors.Danger,
            //        Title = "Erro no servidor",
            //        Problem = "O servidor encontrou um problema interno e não conseguiu processar sua requisição.",
            //        Description = $"Código HTTP: {(int)response.StatusCode} ({response.StatusCode})",
            //        CorrelationId = correlationId
            //    };

            //    _logger.LogError(
            //        "Erro 5xx recebido. StatusCode: {StatusCode}, URL: {Url}, CorrelationId: {CorrelationId}",
            //        (int)response.StatusCode,
            //        request.RequestUri,
            //        correlationId);

            //    await _modalService.ShowAsync(problem);
            //}

            //return response;
        }
        catch (HttpRequestException ex)
        {
            var problem = new ProblemModalParameters
            {
                HeaderColor = Colors.Warning,
                Title = "Serviço indisponível",
                Problem = "Não foi possível estabelecer conexão com o servidor.",
                Description = "Verifique sua conexão com a internet ou tente novamente em instantes. " +
                              "Se o problema persistir, o serviço pode estar temporariamente fora do ar."
            };

            _logger.LogError(ex,
                "Falha ao conectar com a API. URL: {Url}. Motivo: {Message}",
                request.RequestUri,
                ex.Message);

            await _modalService.ShowAsync(problem);

            return new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable);
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            var problem = new ProblemModalParameters
            {
                HeaderColor = Colors.Warning,
                Title = "Tempo esgotado",
                Problem = "A requisição demorou mais do que o esperado e foi cancelada.",
                Description = "Isso pode indicar lentidão no servidor ou na sua conexão. Tente novamente em instantes."
            };

            _logger.LogError(ex,
                "Timeout na requisição. URL: {Url}",
                request.RequestUri);

            await _modalService.ShowAsync(problem);

            return new HttpResponseMessage(System.Net.HttpStatusCode.RequestTimeout);
        }
        catch (Exception ex)
        {
            var problem = new ProblemModalParameters
            {
                HeaderColor = Colors.Danger,
                Title = "Erro inesperado",
                Problem = "Ocorreu um erro inesperado ao processar a sua solicitação.",
                Description = "Se o problema continuar, entre em contato com o suporte."
            };

            _logger.LogError(ex,
                "Erro inesperado no handler HTTP. URL: {Url}. Tipo: {ExceptionType}",
                request.RequestUri,
                ex.GetType().Name);

            await _modalService.ShowAsync(problem);

            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}