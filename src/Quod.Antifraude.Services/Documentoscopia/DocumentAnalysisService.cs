using System.IO;
using Quod.Antifraude.Core.Models;
using MongoDB.Driver;

namespace Quod.Antifraude.Services.Documentoscopia
{
    public class DocumentAnalysisService
    {
        private readonly ImageProcessingService _imgSvc;
        private readonly CpfExtractionService _ocrSvc;
        private readonly CpfValidationService _valSvc;
        private readonly IMongoCollection<Pessoa> _pessoas;

        public DocumentAnalysisService(
            ImageProcessingService imgSvc,
            CpfExtractionService ocrSvc,
            CpfValidationService valSvc,
            IMongoCollection<Pessoa> pessoas)
        {
            _imgSvc = imgSvc;
            _ocrSvc = ocrSvc;
            _valSvc = valSvc;
            _pessoas = pessoas;
        }

        /// <summary>
        /// Tenta extrair e validar o CPF da imagem.
        /// Se extrair e for válido, retorna (cpf, objeto Pessoa) – sendo pessoa null se não existir na base.
        /// Se não extrair, retorna (null, null).
        /// Se extrair mas for inválido, retorna (cpfExtraido, null).
        /// </summary>
        public (string? cpfExtraido, Pessoa? pessoa) AnalyzeWithCpf(string inputImagePath)
        {
            // Pré-processa a imagem
            var temp = Path.Combine(Path.GetTempPath(), "doc_pre.png");
            _imgSvc.PreprocessImage(inputImagePath, temp);

            // Extrai o texto e captura o CPF
            var cpf = _ocrSvc.ExtractCpf(temp);
            if (cpf == null)
                return (null, null);

            // Se extraiu mas os dígitos verificadores falharem
            if (!_valSvc.IsValid(cpf))
                return (cpf, null);

            // Consulta no MongoDB
            var filtro = Builders<Pessoa>.Filter.Eq(p => p.Cpf, cpf);
            var pessoa = _pessoas.Find(filtro).FirstOrDefault();

            return (cpf, pessoa);
        }
    }
}
