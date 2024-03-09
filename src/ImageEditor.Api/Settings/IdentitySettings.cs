namespace ImageEditor.Api.Settings;
public class IdentitySettings
{
    public string Secret { get; set; }
    public int ExpirationTime { get; set; }
    public string Issuer { get; set; }
}