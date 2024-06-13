using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrediV1_Prueba.Others
{


    public class OtherServices: IOtherServices
    {
        private readonly IConfiguration _configuration;
        private IHostEnvironment _hostingEnvironment; //para saber ubicación de app


        public OtherServices(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }


        public string GenerarHtml(UsuarioEnt entidad )
        {
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Others\\RecuperarContrasenna.html");
            string htmlArchivo = System.IO.File.ReadAllText(rutaArchivo);
            htmlArchivo = htmlArchivo.Replace("@@Nombre", entidad.nombre);
            htmlArchivo = htmlArchivo.Replace("@@Link", "https://localhost:7001/Inicio/CambiarContrasenna?q=" + Encrypt(entidad.idUsuario.ToString()));

            return htmlArchivo;
        }
        public string GenerarHtmlBienvenida(UsuarioEnt entidad)
        {
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Others\\Bienvenida.html");
            string htmlArchivo = System.IO.File.ReadAllText(rutaArchivo);
            htmlArchivo = htmlArchivo.Replace("@@ClaveTemporal", entidad.contrasenna);

            return htmlArchivo;
        }

        public string Encrypt(string texto)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("EzfjS0IHnNSjv0jo");
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(texto);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string Decrypt(string texto)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(texto);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("EzfjS0IHnNSjv0jo");
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public void EnviarCorreo(string Destinatario, string Asunto, string Mensaje)
        {
            string correoSMTP = _configuration.GetSection("Correo:correoSMTP").Value;
            string claveSMTP = _configuration.GetSection("Correo:claveSMTP").Value;

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(Destinatario));
            msg.From = new MailAddress(correoSMTP);
            msg.Subject = Asunto;
            msg.Body = Mensaje;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(correoSMTP, claveSMTP);
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Send(msg);
        }





    }
}
