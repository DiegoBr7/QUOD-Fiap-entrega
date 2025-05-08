using Quod.Antifraude.Core.Repositories;
using Quod.Antifraude.Core.Settings;
using Quod.Antifraude.Infrastructure.Repositories;
using Quod.Antifraude.Services.Detection;
using Quod.Antifraude.Services.Notification;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        // Faz com que todos os enums sejam serializados/deserializados como string
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// configurações fortemente tipadas
builder.Services
    .Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));
builder.Services.Configure<NotificationSettings>(
    builder.Configuration.GetSection("NotificationSettings"));

// injeção de dependências
builder.Services.AddSingleton<IValidacaoRepository, ValidacaoRepository>();
builder.Services.AddScoped<IFraudDetectionService, FraudDetectionService>();
builder.Services.AddHttpClient<INotificationService, NotificationService>();

// controllers + Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
