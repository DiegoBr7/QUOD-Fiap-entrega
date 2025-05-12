// src/Quod.Antifraude.Api/Filters/DisplaySchemaFilter.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Quod.Antifraude.Api.Filters
{
    /// <summary>
    /// Lê o atributo [Display(Description="...")] dos DTOs e passa para a descrição do schema.
    /// </summary>
    public class DisplaySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // só aplica em tipos que possuem campo Description via [Display]
            var displayAttr = context.Type
                .GetCustomAttributes(typeof(DisplayAttribute), true)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            if (displayAttr != null && !string.IsNullOrWhiteSpace(displayAttr.Description))
            {
                schema.Description = displayAttr.Description;
            }

            // também podemos iterar pelos membros para injetar descrições em cada property:
            foreach (var prop in schema.Properties)
            {
                var memberInfo = context.Type.GetProperty(prop.Key);
                var memberDisplay = memberInfo?
                    .GetCustomAttributes(typeof(DisplayAttribute), true)
                    .OfType<DisplayAttribute>()
                    .FirstOrDefault();

                if (memberDisplay != null && !string.IsNullOrWhiteSpace(memberDisplay.Description))
                {
                    schema.Properties[prop.Key].Description = memberDisplay.Description;
                }
            }
        }
    }
}
