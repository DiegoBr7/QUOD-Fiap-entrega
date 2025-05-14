using Microsoft.AspNetCore.Mvc;
using Quod.Antifraude.Core.Models;

namespace Quod.Antifraude.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacoesController : ControllerBase
    {
        [HttpPost("fraude")]
        public IActionResult ReceberFraude([FromBody] RegistroValidacao registro)
        {
            // Simule o processamento da notificação
            return Ok(new { mensagem = "Notificação de fraude recebida com sucesso!" });
        }
    }
}
