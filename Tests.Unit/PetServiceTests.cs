using ChallengePetApi.Models;
using ChallengePetApi.Services;
using Moq;
using Xunit;

namespace Tests.Unit;

public class PetServiceTests
{
    [Fact]
    public void ObterPetPorId_IdValido_DeveRetornarPet()
    {
        var petEsperado = new Pet
        {
            Id = 1,
            Nome = "Thor",
            Especie = "Cachorro",
            Idade = 5,
            NivelRisco = "Baixo"
        };

        var mockService = new Mock<IPetService>();

        mockService
            .Setup(service => service.ObterPetPorId(1))
            .Returns(petEsperado);

        var resultado = mockService.Object.ObterPetPorId(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
        Assert.Equal("Thor", resultado.Nome);

        mockService.Verify(
            service => service.ObterPetPorId(1),
            Times.Once
        );
    }
}