using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers
{
    public class InventarioController : Controller
    {

		private readonly IHttpClientFactory _clientFactory;
		private readonly IConfiguration _configuration;
		private string _connection;
		private readonly IProducto _productoModel;
        private readonly ICategoria _categoriaModel;
        private readonly IProveedoresModel _proveedoresModel;


		public InventarioController(IHttpClientFactory clientFactory, 
            ICategoria categoriaModel, IConfiguration configuration, 
            IProducto productoModel, IProveedoresModel proveedoresModel)
		{
			_configuration = configuration;
			_clientFactory = clientFactory;
			_connection = _configuration.GetConnectionString("Connection");
			_productoModel = productoModel;
            _categoriaModel = categoriaModel;
           _proveedoresModel = proveedoresModel;
		}

		public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> AgregarProducto()
        {
            try
            {
                var categorias = await _categoriaModel.GetCategorias();
                var proveedores = await _proveedoresModel.GetProveedores(); // Asumiendo que GetProveedores es un método que obtiene los proveedores

                ViewData["categorias"] = categorias;
                ViewData["proveedores"] = proveedores;
                

                return View();
            }
            catch (Exception ex)
            {
                // Manejo de errores aquí
                ViewBag.ErrorMessage = "Error al obtener datos para agregar producto: " + ex.Message;
                return View(); // Retornar la vista con el mensaje de error
            }
        }



        public async Task<IActionResult> ListadoProducto()
		{
			try
			{
                //var categorias = await _categoriaModel.GetCategorias(); para mas adelante para filtrar por categorias
				var productos = await _productoModel.GetProductos();


               

				return View(productos);
			}
			catch (Exception ex)
			{
			}
			return View();
		}


		public IActionResult EditarProducto() { 
        
        
            return View();
        }


        public async Task <IActionResult> GuardarProductoNuevo([FromBody] ProductoEnt producto)
        {


            try
            {

            }catch (Exception ex)
            {


            }


            return View();
        }


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







}
