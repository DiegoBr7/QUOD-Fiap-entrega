using Microsoft.AspNetCore.Mvc;
using Quod.Antifraude.Api.Models;
using Quod.Antifraude.Services.Biometria;

namespace Quod.Antifraude.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BiometriaController    :   ControllerBase
    {
        private readonly IBiometriaDigitalService _biometriaService;

        public BiometriaController(IBiometriaDigitalService biometriaService)
        {
            _biometriaService = biometriaService;
        }

        [HttpPost("digital/simular-captura")]
        public async Task<IActionResult> SimularCapturaDigital()
        {
            var templateSimulado = await _biometriaService.SimularCapturaDigitalAsync();
            return Ok(new { sucesso = true, templateDigital = templateSimulado });
        }

        [HttpPost("digital/registrar")]
        public async Task<IActionResult> RegistrarDigital([FromBody] RegistrarBiometriaDigitalRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sucesso = await _biometriaService.RegistrarDigitalAsync(request.PessoaId, request.TemplateDigital);
            if (sucesso)
            {
                return Ok(new { sucesso = true, mensagem = "Biometria digital registrada." });
            }
            return BadRequest(new { sucesso = false, mensagem = "Falha ao registrar biometria digital." });
        }

        [HttpPost("digital/validar")]
        public async Task<IActionResult> ValidarDigital([FromBody] ValidarBiometriaDigitalRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (sucesso, mensagem, similaridade) = await _biometriaService.ValidarDigitalAsync(request.Cpf, request.TemplateDigitalCapturado);

            return Ok(new { sucesso, mensagem, similaridade });
        }
    }
}
