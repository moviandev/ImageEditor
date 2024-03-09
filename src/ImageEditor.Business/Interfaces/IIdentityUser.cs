using System.Security.Claims;

namespace ImageEditor.Business.Interfaces;
public interface IIdentityUser
{
    public string Name { get; }
    public Guid GetUserId();
    public string GetUserEmail();
    public bool IsUserAuthenticated();
    public bool IsInRole(string role);
    IEnumerable<Claim> GetClaims();
}
