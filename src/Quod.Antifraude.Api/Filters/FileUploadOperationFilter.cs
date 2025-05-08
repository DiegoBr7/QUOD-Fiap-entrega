using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Quod.Antifraude.Api.Filters
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // só para POSTs (você pode refinar a lógica)
            if (context.ApiDescription.HttpMethod != "POST") return;

            // garante que exista um RequestBody
            operation.RequestBody ??= new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>()
            };

            // define multiparte/form-data com um campo "imagem"
            operation.RequestBody.Content["multipart/form-data"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["imagem"] = new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary",
                            Description = "Selecione o arquivo de imagem"
                        }
                    },
                    Required = new HashSet<string> { "imagem" }
                }
            };
        }
    }
}
