using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quod.Antifraude.Services.Documentoscopia;
using Quod.Antifraude.Core.Models;

namespace Quod.Antifraude.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentosController : ControllerBase
    {
        private readonly DocumentAnalysisService _analysisSvc;

        public DocumentosController(DocumentAnalysisService analysisSvc)
        {
            _analysisSvc = analysisSvc;
        }

        [HttpPost("analisar")]
        [Consumes("multipart/form-data")]
        public IActionResult Analisar(IFormFile imagem)
        {
            if (imagem == null || imagem.Length == 0)
                return BadRequest(new { sucesso = false, motivo = "Envie a foto do documento." });

            // Salva em arquivo temporário
            var tempFile = Path.Combine(Path.GetTempPath(),
                              $"{Guid.NewGuid()}{Path.GetExtension(imagem.FileName)}");
            using (var fs = System.IO.File.Create(tempFile))
                imagem.CopyTo(fs);

            // Chama o novo método
            var (cpfExtraido, pessoa) = _analysisSvc.AnalyzeWithCpf(tempFile);
            System.IO.File.Delete(tempFile);

            // Se não extraiu nada
            if (cpfExtraido == null)
                return BadRequest(new { sucesso = false, motivo = "Não foi possível extrair um CPF da imagem." });

            // Se extraiu mas não encontrou na base
            if (pessoa == null)
                return Ok(new { sucesso = false, motivo = "Documento válido, mas CPF não cadastrado.", cpf = cpfExtraido });

            // Sucesso total
            return Ok(new
            {
                sucesso = true,
                cpfExtraido,
                dadosPessoa = pessoa
            });
        }
    }
}
