using CrediV1_Prueba.Entities;

namespace CrediV1_Prueba.Interfaces
{
    public interface IOtherServices
    {
        public string GenerarHtml(UsuarioEnt entidad);
        public string Encrypt(string texto);
        public string Decrypt(string texto);
        public void EnviarCorreo(string Destinatario, string Asunto, string Mensaje);
        public string GenerarHtmlBienvenida(UsuarioEnt entidad);

    }
}
