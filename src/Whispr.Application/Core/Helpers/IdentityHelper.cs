using Microsoft.AspNetCore.Identity;

namespace Whispr.Application.Core.Helpers;

public static class IdentityHelper
{
    public static string ToErrorMessage(IEnumerable<IdentityError> identityErrors)
    {
        return string.Join("\n", identityErrors.Select(x => $"{x.Code}:{x.Description}"));
    }
}