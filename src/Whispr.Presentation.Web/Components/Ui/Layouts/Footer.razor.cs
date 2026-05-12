using Microsoft.AspNetCore.Components;

namespace Whispr.Presentation.Web.Components.Ui.Layouts;

public partial class Footer : ComponentBase
{
    private int CurrentYear => DateTime.Now.Year;
    private const string MyName = "Igor Almeida da Silva";
}