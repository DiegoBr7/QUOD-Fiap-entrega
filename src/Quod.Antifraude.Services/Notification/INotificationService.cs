using Quod.Antifraude.Core.Models;

namespace Quod.Antifraude.Services.Notification
{
    public interface INotificationService
    {
        /// <summary>
        /// Envia ao sistema externo a notificação de fraude.
        /// </summary>
        Task NotifyFraudAsync(RegistroValidacao registro);
    }
}
