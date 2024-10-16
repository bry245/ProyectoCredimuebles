using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Entities.DTO;
using CrediV1_Prueba.Entities.Otros;
using CrediV1_Prueba.Interfaces;
using EllipticCurve.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace CrediV1_Prueba.Controllers;

[Authorize(Roles = "Administrador,Gerente,Vendedor")]
public class InventarioController : Controller
{

		private readonly IHttpClientFactory _clientFactory;
		private readonly IConfiguration _configuration;
		private string _connection;
		private readonly IProducto _productoModel;
        private readonly ICategoria _categoriaModel;
        private readonly IProveedoresModel _proveedoresModel;
        private readonly IInventarioModel _inventarioModel;
        private readonly ISalidasModel _salidasModel;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IHostEnvironment _host;


    public InventarioController(IHttpClientFactory clientFactory, 
        ICategoria categoriaModel, IConfiguration configuration, 
        IProducto productoModel, IProveedoresModel proveedoresModel,
        IInventarioModel inventarioModel, ISalidasModel salidsaModel,
        IHttpContextAccessor httpContextAccessor, IEmailService emailService, IHostEnvironment host)
		{
			_configuration = configuration;
			_clientFactory = clientFactory;
			_connection = _configuration.GetConnectionString("Connection");
			_productoModel = productoModel;
            _categoriaModel = categoriaModel;
           _proveedoresModel = proveedoresModel;
            _inventarioModel = inventarioModel;
            _salidasModel = salidsaModel;
         _httpContextAccessor =  httpContextAccessor;
         _emailService = emailService; 
         _host = host;

    }


		public async Task<IActionResult> Index()
    {
        var Articulos = await _productoModel.GetCantidadArticulo();
        var Proveedores = await _productoModel.GetCantidadProveedor();
        ViewData["Articulos"] = Articulos;
        ViewData["Proveedores"] = Proveedores;


        return View();
    }




    [Authorize(Roles = "Administrador,Gerente")]
    [HttpGet]
    public async Task<IActionResult> HistorialProducto()
    {
        try
        {

            var bitacoraProductos = await _inventarioModel.ConsultarBitacoraProductos();



            return View(bitacoraProductos);
        }
        catch (Exception ex)
        {
            ViewBag.exepcion = "Ocurrió un error: " + ex.Message;
            return View();
        }
    }


    [Authorize(Roles = "Administrador,Gerente")]
    [HttpGet]
    public async Task<IActionResult> HistorialProveedor()
    {
        try
        {

            var bitacoraProductos = await _inventarioModel.ConsultarBitacoraProveedores();



            return View(bitacoraProductos);
        }
        catch (Exception ex)
        {
            ViewBag.exepcion = "Ocurrió un error: " + ex.Message;
            return View();
        }
    }





    [Authorize(Roles = "Administrador,Gerente")]
    [HttpGet]
    public async Task<IActionResult> Pedidos()
    {
        try
        {

            var productos = await _productoModel.GetProductos();

            var pedidos = await _inventarioModel.ConsultarPedidos();
            var pedidosDetalles = await _inventarioModel.ConsultarPedidosDetalles();
            var pedidosDetallesEnCurso = await _inventarioModel.ConsultarPedidosDetallesEnCurso();

            ViewBag.StockRecomendaciones = await _inventarioModel.ConsultarRecomendacionestock();
            ViewBag.Vendedores = _salidasModel.ConsultarVendedores();
            ViewBag.MetodosPago = _salidasModel.ConsultarMetodosPago();
            ViewBag.Productos = productos;
            ViewBag.Pedidos = pedidos;
            ViewBag.PedidosDetalles = pedidosDetalles;
            ViewBag.pedidosDetallesEnCurso = pedidosDetallesEnCurso;
          

            return View();
        }
        catch (Exception ex)
        {
            ViewBag.exepcion = "Ocurrió un error: " + ex.Message;
            return View();
        }
    }



    [HttpPost]
    public async Task<IActionResult> EnviarNotificacion([FromBody] EnviarNotificacion notificacion)
    {
        try
        {

            Console.WriteLine("USUARIO" + notificacion.Usuario);
            Console.WriteLine("correo" + notificacion.Correo);
            Console.WriteLine("Mensaje" + notificacion.Mensaje);
            // Aquí puedes agregar la lógica para enviar el correo electrónico usando tu servicio de correo
            await _emailService.SendNotificationProveedorAsync(notificacion.Correo, notificacion.Mensaje, notificacion.Usuario);
            return Ok(new { message = "Notificación enviada correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Error al enviar la notificación: " + ex.Message });
        }
    }

    [Authorize(Roles = "Administrador,Gerente")]
    [HttpPost]
    public async Task<IActionResult> RegistrarPedido([FromBody] List<RegistrarPedidoDTO> productos)
    {
        try
        {
            if (productos == null || productos.Count == 0)
            {
                return BadRequest("No se han recibido productos.");
            }

           

            float montoTotalPedidoArray = (float)(productos.FirstOrDefault()?.montoTotalPedido);
            RegistrarPedidoDTO pedido = new RegistrarPedidoDTO
            {
                idEmpleado = (int)HttpContext.Session.GetInt32("idUsuario"),
                fechaEncargo = DateTime.Now,
                Estado = "En Curso",
                montoTotalPedido = montoTotalPedidoArray

            };

            int idPedidoRegistrado = _inventarioModel.RegistrarPedido(pedido);


            foreach (var producto in productos)
            {
                RegistrarPedidoDTO pedidoDetalle = new RegistrarPedidoDTO
                {
                    idPedido = idPedidoRegistrado,
                    idProducto = producto.idProducto,
                    idProveedor = producto.idProveedor,
                    cantidad = producto.cantidad,
                    montoUnitario = producto.montoUnitario,
                    montoTotalProducto = producto.montoTotalProducto
                };

                // Log de los datos que se están enviando
                Console.WriteLine($"idPedido: {pedidoDetalle.idPedido}, idProducto: {pedidoDetalle.idProducto}, idProveedor: {pedidoDetalle.idProveedor}, cantidad: {pedidoDetalle.cantidad}, montoUnitario: {pedidoDetalle.montoUnitario}, montoTotalProducto: {pedidoDetalle.montoTotalProducto}");

                _inventarioModel.RegistrarPedidoDetalle(pedidoDetalle);
            }

            return Ok(new { mensaje = "Pedido registrado con éxito" });
        }
        catch (Exception ex)
        {
      
            return BadRequest(new { mensaje = "Error al registrar el pedido", detalle = ex.Message });
        }
    }

    [Authorize(Roles = "Administrador,Gerente")]
    [HttpPost]
    public async Task<IActionResult> RecibirProductoPedido([FromBody] List<ConfirmarPedidoDTO> pedidos)
    {
        int idUsuario = (int?)HttpContext.Session.GetInt32("idUsuario") ?? 0;

        if (idUsuario == 0)
        {
            return BadRequest(new { mensaje = "Usuario no autenticado" });
        }

        foreach (var pedido in pedidos)
        {
            var confirmar = new RegistrarPedidoDTO
            {
                idDetalle = pedido.idDetalle,
                idPedido = pedido.idPedido,
                observaciones = pedido.observaciones,
                EmpleadoRecibido = idUsuario,
                fechaRecibido = DateTime.Now,
                Estado = pedido.estadoProducto,
                cantidadRecibida = pedido.cantidadRecibida // Asegúrate de incluir esta propiedad
            };
            string result = await _inventarioModel.ConfirmarPedido(confirmar);
            Console.WriteLine("Cantidad Recibida"+confirmar.cantidadRecibida);
        }

        int idPedido = (int)(pedidos.FirstOrDefault()?.idPedido);

        // Esperar la tarea para obtener la lista de productos
        var productos = await _inventarioModel.ConsultarPedidoDetallesPorID(idPedido);

        var correosAdmins = _inventarioModel.ConsultarCorreosAdministradores();

        // Generar la lista de productos en formato HTML
        var listaProductosHtml = new StringBuilder();
        listaProductosHtml.Append("<ul>");
        foreach (var producto in productos)
        {
            listaProductosHtml.Append($"<li>Producto: {producto.nombreProducto}, Observaciones: {producto.observaciones}, Cantidad Recibida: {producto.cantidadRecibida}</li>");
            Console.WriteLine("RECIBIDAEMAIL" + producto.cantidadRecibida);
        }
        listaProductosHtml.Append("</ul>");

        // Leer la plantilla HTML
        string ruta = Path.Combine(_host.ContentRootPath, "FormatoCorreo.html");
       
        var html = $@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
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
    <span class=""preheader"">Este es un aviso de la recepción exitosa de un pedido en nuestro sistema.</span>
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
                                            <h1>Hola,</h1>
                                            <p>Este correo es para informar que el pedido ha sido recibido exitosamente. A continuación se detallan los productos recibidos:</p>
                                            <!-- Lista de Productos -->
                                            <table class=""purchase"" width=""100%"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                                                <tr>
                                                    <td colspan=""2"">
                                                        <h3>Detalles del Pedido</h3>
                                                        <table class=""purchase_content"" width=""100%"" cellpadding=""0"" cellspacing=""0"">
                                                            <tr>
                                                                <th class=""purchase_heading"" align=""left"">
                                                                    <p class=""f-fallback"">Producto</p>
                                                                </th>
                                                                <th class=""purchase_heading"" align=""left"">
                                                                    <p class=""f-fallback"">Observaciones</p>
                                                                </th>
                                                            </tr>
                                                            @@productos
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <p><strong>Proveedor:</strong> @@Nombre</p>
                                            <p><strong>Empleado a Cargo</strong> @@Empleado</p>
                                            <p>Si tiene alguna pregunta o requiere más detalles, no dude en ponerse en contacto con nosotros.</p>
                                        
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
                                            Credimuebles, S.A.
                                      
                                            <br>San Jose, Costa Rica
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

</html>
";
        var empleado = productos.FirstOrDefault()?.nombreEmpleado;

        // Reemplazar los marcadores de posición en la plantilla HTML
        html = html.Replace("@@Nombre", productos.FirstOrDefault()?.nombreProveedor);
        html = html.Replace("@@Empleado", empleado);
        html = html.Replace("@@productos", listaProductosHtml.ToString());
        html = html.Replace("@@Contrasenna", "CONTRASEÑA_TEMPORAL");  // Cambia este valor si es necesario
        html = html.Replace("@@Vencimiento", DateTime.Now.AddHours(24).ToString("dd/MM/yyyy HH:mm"));

        // Enviar el correo a los administradores
        var pdfContent = _emailService.GenerarPDFPedido(productos, productos.FirstOrDefault()?.nombreProveedor);

        foreach (var correo in await correosAdmins)
        {
            _emailService.SendNotificationAdministradoresAsync(correo.correo, html, productos.FirstOrDefault()?.nombreProveedor, pdfContent);
        }

        return Ok(new { mensaje = "Pedidos actualizados correctamente" });
    }





    [Authorize(Roles = "Administrador,Gerente")]
    [HttpGet]
    public async Task<IActionResult> EditarPedido(long id)
    {
        try
        {
            // Obtener detalles del pedido
            var pedidoDetalles = await _inventarioModel.ConsultarPedidoDetallesPorID(id);

            if (pedidoDetalles != null)
            {
                // Obtener otros datos necesarios para la vista
                var productos = await _productoModel.GetProductos();
                ViewBag.StockRecomendaciones = await _inventarioModel.ConsultarRecomendacionestock();
                ViewBag.Vendedores = _salidasModel.ConsultarVendedores();
                ViewBag.Productos = productos;
                ViewBag.pedidoDetalles = pedidoDetalles;

                // Extraer proveedores únicos de los detalles del pedido
                var proveedoresRelacionados = pedidoDetalles
                    .Select(d => new { d.idProveedor, d.nombreProveedor })
                    .Distinct()
                    .ToList();

               
                string empleadoEncargo = (string)(pedidoDetalles.FirstOrDefault()?.nombreEmpleado);
                float montoTotalPedidoArray = (float)(pedidoDetalles.FirstOrDefault()?.montoTotalPedido);
                int idEmpleado = (int)(pedidoDetalles.FirstOrDefault()?.idEmpleado);
                int idPedido = (int)(pedidoDetalles.FirstOrDefault()?.idPedido);
                ViewBag.CostoTotalPedido = montoTotalPedidoArray;
                ViewBag.Proveedores = proveedoresRelacionados;
                ViewBag.NombreEmpleadoPedido = empleadoEncargo;
                ViewBag.idEmpleado = idEmpleado;
                ViewBag.idPedido = idPedido;

                // Pasar la lista de detalles del pedido a la vista
                return View();
            }
            else
            {
                // Manejar el caso en que el pedido no se encuentra
                return View();
            }
        }
        catch (Exception ex)
        {
            ViewBag.exepcion = "Ocurrió un error: " + ex.Message;
            return View();
        }
    }





    [Authorize(Roles = "Administrador,Gerente")]
    [HttpPost]
    public async Task<IActionResult> ActualizarPedido([FromBody] List<RegistrarPedidoDTO> productos)
    {
        try
        {
            if (productos == null || productos.Count == 0)
            {
                Console.WriteLine("No se han recibido productos.");
                return BadRequest("No se han recibido productos.");
            }
            int detalle = (int)(productos.FirstOrDefault()?.idDetalle);
            
            


    
            Console.WriteLine("Contenido de productos recibido:");
            foreach (var producto in productos)
            {

                Console.WriteLine($"ID Detalle: {producto.idDetalle}, ID PEDIDO: {producto.idPedido}, FECHA RECIB: {producto.fechaRecibido}," +
                  $" monto total: {producto.montoTotalPedido}, EMPLEADO: {producto.EmpleadoRecibido}, Cantidad: {producto.cantidad}, Monto Unitario:" +
                  $" {producto.montoUnitario}, Monto Total Producto: {producto.montoTotalProducto}");


                if (producto.idDetalle != 0)
                {
                    _inventarioModel.ActualizarPedido(producto);
                }
                else
                {
                    _inventarioModel.RegistrarPedidoDetalle(producto);
                }



            }

      


            // Productos para actualizar
     

            return Ok(new { mensaje = "Pedido actualizado con éxito" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar el pedido: {ex.Message}");
            return BadRequest(new { mensaje = "Error al actualizar el pedido", detalle = ex.Message });
        }
    }









    [Authorize(Roles = "Administrador,Gerente")]
    public async Task<IActionResult> AgregarProducto()
    {
        try
        {
            var categorias = await _categoriaModel.GetCategorias();
            var proveedores = await _proveedoresModel.GetProveedores(); 

            ViewData["categorias"] = categorias;
            ViewData["proveedores"] = proveedores;
            

            return View();
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Error al obtener datos para agregar producto: " + ex.Message;
            return View(); 
        }
    }




    [HttpGet]
    public async Task<IActionResult> ListadoProducto()
    {
        try
        {
            //var categorias = await _categoriaModel.GetCategorias(); para mas adelante para filtrar por categorias
            var productos = await _productoModel.GetProductos();
            var productosStockBajo = await _inventarioModel.ConsultarProductosBajosStock();
            var recomendacionesStock = await _inventarioModel.ConsultarRecomendacionestock();
            ViewBag.ProductosStock = productosStockBajo ?? new List<ProductosBajoStock>();
       



            return View(productos);
         
        }
        catch (Exception ex)
        {
            return View();

        }
    }

            public async Task<IActionResult> EditarProducto(int idProducto) { 
            var producto = await _productoModel.buscarProducto(idProducto);
            if (producto.Codigo == 1)
            {
                var categorias = await _categoriaModel.GetCategorias();
                var proveedores = await _proveedoresModel.GetProveedores(); 

                ViewData["categorias"] = categorias;
                ViewData["proveedores"] = proveedores;

                ProductoEnt resp = new ProductoEnt();
                resp = (ProductoEnt)producto.Contenido;
                return View(resp);

            }
            else {
                ViewBag["error"] = producto.Codigo;
            }
            return View();
            
             
        }




        public async Task<IActionResult> GuardarProductoNuevo([FromBody] ProductoEnt producto)
        {
            try
            {

        

            Console.WriteLine("Datos del producto" + " " + producto.cantidadStock, producto.idCategoria);


                var mensaje = await _productoModel.agregarProducto(producto);
                if (mensaje == true)
                {
                    return RedirectToAction("Index", "Inventario"); ;
                }
                else
                {
                    return NotFound(mensaje);
                }
            }
            catch (Exception ex)
            {
                // Registra el error para fines de depuración
                Console.WriteLine($"Error alagregar el producto: {ex.Message}");
                return StatusCode(500, "Error interno del servidor.");
            }


        }
         [HttpPost]     
         public async Task<IActionResult> ActualizarProducto([FromBody] ProductoEnt producto)
            {
                try
                {
                    int idUsuario = (int?)HttpContext.Session.GetInt32("idUsuario") ?? 0;
                    BitacoraProducto bit = new BitacoraProducto
                    {
                        idProducto = producto.idProducto,
                        idUsuario = idUsuario,
                        accion = "El usuario Editó el Producto",
                        fecha = DateTime.Now

                    };
            _inventarioModel.RegistrarBitacoraProducto(bit);
                    var resp = await _productoModel.actualizarProducto(producto);
                    if (resp == true)
                    {
                        return RedirectToAction("ListadoProduct", "Inventario"); ;
                    }
                    else
                    {
                        // Aquí también debes retornar el resultado de RedirectToAction
                        return NotFound(resp); 
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores aquí
                    ViewBag.ErrorMessage = "Error al obtener datos para agregar producto: " + ex.Message;
                    // Aquí también debes asegurarte de retornar algo, en este caso, una redirección
                    return RedirectToAction("ListadoProduct", "Inventario");
                }
            }




    [HttpPost]
    public async Task<IActionResult> ActualizarStock([FromBody] RestablecerCantidadDTO stock)
    {
        try
        {
            Console.WriteLine("IDD", stock.idProducto);
            Console.WriteLine("CANTIDADD", stock.cantidadStock);
            var resp = await _inventarioModel.RestablecerStockProducto(stock);
            if (resp == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Error al actualizar el stock" });
            }
        }
        catch (Exception ex)
        {
            // Retorna un JSON con el mensaje de error
            return Json(new { success = false, message = "Error al actualizar el stock: " + ex.Message });
        }
    }

    [Authorize(Roles = "Administrador,Gerente")]
    [HttpPost]
    public IActionResult CancelarProducto([FromBody] RegistrarPedidoDTO ent)
    {
        try
        {

            Console.WriteLine($"{ent.idDetalle}");
            _inventarioModel.CancelarPedido(ent);
     


            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
        
            return BadRequest(new { success = false, message = ex.Message });
        }
    }


    [Authorize(Roles = "Administrador,Gerente")]
    [HttpPost]
    public async Task<IActionResult> DesactivarProducto([FromBody] ProductoEnt producto)
    {
        try
        {

            int idUsuario = (int?)HttpContext.Session.GetInt32("idUsuario") ?? 0;
            BitacoraProducto bit = new BitacoraProducto
            {
                idProducto = producto.idProducto,
                idUsuario = idUsuario,
                accion = "El usuario Desactivo el Producto",
                fecha = DateTime.Now

            };

             _inventarioModel.RegistrarBitacoraProducto(bit);

            var mensaje = await _productoModel.DesactivarProducto(producto);
            if (mensaje == "Producto desactivado exitosamente" || mensaje == "Producto activado exitosamente")
            {
                return Ok(mensaje);
            }
            else
            {
                return NotFound(mensaje);
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error al cambiar el estado del producto: {ex.Message}");
            return StatusCode(500, "Error interno del servidor.");
        }
    }

}





  
  
