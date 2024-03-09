using System.Security.Claims;
using ImageEditor.Business.Interfaces;

namespace ImageEditor.Api.Extensions;
public class IdentityUser : IIdentityUser
{
    public readonly IHttpContextAccessor _accessor;

    public IdentityUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string Name
        => _accessor.HttpContext.User.Identity.Name;

    public IEnumerable<Claim> GetClaims()
        => _accessor.HttpContext.User.Claims;

    public string GetUserEmail()
        => IsUserAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : string.Empty;

    public Guid GetUserId()
        => IsUserAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;

    public bool IsInRole(string role)
        => _accessor.HttpContext.User.IsInRole(role);

    public bool IsUserAuthenticated()
        => _accessor.HttpContext.User.Identity.IsAuthenticated;
}

public static class ClaimsPrincipalExtension
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal is null)
            throw new ArgumentNullException(nameof(principal));

        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

        return claim?.Value;
    }

    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        if (principal is null)
            throw new ArgumentNullException(nameof(principal));

        var claim = principal.FindFirst(ClaimTypes.Email);

        return claim?.Value;
    }
}