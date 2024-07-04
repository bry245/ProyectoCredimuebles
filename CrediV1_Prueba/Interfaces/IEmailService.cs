namespace CrediV1_Prueba.Interfaces
{
    public interface IEmailService
    {
        public Task SendNotificationEmailAsync(string toEmail, string message, string nombreCompletoUsuario);
        public Task SendPasswordResetEmailAsync(string toEmail, string resetToken);
    }
}
