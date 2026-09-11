using ChallengePetApi.Models;
using Xunit;

namespace Tests.Unit;

public class PetTests
{
    [Fact]
    public void CriarPet_DadosValidos_DeveCriarPetCorretamente()
    {
        var nome = "Thor";
        var especie = "Cachorro";
        var idade = 5;
        var risco = "Baixo";

        var pet = new Pet
        {
            Nome = nome,
            Especie = especie,
            Idade = idade,
            NivelRisco = risco
        };

        Assert.Equal("Thor", pet.Nome);
        Assert.Equal("Cachorro", pet.Especie);
        Assert.Equal(5, pet.Idade);
        Assert.Equal("Baixo", pet.NivelRisco);
    }

    [Fact]
    public void AlterarNivelRisco_NovoNivel_DeveAtualizarNivel()
    {
        var pet = new Pet
        {
            Nome = "Luna",
            Especie = "Gato",
            Idade = 3,
            NivelRisco = "Baixo"
        };

        pet.NivelRisco = "Medio";

        Assert.Equal("Medio", pet.NivelRisco);
    }
}