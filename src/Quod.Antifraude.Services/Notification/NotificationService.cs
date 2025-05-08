using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Quod.Antifraude.Core.Models;
using Quod.Antifraude.Core.Settings;

namespace Quod.Antifraude.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _http;
        private readonly NotificationSettings _opts;

        public NotificationService(HttpClient http, IOptions<NotificationSettings> opts)
        {
            _http = http;
            _opts = opts.Value;
            _http.BaseAddress = new Uri(_opts.BaseUrl);
        }

        public Task NotifyFraudAsync(RegistroValidacao registro) =>
            _http.PostAsJsonAsync(_opts.EndpointFraude, registro);
    }
}
