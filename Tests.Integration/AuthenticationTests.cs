using System.Net;
using Xunit;

namespace Tests.Integration;

[Collection("IntegrationTests")]
public class AuthenticationTests
{
    private readonly HttpClient _client;

    public AuthenticationTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RotaProtegida_SemApiKey_DeveRetornar401()
    {
        var endpoint = "/api/protected";

        var response = await _client.GetAsync(endpoint);

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode
        );
    }

    [Fact]
    public async Task RotaProtegida_ComApiKeyValida_DeveRetornar200()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "/api/protected"
        );

        request.Headers.Add(
            "X-API-KEY",
            "SUPERNOVAVET-123"
        );

        var response = await _client.SendAsync(request);

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode
        );
    }
}