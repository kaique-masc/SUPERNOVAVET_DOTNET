using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChallengePetApi.Controllers;

[ApiController]
[Route("api/protected")]
public class ProtectedController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            mensagem = "Acesso autorizado."
        });
    }
}