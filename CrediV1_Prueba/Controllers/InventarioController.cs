using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Entities.DTO;
using CrediV1_Prueba.Entities.Otros;
using CrediV1_Prueba.Interfaces;
using EllipticCurve.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


    public InventarioController(IHttpClientFactory clientFactory, 
        ICategoria categoriaModel, IConfiguration configuration, 
        IProducto productoModel, IProveedoresModel proveedoresModel,
        IInventarioModel inventarioModel, ISalidasModel salidsaModel,
        IHttpContextAccessor httpContextAccessor, IEmailService emailService)
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
    }


		public async Task<IActionResult> Index()
    {
        var Articulos = await _productoModel.GetCantidadArticulo();
        var Proveedores = await _productoModel.GetCantidadProveedor();
        ViewData["Articulos"] = Articulos;
        ViewData["Proveedores"] = Proveedores;


        return View();
    }



    [HttpGet]
    public async Task<IActionResult> Pedidos()
    {
        try
        {

            var productos = await _productoModel.GetProductos();

            var pedidos = await _inventarioModel.ConsultarPedidos();
            var pedidosDetalles = await _inventarioModel.ConsultarPedidosDetalles();

            ViewBag.StockRecomendaciones = _inventarioModel.ConsultarRecomendacionestock();
            ViewBag.Vendedores = _salidasModel.ConsultarVendedores();
            ViewBag.MetodosPago = _salidasModel.ConsultarMetodosPago();
            ViewBag.Productos = productos;
            ViewBag.Pedidos = pedidos;
            ViewBag.PedidosDetalles = pedidosDetalles;

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
            Console.WriteLine("ID pedidooo: " + idPedidoRegistrado);


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

                _inventarioModel.RegistrarPedidoDetalle(pedidoDetalle);
            }

            return Ok(new { mensaje = "Pedido registrado con éxito" });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return BadRequest(new { mensaje = "Error al registrar el pedido", detalle = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RecibirProductoPedido([FromBody] ConfirmarPedidoDTO pedido)
    {
        Console.WriteLine("IDDETALLEEEE"+pedido.idDetalle);

        int idUsuario = (int)HttpContext.Session.GetInt32("idUsuario");

        if (idUsuario == null)
        {
    
            return BadRequest(new { mensaje = "Usuario no autenticado" });
        }
        Console.WriteLine("ASDASDAS"+pedido.idDetalle);


        RegistrarPedidoDTO confirmar = new RegistrarPedidoDTO
        {
            idDetalle = pedido.idDetalle,
            fechaRecibido = DateTime.UtcNow,
            EmpleadoRecibido = idUsuario  
        };

           string result = await _inventarioModel.ConfirmarPedido(confirmar);

        return Ok(new { mensaje = "Pedido registrado con éxito" });
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

         public async Task<IActionResult> ActualizarProducto([FromBody] ProductoEnt producto)
            {
                try
                {
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
    public async Task<IActionResult> DesactivarProducto([FromBody] ProductoEnt producto)
    {
        try
        {
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
            // Registra el error para fines de depuración
            Console.WriteLine($"Error al cambiar el estado del producto: {ex.Message}");
            return StatusCode(500, "Error interno del servidor.");
        }
    }

}





  
  
