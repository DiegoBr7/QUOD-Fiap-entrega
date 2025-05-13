// src/Quod.Antifraude.Api/Models/BiometriaDigitalRequest.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Quod.Antifraude.Api.Models
{
    public class RegistrarBiometriaDigitalRequest
    {
        [Required]
        public Guid PessoaId { get; set; } // ou cpf, depende de como a pessoa é identificada

        [Required]
        public string? TemplateDigital { get; set; } // Template da digital capturada/simulada
    }

    public class ValidarBiometriaDigitalRequest
    {
        [Required]
        public string? Cpf { get; set; }

        [Required]
        public string? TemplateDigitalCapturado { get; set; }
    }
}
