using Quod.Antifraude.Core.Repositories;
using Quod.Antifraude.Core.Settings;
using Quod.Antifraude.Infrastructure.Repositories;
using Quod.Antifraude.Services.Detection;
using Quod.Antifraude.Services.Notification;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a controllers e converte enums para strings no JSON
builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        // Faz com que todos os enums sejam serializados/deserializados como string
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configurações fortemente tipadas
builder.Services
    .Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));
builder.Services.Configure<NotificationSettings>(
    builder.Configuration.GetSection("NotificationSettings"));

// Injeção de dependências
builder.Services.AddSingleton<IValidacaoRepository, ValidacaoRepository>();
builder.Services.AddScoped<IFraudDetectionService, FraudDetectionService>();
builder.Services.AddHttpClient<INotificationService, NotificationService>();

// Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do MongoDB
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var app = builder.Build();

// Habilita Swagger em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
