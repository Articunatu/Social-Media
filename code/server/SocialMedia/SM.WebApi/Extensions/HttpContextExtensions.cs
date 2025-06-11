using System.Security.Claims;

namespace SM.WebApi.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetLoggedInUserId(this HttpContext context)
    {
        string? userIdText = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdText, out Guid id) ? id : Guid.Empty;
    }
}
