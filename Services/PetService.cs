using ChallengePetApi.Models;

namespace ChallengePetApi.Services;

public class PetService : IPetService
{
    public Pet ObterPetPorId(int id)
    {
        return new Pet
        {
            Id = id,
            Nome = "Pet Teste",
            Especie = "Cachorro",
            Idade = 3,
            NivelRisco = "Baixo"
        };
    }
}