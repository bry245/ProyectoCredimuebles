using CrediV1_Prueba.Entities;

namespace CrediV1_Prueba.Interfaces
{
    public interface IEmailService
    {
        public Task SendNotificationEmailAsync(string toEmail, string message, string nombreCompletoUsuario);
        public Task SendPasswordResetEmailAsync(string toEmail, string resetToken);
        public void SendNotificationAdministradoresAsync(string toEmail, string message, string nombreCompletoProveedor, byte[] pdfContent);

        public  Task SendNotificationProveedorAsync(string toEmail, string message, string nombreCompletoProveedor);



        public byte[] GenerarPDFPedido(IEnumerable<PedidoEnt> productos, string proveedor);


    }
}
