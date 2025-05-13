using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Tesseract;

namespace Quod.Antifraude.Services.Documentoscopia
{
    public class CpfExtractionService
    {
        private readonly string _tessDataPath;

        public CpfExtractionService(IWebHostEnvironment env)
        {
            // assume que você criou a pasta tessdata/ na raiz do projeto API
            _tessDataPath = Path.Combine(env.ContentRootPath, "tessdata");
        }

        /// <summary>
        /// Retorna o CPF (somente dígitos) ou null se não encontrar.
        /// </summary>
        public string? ExtractCpf(string imagePath)
        {
            using var engine = new TesseractEngine(_tessDataPath, "por", EngineMode.LstmOnly);
            using var pix = Pix.LoadFromFile(imagePath);
            using var page = engine.Process(pix);
            var text = page.GetText() ?? string.Empty;

            var match = Regex.Match(text, @"\d{3}\.?\d{3}\.?\d{3}-?\d{2}");
            if (!match.Success) return null;

            return match.Value
                        .Replace(".", "")
                        .Replace("-", "");
        }
    }
}
