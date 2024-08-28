using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Colors;

namespace CrediV1_Prueba.Models
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _host;

        public EmailService(IConfiguration configuration, IHostEnvironment host)
        {
            _configuration = configuration;
            _host = host;
        }

        public byte[] GenerarPDFPedido(IEnumerable<PedidoEnt> productos, string nombreproveedor)
        {
            using (var memoryStream = new MemoryStream())
            {
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Agregar el logo de la empresa
                string logoPath = Path.Combine(_host.ContentRootPath, "wwwroot", "images", "logoc2.PNG");
                if (File.Exists(logoPath))
                {
                    var imageData = ImageDataFactory.Create(logoPath);
                    var image = new Image(imageData).SetFixedPosition(500, 750).ScaleToFit(100, 100);
                    document.Add(image);
                }
                else
                {
                    // Optional: log or handle the case where the logo is not found
                    Console.WriteLine($"Logo not found at: {logoPath}");
                }

                // Encabezado de la factura
                document.Add(new Paragraph("PEDIDO")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(20)
                    .SetBold());

                // Información del proveedor
                var primerProducto = productos.First();
                document.Add(new Paragraph($"Proveedor: {primerProducto.nombreProveedor}")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(12)
                    .SetBold());
                document.Add(new Paragraph($"Teléfono: {primerProducto.Telefono}")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(12));
                document.Add(new Paragraph($"Dirección: {primerProducto.direccionProveedor}")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(12));

                // Información del empleado
                document.Add(new Paragraph($"Empleado: {primerProducto.nombreEmpleado}")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(12)
                    .SetBold());
                document.Add(new Paragraph($"Cédula: {primerProducto.Cedula}")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(12));

                // Espacio entre secciones
                document.Add(new Paragraph("\n"));

                // Información del pedido
                var infoPedido = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
                infoPedido.AddCell(new Paragraph($"Nº DE FACTURA\nES-001").SetFontSize(10));
                infoPedido.AddCell(new Paragraph($"FECHA\n{DateTime.Now:dd.MM.yyyy}").SetFontSize(10));
                document.Add(infoPedido);

                document.Add(new Paragraph("\n"));

                // Tabla de productos
                var table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 4, 2, 2 })).UseAllAvailableWidth();
                table.SetBackgroundColor(new DeviceRgb(240, 240, 240));

                table.AddHeaderCell(new Cell().Add(new Paragraph("CANT.")).SetBackgroundColor(new DeviceRgb(200, 200, 200)).SetBold());
                table.AddHeaderCell(new Cell().Add(new Paragraph("DESCRIPCIÓN")).SetBackgroundColor(new DeviceRgb(200, 200, 200)).SetBold());
                table.AddHeaderCell(new Cell().Add(new Paragraph("PRECIO UNITARIO")).SetBackgroundColor(new DeviceRgb(200, 200, 200)).SetBold());
                table.AddHeaderCell(new Cell().Add(new Paragraph("IMPORTE")).SetBackgroundColor(new DeviceRgb(200, 200, 200)).SetBold());

                foreach (var producto in productos)
                {
                    table.AddCell(new Cell().Add(new Paragraph(producto.cantidad.ToString())));
                    table.AddCell(new Cell().Add(new Paragraph(producto.nombreProducto ?? "N/A")));
                    table.AddCell(new Cell().Add(new Paragraph(producto.montoUnitario.ToString("C"))));
                    table.AddCell(new Cell().Add(new Paragraph(producto.montoTotalProducto.ToString("C"))));
                }

                document.Add(table);
                document.Add(new Paragraph("\n"));

                // Total del pedido
                float totalPedido = (float)(productos.FirstOrDefault()?.montoTotalPedido);
                document.Add(new Paragraph($"TOTAL: {totalPedido.ToString("C")}")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(16)
                    .SetBold());

                // Condiciones de pago
                document.Add(new Paragraph("\nCONDICIONES Y FORMA DE PAGO\nEl pago se realizará en un plazo de 15 días.")
                    .SetFontSize(10)
                    .SetBold()
                    .SetFontColor(DeviceRgb.RED));

                document.Close();
                return memoryStream.ToArray();
            }
        }







        public void SendNotificationAdministradoresAsync(string toEmail, string message, string nombreCompletoProveedor, byte[] pdfContent)
        {
            string cuenta = _configuration.GetSection("Smtp:User").Value!;
            string contrasenna = _configuration.GetSection("Smtp:Password").Value!;

            MailMessage messages = new MailMessage();
            messages.From = new MailAddress(cuenta);
            messages.To.Add(new MailAddress(toEmail));
            messages.Subject = "Pedido Recibido";
            messages.Body = message;
            messages.Priority = MailPriority.Normal;
            messages.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential(cuenta, contrasenna);
            client.EnableSsl = true;
            using (var stream = new MemoryStream(pdfContent))
            {
                Attachment pdfAttachment = new Attachment(stream, "DetallesPedido.pdf", "application/pdf");
                messages.Attachments.Add(pdfAttachment);
                client.Send(messages);
            }
        }

        public async Task SendNotificationEmailAsync(string toEmail, string message, string nombreCompletoUsuario)
        {
            var smtpConfig = _configuration.GetSection("Smtp");
            var fromEmail = smtpConfig["User"];
            var fromPassword = smtpConfig["Password"];
            var smtpHost = smtpConfig["Host"];
            var smtpPort = int.Parse(smtpConfig["Port"]);
            var enableSsl = bool.Parse(smtpConfig["EnableSsl"]);

            var from = new MailAddress(fromEmail, "Credimuebles");
            var to = new MailAddress(toEmail);
            var subject = " Credimuebles Notificación Importante";

            var body = $@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <meta name=""x-apple-disable-message-reformatting"" />
    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
    <meta name=""color-scheme"" content=""light dark"" />
    <meta name=""supported-color-schemes"" content=""light dark"" />
    <title></title>
    <style type=""text/css"" rel=""stylesheet"" media=""all"">
        /* Base ------------------------------ */

        @import url(""https://fonts.googleapis.com/css?family=Nunito+Sans:400,700&display=swap"");
        body {{
            width: 100% !important;
            height: 100%;
            margin: 0;
            -webkit-text-size-adjust: none;
        }}

        a {{
            color: #3869D4;
        }}

        a img {{
            border: none;
        }}

        td {{
            word-break: break-word;
        }}

        .preheader {{
            display: none !important;
            visibility: hidden;
            mso-hide: all;
            font-size: 1px;
            line-height: 1px;
            max-height: 0;
            max-width: 0;
            opacity: 0;
            overflow: hidden;
        }}
        /* Type ------------------------------ */

        body,
        td,
        th {{
            font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif;
        }}

        h1 {{
            margin-top: 0;
            color: #333333;
            font-size: 22px;
            font-weight: bold;
            text-align: left;
        }}

        h2 {{
            margin-top: 0;
            color: #333333;
            font-size: 16px;
            font-weight: bold;
            text-align: left;
        }}

        h3 {{
            margin-top: 0;
            color: #333333;
            font-size: 14px;
            font-weight: bold;
            text-align: left;
        }}

        td,
        th {{
            font-size: 16px;
        }}

        p,
        ul,
        ol,
        blockquote {{
            margin: .4em 0 1.1875em;
            font-size: 16px;
            line-height: 1.625;
        }}

        p.sub {{
            font-size: 13px;
        }}
        /* Utilities ------------------------------ */

        .align-right {{
            text-align: right;
        }}

        .align-left {{
            text-align: left;
        }}

        .align-center {{
            text-align: center;
        }}

        .u-margin-bottom-none {{
            margin-bottom: 0;
        }}
        /* Buttons ------------------------------ */

        .button {{
            background-color: #3869D4;
            border-top: 10px solid #3869D4;
            border-right: 18px solid #3869D4;
            border-bottom: 10px solid #3869D4;
            border-left: 18px solid #3869D4;
            display: inline-block;
            color: #FFF;
            text-decoration: none;
            border-radius: 3px;
            box-shadow: 0 2px 3px rgba(0, 0, 0, 0.16);
            -webkit-text-size-adjust: none;
            box-sizing: border-box;
        }}

        .button--green {{
            background-color: #22BC66;
            border-top: 10px solid #22BC66;
            border-right: 18px solid #22BC66;
            border-bottom: 10px solid #22BC66;
            border-left: 18px solid #22BC66;
        }}

        .button--red {{
            background-color: #FF6136;
            border-top: 10px solid #FF6136;
            border-right: 18px solid #FF6136;
            border-bottom: 10px solid #FF6136;
            border-left: 18px solid #FF6136;
        }}

        @media only screen and (max-width: 500px) {{
            .button {{
                width: 100% !important;
                text-align: center !important;
            }}
        }}
        /* Attribute list ------------------------------ */

        .attributes {{
            margin: 0 0 21px;
        }}

        .attributes_content {{
            background-color: #F4F4F7;
            padding: 16px;
        }}

        .attributes_item {{
            padding: 0;
        }}
        /* Related Items ------------------------------ */

        .related {{
            width: 100%;
            margin: 0;
            padding: 25px 0 0 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .related_item {{
            padding: 10px 0;
            color: #CBCCCF;
            font-size: 15px;
            line-height: 18px;
        }}

        .related_item-title {{
            display: block;
            margin: .5em 0 0;
        }}

        .related_item-thumb {{
            display: block;
            padding-bottom: 10px;
        }}

        .related_heading {{
            border-top: 1px solid #CBCCCF;
            text-align: center;
            padding: 25px 0 10px;
        }}
        /* Discount Code ------------------------------ */

        .discount {{
            width: 100%;
            margin: 0;
            padding: 24px;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
            background-color: #F4F4F7;
            border: 2px dashed #CBCCCF;
        }}

        .discount_heading {{
            text-align: center;
        }}

        .discount_body {{
            text-align: center;
            font-size: 15px;
        }}
        /* Social Icons ------------------------------ */

        .social {{
            width: auto;
        }}

        .social td {{
            padding: 0;
            width: auto;
        }}

        .social_icon {{
            height: 20px;
            margin: 0 8px 10px 8px;
            padding: 0;
        }}
        /* Data table ------------------------------ */

        .purchase {{
            width: 100%;
            margin: 0;
            padding: 35px 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .purchase_content {{
            width: 100%;
            margin: 0;
            padding: 25px 0 0 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .purchase_item {{
            padding: 10px 0;
            color: #51545E;
            font-size: 15px;
            line-height: 18px;
        }}

        .purchase_heading {{
            padding-bottom: 8px;
            border-bottom: 1px solid #EAEAEC;
        }}

        .purchase_heading p {{
            margin: 0;
            color: #85878E;
            font-size: 12px;
        }}

        .purchase_footer {{
            padding-top: 15px;
            border-top: 1px solid #EAEAEC;
        }}

        .purchase_total {{
            margin: 0;
            text-align: right;
            font-weight: bold;
            color: #333333;
        }}

        .purchase_total--label {{
            padding: 0 15px 0 0;
        }}

        body {{
            background-color: #FFF;
            color: #333;
        }}

        p {{
            color: #333;
        }}

        .email-wrapper {{
            width: 100%;
            margin: 0;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .email-content {{
            width: 100%;
            margin: 0;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}
        /* Masthead ----------------------- */

        .email-masthead {{
            padding: 25px 0;
            text-align: center;
        }}

        .email-masthead_logo {{
            width: 94px;
        }}

        .email-masthead_name {{
            font-size: 16px;
            font-weight: bold;
            color: #A8AAAF;
            text-decoration: none;
            text-shadow: 0 1px 0 white;
        }}
        /* Body ------------------------------ */

        .email-body {{
            width: 100%;
            margin: 0;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .email-body_inner {{
            width: 570px;
            margin: 0 auto;
            padding: 0;
            -premailer-width: 570px;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .email-footer {{
            width: 570px;
            margin: 0 auto;
            padding: 0;
            -premailer-width: 570px;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
            text-align: center;
        }}

        .email-footer p {{
            color: #A8AAAF;
        }}

        .body-action {{
            width: 100%;
            margin: 30px auto;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
            text-align: center;
        }}

        .body-sub {{
            margin-top: 25px;
            padding-top: 25px;
            border-top: 1px solid #EAEAEC;
        }}

        .content-cell {{
            padding: 35px;
        }}
        /*Media Queries ------------------------------ */

        @media only screen and (max-width: 600px) {{
            .email-body_inner,
            .email-footer {{
                width: 100% !important;
            }}
        }}

        @media (prefers-color-scheme: dark) {{
            body {{
                background-color: #333333 !important;
                color: #FFF !important;
            }}
            p,
            ul,
            ol,
            blockquote,
            h1,
            h2,
            h3,
            span,
            .purchase_item {{
                color: #FFF !important;
            }}
            .attributes_content,
            .discount {{
                background-color: #222 !important;
            }}
            .email-masthead_name {{
                text-shadow: none !important;
            }}
        }}

        :root {{
            color-scheme: light dark;
            supported-color-schemes: light dark;
        }}
    </style>
    <!--[if mso]>
    <style type=""text/css"">
        .f-fallback  {{
            font-family: Arial, sans-serif;
        }}
    </style>
    <![endif]-->
</head>
<body>
<span class=""preheader"">This is a receipt for your recent purchase on {{{{ purchase_date }}}}. No payment is due with this receipt.</span>
<table class=""email-wrapper"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
    <tr>
        <td align=""center"">
            <table class=""email-content"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                <tr>
                    <td class=""email-masthead"">
                        <a href=""https://example.com"" class=""f-fallback email-masthead_name"">
                            Credimuebles
                        </a>
                    </td>
                </tr>
                <!-- Email Body -->
                <tr>
                    <td class=""email-body"" width=""570"" cellpadding=""0"" cellspacing=""0"">
                        <table class=""email-body_inner"" align=""center"" width=""570"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                            <!-- Body content -->
                            <tr>
                                <td class=""content-cell"">
                                    <div class=""f-fallback"">
                                        <h1>Hola {nombreCompletoUsuario},</h1>

                                        <table class=""discount"" align=""center"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                                            <tr>
                                                <td align=""center"">
                                                    <h1 class=""f-fallback discount_heading"">Tenemos una  noticia que darte</h1>

                                                    <!-- Border based button
                                 https://litmus.com/blog/a-guide-to-bulletproof-buttons-in-email-design -->

                                                </td>
                                            </tr>
                                        </table>

                                        <p class=""f-fallback discount_body"">Gracias por el apoyo! te comentamos que {message} .</p>
                                      

                                        <!-- Action -->
                                        <table class=""body-action"" align=""center"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                                            <tr>
                                                <td align=""center"">
                                                    <!-- Border based button
                                 https://litmus.com/blog/a-guide-to-bulletproof-buttons-in-email-design -->

                                                </td>
                                            </tr>
                                        </table>
                                        <!-- Sub copy -->
                                        <table class=""body-sub"" role=""presentation"">
                                            <tr>
                                                <td>
                                                    <p class=""f-fallback sub""><strong>¿Tienes interes sobre esto?</strong> recuerda que puedes contactar con nosotros cuando quieras.</p>
                                                    <p class=""f-fallback sub"">Informacion de contacto abajo.</p>
                                                </td>
                                            </tr>

                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class=""email-footer"" align=""center"" width=""570"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                            <tr>
                                <td class=""content-cell"" align=""center"">
                                    <p class=""f-fallback sub align-center"">
                                        Credimuebles, Costa Rica
                                        <br>San Jose.
                                        <br>24335522
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</body>
</html>
";

            using (var smtp = new SmtpClient(smtpHost, smtpPort))
            {
                smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
                smtp.EnableSsl = enableSsl;

                var mailMessage = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(mailMessage);
            }
        }

        public async Task SendNotificationProveedorAsync(string toEmail, string message, string nombreCompletoProveedor)
        {
            var smtpConfig = _configuration.GetSection("Smtp");
            var fromEmail = smtpConfig["User"];
            var fromPassword = smtpConfig["Password"];
            var smtpHost = smtpConfig["Host"];
            var smtpPort = int.Parse(smtpConfig["Port"]);
            var enableSsl = bool.Parse(smtpConfig["EnableSsl"]);

            var from = new MailAddress(fromEmail, "Credimuebles");
            var to = new MailAddress(toEmail);
            var subject = " Credimuebles Notificación Importante";

            var body = $@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <meta name=""x-apple-disable-message-reformatting"" />
    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
    <meta name=""color-scheme"" content=""light dark"" />
    <meta name=""supported-color-schemes"" content=""light dark"" />
    <title></title>
    <style type=""text/css"" rel=""stylesheet"" media=""all"">
        /* Base ------------------------------ */

        @import url(""https://fonts.googleapis.com/css?family=Nunito+Sans:400,700&display=swap"");
        body {{
            width: 100% !important;
            height: 100%;
            margin: 0;
            -webkit-text-size-adjust: none;
        }}

        a {{
            color: #3869D4;
        }}

        a img {{
            border: none;
        }}

        td {{
            word-break: break-word;
        }}

        .preheader {{
            display: none !important;
            visibility: hidden;
            mso-hide: all;
            font-size: 1px;
            line-height: 1px;
            max-height: 0;
            max-width: 0;
            opacity: 0;
            overflow: hidden;
        }}
        /* Type ------------------------------ */

        body,
        td,
        th {{
            font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif;
        }}

        h1 {{
            margin-top: 0;
            color: #333333;
            font-size: 22px;
            font-weight: bold;
            text-align: left;
        }}

        h2 {{
            margin-top: 0;
            color: #333333;
            font-size: 16px;
            font-weight: bold;
            text-align: left;
        }}

        h3 {{
            margin-top: 0;
            color: #333333;
            font-size: 14px;
            font-weight: bold;
            text-align: left;
        }}

        td,
        th {{
            font-size: 16px;
        }}

        p,
        ul,
        ol,
        blockquote {{
            margin: .4em 0 1.1875em;
            font-size: 16px;
            line-height: 1.625;
        }}

        p.sub {{
            font-size: 13px;
        }}
        /* Utilities ------------------------------ */

        .align-right {{
            text-align: right;
        }}

        .align-left {{
            text-align: left;
        }}

        .align-center {{
            text-align: center;
        }}

        .u-margin-bottom-none {{
            margin-bottom: 0;
        }}
        /* Buttons ------------------------------ */

        .button {{
            background-color: #3869D4;
            border-top: 10px solid #3869D4;
            border-right: 18px solid #3869D4;
            border-bottom: 10px solid #3869D4;
            border-left: 18px solid #3869D4;
            display: inline-block;
            color: #FFF;
            text-decoration: none;
            border-radius: 3px;
            box-shadow: 0 2px 3px rgba(0, 0, 0, 0.16);
            -webkit-text-size-adjust: none;
            box-sizing: border-box;
        }}

        .button--green {{
            background-color: #22BC66;
            border-top: 10px solid #22BC66;
            border-right: 18px solid #22BC66;
            border-bottom: 10px solid #22BC66;
            border-left: 18px solid #22BC66;
        }}

        .button--red {{
            background-color: #FF6136;
            border-top: 10px solid #FF6136;
            border-right: 18px solid #FF6136;
            border-bottom: 10px solid #FF6136;
            border-left: 18px solid #FF6136;
        }}

        @media only screen and (max-width: 500px) {{
            .button {{
                width: 100% !important;
                text-align: center !important;
            }}
        }}
        /* Attribute list ------------------------------ */

        .attributes {{
            margin: 0 0 21px;
        }}

        .attributes_content {{
            background-color: #F4F4F7;
            padding: 16px;
        }}

        .attributes_item {{
            padding: 0;
        }}
        /* Related Items ------------------------------ */

        .related {{
            width: 100%;
            margin: 0;
            padding: 25px 0 0 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .related_item {{
            padding: 10px 0;
            color: #CBCCCF;
            font-size: 15px;
            line-height: 18px;
        }}

        .related_item-title {{
            display: block;
            margin: .5em 0 0;
        }}

        .related_item-thumb {{
            display: block;
            padding-bottom: 10px;
        }}

        .related_heading {{
            border-top: 1px solid #CBCCCF;
            text-align: center;
            padding: 25px 0 10px;
        }}
        /* Discount Code ------------------------------ */

        .discount {{
            width: 100%;
            margin: 0;
            padding: 24px;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
            background-color: #F4F4F7;
            border: 2px dashed #CBCCCF;
        }}

        .discount_heading {{
            text-align: center;
        }}

        .discount_body {{
            text-align: center;
            font-size: 15px;
        }}
        /* Social Icons ------------------------------ */

        .social {{
            width: auto;
        }}

        .social td {{
            padding: 0;
            width: auto;
        }}

        .social_icon {{
            height: 20px;
            margin: 0 8px 10px 8px;
            padding: 0;
        }}
        /* Data table ------------------------------ */

        .purchase {{
            width: 100%;
            margin: 0;
            padding: 35px 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .purchase_content {{
            width: 100%;
            margin: 0;
            padding: 25px 0 0 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .purchase_item {{
            padding: 10px 0;
            color: #51545E;
            font-size: 15px;
            line-height: 18px;
        }}

        .purchase_heading {{
            padding-bottom: 8px;
            border-bottom: 1px solid #EAEAEC;
        }}

        .purchase_heading p {{
            margin: 0;
            color: #85878E;
            font-size: 12px;
        }}

        .purchase_footer {{
            padding-top: 15px;
            border-top: 1px solid #EAEAEC;
        }}

        .purchase_total {{
            margin: 0;
            text-align: right;
            font-weight: bold;
            color: #333333;
        }}

        .purchase_total--label {{
            padding: 0 15px 0 0;
        }}

        body {{
            background-color: #FFF;
            color: #333;
        }}

        p {{
            color: #333;
        }}

        .email-wrapper {{
            width: 100%;
            margin: 0;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .email-content {{
            width: 100%;
            margin: 0;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}
        /* Masthead ----------------------- */

        .email-masthead {{
            padding: 25px 0;
            text-align: center;
        }}

        .email-masthead_logo {{
            width: 94px;
        }}

        .email-masthead_name {{
            font-size: 16px;
            font-weight: bold;
            color: #A8AAAF;
            text-decoration: none;
            text-shadow: 0 1px 0 white;
        }}
        /* Body ------------------------------ */

        .email-body {{
            width: 100%;
            margin: 0;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .email-body_inner {{
            width: 570px;
            margin: 0 auto;
            padding: 0;
            -premailer-width: 570px;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .email-footer {{
            width: 570px;
            margin: 0 auto;
            padding: 0;
            -premailer-width: 570px;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
            text-align: center;
        }}

        .email-footer p {{
            color: #A8AAAF;
        }}

        .body-action {{
            width: 100%;
            margin: 30px auto;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
            text-align: center;
        }}

        .body-sub {{
            margin-top: 25px;
            padding-top: 25px;
            border-top: 1px solid #EAEAEC;
        }}

        .content-cell {{
            padding: 35px;
        }}
        /*Media Queries ------------------------------ */

        @media only screen and (max-width: 600px) {{
            .email-body_inner,
            .email-footer {{
                width: 100% !important;
            }}
        }}

        @media (prefers-color-scheme: dark) {{
            body {{
                background-color: #333333 !important;
                color: #FFF !important;
            }}
            p,
            ul,
            ol,
            blockquote,
            h1,
            h2,
            h3,
            span,
            .purchase_item {{
                color: #FFF !important;
            }}
            .attributes_content,
            .discount {{
                background-color: #222 !important;
            }}
            .email-masthead_name {{
                text-shadow: none !important;
            }}
        }}

        :root {{
            color-scheme: light dark;
            supported-color-schemes: light dark;
        }}
    </style>
    <!--[if mso]>
    <style type=""text/css"">
        .f-fallback  {{
            font-family: Arial, sans-serif;
        }}
    </style>
    <![endif]-->
</head>
<body>

<table class=""email-wrapper"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
    <tr>
        <td align=""center"">
            <table class=""email-content"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                <tr>
                    <td class=""email-masthead"">
                        <a href=""https://example.com"" class=""f-fallback email-masthead_name"">
                            Credimuebles
                        </a>
                    </td>
                </tr>
                <!-- Email Body -->
                <tr>
                    <td class=""email-body"" width=""570"" cellpadding=""0"" cellspacing=""0"">
                        <table class=""email-body_inner"" align=""center"" width=""570"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                            <!-- Body content -->
                            <tr>
                                <td class=""content-cell"">
                                    <div class=""f-fallback"">
                                        <h1></h1>

                                        <table class=""discount"" align=""center"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                                            <tr>
                                                <td align=""center"">
                                                    <h1 class=""f-fallback discount_heading"">Tenemos una  noticia que darte</h1>

                                                    <!-- Border based button
                                 https://litmus.com/blog/a-guide-to-bulletproof-buttons-in-email-design -->

                                                </td>
                                            </tr>
                                        </table>

                                        <p class=""f-fallback discount_body"">Hola {nombreCompletoProveedor},<br><br>Espero que te encuentres bien. Quiero comentarte que {message}. Si tienes alguna pregunta o necesitas más información, no dudes en contactarnos. Agradecemos tu colaboración y esperamos poder seguir trabajando juntos.</p>

                                      

                                        <!-- Action -->
                                        <table class=""body-action"" align=""center"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                                            <tr>
                                                <td align=""center"">
                                                    <!-- Border based button
                                 https://litmus.com/blog/a-guide-to-bulletproof-buttons-in-email-design -->

                                                </td>
                                            </tr>
                                        </table>
                                        <!-- Sub copy -->
                                  
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class=""email-footer"" align=""center"" width=""570"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                            <tr>
                                <td class=""content-cell"" align=""center"">
                                    <p class=""f-fallback sub align-center"">
                                        Credimuebles, Costa Rica
                                        <br>San Jose.
                                        <br>24335522
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</body>
</html>
";

            using (var smtp = new SmtpClient(smtpHost, smtpPort))
            {
                smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
                smtp.EnableSsl = enableSsl;

                var mailMessage = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(mailMessage);
            }
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetToken)
        {
            var smtpConfig = _configuration.GetSection("Smtp");
            var fromEmail = smtpConfig["User"];
            var fromPassword = smtpConfig["Password"];
            var smtpHost = smtpConfig["Host"];
            var smtpPort = int.Parse(smtpConfig["Port"]);
            var enableSsl = bool.Parse(smtpConfig["EnableSsl"]);

            var from = new MailAddress(fromEmail, "CREDIMUEBLES");
            var to = new MailAddress(toEmail);
            var subject = "Restablecimiento de Contraseña - CREDIMUEBLES";

            var body = $@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <meta name=""x-apple-disable-message-reformatting"" />
    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
    <meta name=""color-scheme"" content=""light dark"" />
    <meta name=""supported-color-schemes"" content=""light dark"" />
    <title></title>
    <style type=""text/css"" rel=""stylesheet"" media=""all"">
        /* Base ------------------------------ */

        @import url(""https://fonts.googleapis.com/css?family=Nunito+Sans:400,700&display=swap"");
        body {{
            width: 100% !important;
            height: 100%;
            margin: 0;
            -webkit-text-size-adjust: none;
        }}

        a {{
            color: #3869D4;
        }}

        a img {{
            border: none;
        }}

        td {{
            word-break: break-word;
        }}

        .preheader {{
            display: none !important;
            visibility: hidden;
            mso-hide: all;
            font-size: 1px;
            line-height: 1px;
            max-height: 0;
            max-width: 0;
            opacity: 0;
            overflow: hidden;
        }}
        /* Type ------------------------------ */

        body,
        td,
        th {{
            font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif;
        }}

        h1 {{
            margin-top: 0;
            color: #333333;
            font-size: 22px;
            font-weight: bold;
            text-align: left;
        }}

        h2 {{
            margin-top: 0;
            color: #333333;
            font-size: 16px;
            font-weight: bold;
            text-align: left;
        }}

        h3 {{
            margin-top: 0;
            color: #333333;
            font-size: 14px;
            font-weight: bold;
            text-align: left;
        }}

        td,
        th {{
            font-size: 16px;
        }}

        p,
        ul,
        ol,
        blockquote {{
            margin: .4em 0 1.1875em;
            font-size: 16px;
            line-height: 1.625;
        }}

        p.sub {{
            font-size: 13px;
        }}
        /* Utilities ------------------------------ */

        .align-right {{
            text-align: right;
        }}

        .align-left {{
            text-align: left;
        }}

        .align-center {{
            text-align: center;
        }}

        .u-margin-bottom-none {{
            margin-bottom: 0;
        }}
        /* Buttons ------------------------------ */

        .button {{
            background-color: #3869D4;
            border-top: 10px solid #3869D4;
            border-right: 18px solid #3869D4;
            border-bottom: 10px solid #3869D4;
            border-left: 18px solid #3869D4;
            display: inline-block;
            color: #FFF;
            text-decoration: none;
            border-radius: 3px;
            box-shadow: 0 2px 3px rgba(0, 0, 0, 0.16);
            -webkit-text-size-adjust: none;
            box-sizing: border-box;
        }}

        .button--green {{
            background-color: #22BC66;
            border-top: 10px solid #22BC66;
            border-right: 18px solid #22BC66;
            border-bottom: 10px solid #22BC66;
            border-left: 18px solid #22BC66;
        }}

        .button--red {{
            background-color: #FF6136;
            border-top: 10px solid #FF6136;
            border-right: 18px solid #FF6136;
            border-bottom: 10px solid #FF6136;
            border-left: 18px solid #FF6136;
        }}

        @media only screen and (max-width: 500px) {{
            .button {{
                width: 100% !important;
                text-align: center !important;
            }}
        }}
        /* Attribute list ------------------------------ */

        .attributes {{
            margin: 0 0 21px;
        }}

        .attributes_content {{
            background-color: #F4F4F7;
            padding: 16px;
        }}

        .attributes_item {{
            padding: 0;
        }}
        /* Related Items ------------------------------ */

        .related {{
            width: 100%;
            margin: 0;
            padding: 25px 0 0 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .related_item {{
            padding: 10px 0;
            color: #CBCCCF;
            font-size: 15px;
            line-height: 18px;
        }}

        .related_item-title {{
            display: block;
            margin: .5em 0 0;
        }}

        .related_item-thumb {{
            display: block;
            padding-bottom: 10px;
        }}

        .related_heading {{
            border-top: 1px solid #CBCCCF;
            text-align: center;
            padding: 25px 0 10px;
        }}
        /* Discount Code ------------------------------ */

        .discount {{
            width: 100%;
            margin: 0;
            padding: 24px;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
            background-color: #F4F4F7;
            border: 2px dashed #CBCCCF;
        }}

        .discount_heading {{
            text-align: center;
        }}

        .discount_body {{
            text-align: center;
            font-size: 15px;
        }}
        /* Social Icons ------------------------------ */

        .social {{
            width: auto;
        }}

        .social td {{
            padding: 0;
            width: auto;
        }}

        .social_icon {{
            height: 20px;
            margin: 0 8px 10px 8px;
            padding: 0;
        }}
        /* Data table ------------------------------ */

        .purchase {{
            width: 100%;
            margin: 0;
            padding: 35px 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .purchase_content {{
            width: 100%;
            margin: 0;
            padding: 25px 0 0 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .purchase_item {{
            padding: 10px 0;
            color: #51545E;
            font-size: 15px;
            line-height: 18px;
        }}

        .purchase_heading {{
            padding-bottom: 8px;
            border-bottom: 1px solid #EAEAEC;
        }}

        .purchase_heading p {{
            margin: 0;
            color: #85878E;
            font-size: 12px;
        }}

        .purchase_footer {{
            padding-top: 15px;
            border-top: 1px solid #EAEAEC;
        }}

        .purchase_total {{
            margin: 0;
            text-align: right;
            font-weight: bold;
            color: #333333;
        }}

        .purchase_total--label {{
            padding: 0 15px 0 0;
        }}

        body {{
            background-color: #F2F4F6;
            color: #51545E;
        }}

        p {{
            color: #51545E;
        }}

        .email-wrapper {{
            width: 100%;
            margin: 0;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
            background-color: #F2F4F6;
        }}

        .email-content {{
            width: 100%;
            margin: 0;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}
        /* Masthead ----------------------- */

        .email-masthead {{
            padding: 25px 0;
            text-align: center;
        }}

        .email-masthead_logo {{
            width: 94px;
        }}

        .email-masthead_name {{
            font-size: 16px;
            font-weight: bold;
            color: #A8AAAF;
            text-decoration: none;
            text-shadow: 0 1px 0 white;
        }}
        /* Body ------------------------------ */

        .email-body {{
            width: 100%;
            margin: 0;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
        }}

        .email-body_inner {{
            width: 570px;
            margin: 0 auto;
            padding: 0;
            -premailer-width: 570px;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
            background-color: #FFFFFF;
        }}

        .email-footer {{
            width: 570px;
            margin: 0 auto;
            padding: 0;
            -premailer-width: 570px;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
            text-align: center;
        }}

        .email-footer p {{
            color: #A8AAAF;
        }}

        .body-action {{
            width: 100%;
            margin: 30px auto;
            padding: 0;
            -premailer-width: 100%;
            -premailer-cellpadding: 0;
            -premailer-cellspacing: 0;
            text-align: center;
        }}

        .body-sub {{
            margin-top: 25px;
            padding-top: 25px;
            border-top: 1px solid #EAEAEC;
        }}

        .content-cell {{
            padding: 45px;
        }}
        /*Media Queries ------------------------------ */

        @media only screen and (max-width: 600px) {{
            .email-body_inner,
            .email-footer {{
                width: 100% !important;
            }}
        }}

        @media (prefers-color-scheme: dark) {{
            body {{
                background-color: #333333 !important;
                color: #FFF !important;
            }}
            p,
            ul,
            ol,
            blockquote,
            h1,
            h2,
            h3,
            span,
            .purchase_item {{
                color: #FFF !important;
            }}
            .attributes_content,
            .discount {{
                background-color: #222 !important;
            }}
            .email-masthead_name {{
                text-shadow: none !important;
            }}
        }}
    </style>
    <!--[if mso]>
    <style type=""text/css"">
        .f-fallback  {{
            font-family: Arial, sans-serif;
        }}
    </style>
    <![endif]-->
</head>
<body>
    <span class=""preheader"">Use this link to reset your password. The link is only valid for 24 hours.</span>
    <table class=""email-wrapper"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
        <tr>
            <td align=""center"">
                <table class=""email-content"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                    <tr>
                        <td class=""email-masthead"">
                       
                          
                          </a>
                        </td>
                    </tr>
                    <!-- Email Body -->
                    <tr>
                        <td class=""email-body"" width=""100%"" cellpadding=""0"" cellspacing=""0"">
                            <table class=""email-body_inner"" align=""center"" width=""570"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                                <!-- Body content -->
                                <tr>
                                    <td class=""content-cell"">
                                        <div class=""f-fallback"">
                                            <h1>Credimuebles</h1>
                                            <p>Recientemente solicitaste restablecer tu contraseña para tu cuenta de la empresa. Ingrese el codigo, en la pagina solicitada. <strong>Este restablecimiento de contraseña solo es válido durante una hora.</strong></p>

                                            <!-- Action -->
                                            <table class=""body-action"" align=""center"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                                                <tr>
                                                    <td align=""center"">
                                                        <!-- Border based button
                        https://litmus.com/blog/a-guide-to-bulletproof-buttons-in-email-design -->
                                                        <table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" role=""presentation"">
                                                            <tr>
                                                                <td align=""center"">
                                                                    <p  class=""f-fallback button button--green"" target=""_blank"">{resetToken}</p>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <p>Por seguridad, si no has hecho ninguna peticion de cambio de contraseña, por favor ignora este emaill.</p>
                                            <p>Muchas Gracias,
                                                <br>Credimuebles</p>
                                            <!-- Sub copy -->
                                            <table class=""body-sub"" role=""presentation"">
                                                <tr>
                                                    <td>
                                                        <p class=""f-fallback sub"">Si estas teniendo problemas con el proceso, por favor concacta con algun administrador</p>
                                                 
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table class=""email-footer"" align=""center"" width=""570"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                                <tr>
                                    <td class=""content-cell"" align=""center"">
                                        <p class=""f-fallback sub align-center"">&copy; 2024  Credimuebles. All rights reserved.</p>
                                        <p class=""f-fallback sub align-center"">
                                           Costa Rica
                                            <br>22334422 San Jose.
                                         
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>

        


";

            using (var smtp = new SmtpClient(smtpHost, smtpPort))
            {
                smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
                smtp.EnableSsl = enableSsl;

                var message = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(message);
            }
        }


    }

}
