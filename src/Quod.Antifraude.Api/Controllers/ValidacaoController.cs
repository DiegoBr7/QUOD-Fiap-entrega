// src/Quod.Antifraude.Api/Controllers/ValidacaoController.cs
using Microsoft.AspNetCore.Mvc;
using Quod.Antifraude.Api.Models;
using Quod.Antifraude.Core.Models;
using Quod.Antifraude.Core.Repositories;
using Quod.Antifraude.Services.Detection;
using Quod.Antifraude.Services.Notification;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>Simula validação de biometria facial.</summary>
        [HttpPost("facial")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            OperationId = "ValidacaoFacial",
            Summary = "Valida imagem facial",
            Description = "Detecta deepfake, máscara, liveness etc."    
        )]
        public async Task<IActionResult> Facial([FromForm] FacialRequest req)
        {
            // chama o serviço que retorna o resultado completo
            var result = await _fraudSvc.ValidateFacialAsync(req.Imagem);

            // persiste o registro
            var reg = new RegistroValidacao
            {
                TransacaoId = req.TransacaoId,
                DataCaptura = req.DataCaptura,
                Dispositivo = req.Dispositivo,
                MetadadosLocalizacao = req.MetadadosLocalizacao,
                EhFraude = result.EhFraude,
                TipoFraude = result.TipoFraude,
                DataProcessamento = DateTime.UtcNow
            };
            await _repo.SaveAsync(reg);
            if (result.EhFraude)
                await _notiSvc.NotifyFraudAsync(reg);

            // devolve payload estendido para o cliente
            return Ok(new
            {
                reg.TransacaoId,
                reg.EhFraude,
                reg.TipoFraude,
                result.Confidence,
                result.FaceCount,
                result.LivenessScore,
                result.Observacao
            });
        }

        /// <summary>Simula validação de biometria digital.</summary>

        [HttpPost("digital")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            OperationId = "ValidacaoDigital",
            Summary = "Valida impressão digital",
            Description = "Envia scan de digitais para detecção de deepfake/máscara etc."
        )]
        public async Task<IActionResult> Digital([FromForm] DigitalRequest req)
        {
            var result = await _fraudSvc.ValidateDigitalAsync(req.ImagemDigital);

            var reg = new RegistroValidacao
            {
                TransacaoId = req.TransacaoId,
                DataCaptura = req.DataCaptura,
                Dispositivo = req.Dispositivo,
                MetadadosLocalizacao = req.MetadadosLocalizacao,
                EhFraude = result.EhFraude,
                TipoFraude = result.TipoFraude,
                DataProcessamento = DateTime.UtcNow
            };
            await _repo.SaveAsync(reg);
            if (result.EhFraude)
                await _notiSvc.NotifyFraudAsync(reg);

            return Ok(new
            {
                reg.TransacaoId,
                reg.EhFraude,
                reg.TipoFraude,
                result.Confidence,
                result.Observacao
            });
        }



        /// <summary>Simula análise de documentos (Documentoscopia).</summary>
        [HttpPost("documentos")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            OperationId = "ValidacaoDocumentos",
            Summary = "Valida documento e selfie",
            Description = "Detecta deepfake, máscara, foto de foto, etc."
        )]
        public async Task<IActionResult> Documentos([FromForm] DocumentosRequest req)
        {
            var (ehFraude, tipoFraude) = await _fraudSvc
                .DetectDocumentosAsync(
                    req.Documento.OpenReadStream(),
                    req.ImagemFace.OpenReadStream());

            var reg = new RegistroValidacao
            {
                TransacaoId = req.TransacaoId,
                DataCaptura = req.DataCaptura,
                Dispositivo = req.Dispositivo,
                MetadadosLocalizacao = req.MetadadosLocalizacao,
                EhFraude = ehFraude,
                TipoFraude = tipoFraude,
                DataProcessamento = DateTime.UtcNow
            };
            await _repo.SaveAsync(reg);
            if (ehFraude)
                await _notiSvc.NotifyFraudAsync(reg);

            return Ok(reg);
        }
    }
}
