using Whispr.Presentation.Web.Components.Features.ProblemModals;

namespace Whispr.Presentation.Web.Services.Ui.Modal;

public interface IModalService
{
    event Func<ProblemModalParameters, Task>? OnShow;
    Task ShowAsync(ProblemModalParameters parameters);
}