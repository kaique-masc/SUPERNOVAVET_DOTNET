using ChallengePetApi.Models;

namespace ChallengePetApi.Services;

public interface IPetService
{
    Pet ObterPetPorId(int id);
}