using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ImageEditor.Api.Dtos.Users;
using ImageEditor.Api.Settings;
using ImageEditor.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ImageEditor.Api.Controllers.V1;
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
public class AuthController : BaseController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentitySettings _identitySettings;
    private readonly ILogger<AuthController> _logger;

    public AuthController(INotifier notifier,
        IIdentityUser identityUser,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IOptions<IdentitySettings> identitySettings,
        ILogger<AuthController> logger) : base(notifier, identityUser)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _identitySettings = identitySettings.Value;
        _logger = logger;
    }

    [HttpPost("signUp")]
    public async Task<ActionResult> SignUp(SignUpDto registerUser)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var user = new IdentityUser()
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return CustomResponse(await GenerateJWT(registerUser.Email));
        }

        foreach (var error in result.Errors)
            NotifyError(error.Description);

        return CustomResponse(registerUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, true);

        if (result.Succeeded)
        {
            _logger.LogInformation($"User with the email address {loginDto.Email} has successfully logged in");
            return CustomResponse(await GenerateJWT(loginDto.Email));
        }

        if (result.IsLockedOut)
        {
            NotifyError("User exceeded the maximum tentatives, try again later");
            return CustomResponse(loginDto);
        }

        NotifyError("Email or Password don't exist");
        return CustomResponse(loginDto);
    }

    private async Task<LoginResponseDto> GenerateJWT(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
        claims.Add(new Claim("general", "commonUser"));

        foreach (var role in userRoles)
            claims.Add(new Claim("role", role));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_identitySettings.Secret);
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _identitySettings.Issuer,
            Expires = DateTime.UtcNow.AddHours(_identitySettings.ExpirationTime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Subject = identityClaims,
        });

        var encodedToken = tokenHandler.WriteToken(token);

        var response = new LoginResponseDto
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_identitySettings.ExpirationTime).TotalSeconds,
            UserToken = new UserTokenDto
            {
                Id = user.Id,
                Email = user.Email,
                Claims = claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value })
            },
        };

        return response;
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
}
