using System.Security.Claims;

namespace NotesApp.Api.Controllers;

internal static class UserIdResolver
{
    public static Guid GetCurrentUserIdOrThrow(ClaimsPrincipal user)
    {
        var rawUserId = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub");

        if (!Guid.TryParse(rawUserId, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid authenticated user.");
        }

        return userId;
    }
}
