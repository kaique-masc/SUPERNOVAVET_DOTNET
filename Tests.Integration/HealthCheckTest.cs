using System.Net;
using Xunit;

namespace Tests.Integration;

[Collection("IntegrationTests")]
public class HealthCheckTests
{
    private readonly HttpClient _client;

    public HealthCheckTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ApiDisponivel_DeveRetornar200()
    {
        // Arrange
        var endpoint = "/health";

        // Act
        var response = await _client.GetAsync(endpoint);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RotaInexistente_DeveRetornar404()
    {
        // Arrange
        var endpoint = "/rota-inexistente";

        // Act
        var response = await _client.GetAsync(endpoint);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}