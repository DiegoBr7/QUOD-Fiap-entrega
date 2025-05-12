using Quod.Antifraude.Core.Repositories;
using Quod.Antifraude.Core.Settings;
using Quod.Antifraude.Infrastructure.Repositories;
using Quod.Antifraude.Services.Detection;
using Quod.Antifraude.Services.Notification;
using System.Text.Json.Serialization;
using Quod.Antifraude.Api.Filters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1) Controllers + enum-as-string no JSON
builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// 2) Configurações fortemente tipadas
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));
builder.Services.Configure<NotificationSettings>(
    builder.Configuration.GetSection("NotificationSettings"));

// 3) Injeção de dependências
builder.Services.AddSingleton<IValidacaoRepository, ValidacaoRepository>();
builder.Services.AddScoped<IFraudDetectionService, FraudDetectionService>();
builder.Services.AddHttpClient<INotificationService, NotificationService>();

// 4) Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quod.Antifraude.Api",
        Version = "v1",
        Description = "API de simulação de antifraude"
    });
    c.EnableAnnotations();
    c.SchemaFilter<DisplaySchemaFilter>();
});

// 5) Serializador de GUID para MongoDB
BsonSerializer.RegisterSerializer(
    new GuidSerializer(GuidRepresentation.Standard));

var app = builder.Build();

// 1) HTTPS e Roteamento
app.UseHttpsRedirection();
app.UseRouting();

// 2) Swagger (sempre antes do StaticFiles)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quod.Antifraude.Api v1");
        c.RoutePrefix = "swagger";
    });
}

// 3) Static files – mas restringindo DefaultFiles ao root apenas
var defaultFilesOptions = new DefaultFilesOptions();
defaultFilesOptions.DefaultFileNames.Clear();
// Não adiciona subpastas, logo /swagger não será interceptado aqui
app.UseDefaultFiles(defaultFilesOptions);
app.UseStaticFiles();

// 4) Autorização e Controllers
app.UseAuthorization();
app.MapControllers();

// 5) Fallback para SPA em /demo
//app.MapFallback(context =>
//{
//context.Response.Redirect("   ");
//return Task.CompletedTask;
//});

app.Run();