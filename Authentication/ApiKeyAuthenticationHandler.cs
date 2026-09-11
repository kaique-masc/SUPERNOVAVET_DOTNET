using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ChallengePetApi.Authentication;

public class ApiKeyAuthenticationHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "ApiKey";

    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration)
        : base(options, logger, encoder)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-API-KEY", out var apiKey))
        {
            return Task.FromResult(
                AuthenticateResult.Fail("API Key não informada.")
            );
        }

        var chaveEsperada = _configuration["ApiKey"];

        if (string.IsNullOrWhiteSpace(chaveEsperada) ||
            apiKey != chaveEsperada)
        {
            return Task.FromResult(
                AuthenticateResult.Fail("API Key inválida.")
            );
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "SuperNovaVetClient")
        };

        var identity = new ClaimsIdentity(
            claims,
            SchemeName
        );

        var principal = new ClaimsPrincipal(identity);

        var ticket = new AuthenticationTicket(
            principal,
            SchemeName
        );

        return Task.FromResult(
            AuthenticateResult.Success(ticket)
        );
    }
}