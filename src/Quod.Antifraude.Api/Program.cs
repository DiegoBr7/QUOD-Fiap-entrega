using Quod.Antifraude.Core.Models;
using Quod.Antifraude.Core.Repositories;
using Quod.Antifraude.Core.Settings;
using Quod.Antifraude.Infrastructure.Repositories;
using Quod.Antifraude.Services.Detection;
using Quod.Antifraude.Services.Documentoscopia;
using Quod.Antifraude.Services.Notification;
using Quod.Antifraude.Api.Filters;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1) Controllers + enums como string
builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
    );

// 2) Configurações tipadas
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));
builder.Services.Configure<NotificationSettings>(
    builder.Configuration.GetSection("NotificationSettings"));

// 3) Antifraude e Notificação
builder.Services.AddSingleton<IValidacaoRepository, ValidacaoRepository>();
builder.Services.AddScoped<IFraudDetectionService, FraudDetectionService>();
builder.Services.AddHttpClient<INotificationService, NotificationService>();

// 4) MongoDB no DI
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var s = sp.GetRequiredService<IOptions<MongoSettings>>().Value; 
    return new MongoClient(s.ConnectionString);
});
builder.Services.AddScoped(sp =>
{
    var s = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    return sp.GetRequiredService<IMongoClient>().GetDatabase(s.DatabaseName);
});
builder.Services.AddScoped<IMongoCollection<Pessoa>>(sp =>
    sp.GetRequiredService<IMongoDatabase>().GetCollection<Pessoa>("DocumentosDB"));

// 5) Documentoscopia
builder.Services.AddSingleton<ImageProcessingService>();
builder.Services.AddScoped<CpfExtractionService>();
builder.Services.AddSingleton<CpfValidationService>();
builder.Services.AddScoped<DocumentAnalysisService>();

// 6) Swagger/OpenAPI com filtro de upload
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 7) Bson GUID
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var app = builder.Build();

// 8) Habilita o Swagger UI em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
