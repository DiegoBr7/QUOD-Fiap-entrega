// src/Quod.Antifraude.Api/Controllers/ValidacaoController.cs
using Microsoft.AspNetCore.Mvc;
using Quod.Antifraude.Api.Models;
using Quod.Antifraude.Core.Models;
using Quod.Antifraude.Core.Repositories;
using Quod.Antifraude.Services.Detection;
using Quod.Antifraude.Services.Notification;
using System.Threading.Tasks;

namespace Quod.Antifraude.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidacaoController : ControllerBase
    {
        private readonly IFraudDetectionService _fraudSvc;
        private readonly IValidacaoRepository _repo;
        private readonly INotificationService _notiSvc;

        public ValidacaoController(
            IFraudDetectionService fraudSvc,
            IValidacaoRepository repo,
            INotificationService notiSvc)
        {
            _fraudSvc = fraudSvc;
            _repo = repo;
            _notiSvc = notiSvc;
        }

        [HttpPost("facial")]
        [Consumes("multipart/form-data")]
        public Task<IActionResult> Facial([FromForm] ValidacaoRequest req)
            => ProcessAsync(req, isDocument: false);

        [HttpPost("digital")]
        [Consumes("multipart/form-data")]
        public Task<IActionResult> Digital([FromForm] ValidacaoRequest req)
            => ProcessAsync(req, isDocument: false);

        [HttpPost("documentos")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Documentos([FromForm] DocumentosRequest req)
        {
            // rodamos o DetectDocumentosAsync:
            var (ehFraude, tipoFraude) = await _fraudSvc
                .DetectDocumentosAsync(
                    req.Documento.OpenReadStream(),
                    req.ImagemFace.OpenReadStream());

            return await FinalizeAsync(req, ehFraude, tipoFraude);
        }

        // roteiriza biometria facial/digital
        private async Task<IActionResult> ProcessAsync(
            ValidacaoRequest req,
            bool isDocument)
        {
            var (ehFraude, tipoFraude) =
                await _fraudSvc.DetectAsync(req.Imagem);

            return await FinalizeAsync(req, ehFraude, tipoFraude);
        }

        // faz persistência, notificação e devolve
        private async Task<IActionResult> FinalizeAsync(
            BaseCapturaDto req,
            bool ehFraude,
            TipoFraude? tipoFraude)
        {
            var registro = new RegistroValidacao
            {
                TransacaoId = req.TransacaoId,
                TipoBiometria = req.TipoBiometria,
                SimularFraude = req.SimularFraude,
                DataCaptura = req.DataCaptura,
                Dispositivo = req.Dispositivo,
                MetadadosLocalizacao = req.MetadadosLocalizacao,
                //EhFraude = ehFraude,
                TipoFraude = tipoFraude,
                DataProcessamento = DateTime.UtcNow
            };

            await _repo.SaveAsync(registro);
            if (registro.SimularFraude == SimularFraude.True)
                await _notiSvc.NotifyFraudAsync(registro);

            return Ok(registro);
        }
    }
}
