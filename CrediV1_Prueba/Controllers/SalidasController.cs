using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrediV1_Prueba.Controllers
{
    public class SalidasController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private string _connection;
        private readonly ISalidasModel _salidasModel;

        public SalidasController(IHttpClientFactory clientFactory, IConfiguration configuration, ISalidasModel salidasModel)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connection = _configuration.GetConnectionString("Connection");
            _salidasModel = salidasModel;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ListadoSalidas(int page = 1)
        {
            try
            {
                int pageSize = 4; // Número de elementos por página
                var salidas = await _salidasModel.ListarSalidas(page, pageSize);

                return View(salidas);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeSalidas = "Error al listar las salidas";
                return View();
            }
        }
        [HttpGet]
        public IActionResult CrearVenta()
        {
            try
            {
                ViewBag.Vendedores = _salidasModel.ConsultarVendedores();
                ViewBag.MetodosPago = _salidasModel.ConsultarMetodosPago();
                List<ProductoEnt> productos = _salidasModel.ObtenerProductos();
                ViewBag.Productos = productos;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.exepcion = "Ocurrió un error: " + ex.Message;
                return View();
            }

        }

        [HttpPost]
        public IActionResult RegistrarSalida([FromBody] SalidasEnt datos)
        {
            if (datos == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                datos.idMetodoPago = long.Parse(datos.metodoPago);
                datos.idVendedor = long.Parse(datos.vendedor);
                if (datos != null)
                {
                    var registroSalida = _salidasModel.RegistrarSalida(datos);
                    if (registroSalida != -1)
                    {
                        foreach (var item in datos.productosCompra)
                        {
                            ProductoEnt productoSalida = new ProductoEnt
                            {
                                idSalida = registroSalida,
                                idProducto = item.idProducto,
                                cantidadSalida = item.cantidadSalida,
                                costo = item.costo
                            };
                            _salidasModel.RegistrarProductosSalida(productoSalida);
                        }
                    }
                    return Ok(new { success = true, message = "Registro de salida exitoso" });
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Ocurrió un error inesperado." });
            }
        }


        [HttpGet]
        public IActionResult EditarVenta(long id)
        {
            try
            {
                var salida = _salidasModel.VerSalida(id);
                if (salida != null)
                {
                    ViewBag.Vendedores = _salidasModel.ConsultarVendedores();
                    ViewBag.MetodosPago = _salidasModel.ConsultarMetodosPago();
                    List<ProductoEnt> productos = _salidasModel.ObtenerProductos();
                    ViewBag.Productos = productos;

                    ViewBag.Fecha = salida.fecha;
                    ViewBag.MetodoPago = salida.idMetodoPago;
                    ViewBag.Vendedor = salida.idVendedor;
                    //cargar los productos
                    List<ProductoEnt> productoSalida = _salidasModel.ObtenerProductoDeSalida(id);
                    ViewBag.ProductosSalida = productoSalida;

                    return View(salida);
                }
                else
                {

                    return View(null);
                }
            }
            catch (Exception ex)
            {
                ViewBag.exepcion = "Ocurrió un error: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult EditarSalida([FromBody] SalidasEnt datos)
        {

            if (datos == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                datos.idMetodoPago = long.Parse(datos.metodoPago);
                datos.idVendedor = long.Parse(datos.vendedor);
                if (datos != null)
                {
                    var registroSalida = _salidasModel.ActualizarSalida(datos);
                    if (registroSalida != -1)
                    {
                        foreach (var item in datos.productosCompra)
                        {
                            ProductoEnt productoSalida = new ProductoEnt
                            {
                                idSalida = registroSalida,
                                idProducto = item.idProducto,
                                cantidadSalida = item.cantidadSalida,
                                costo = item.costo
                            };
                            _salidasModel.ActualizarProductosSalida(productoSalida);
                        }
                        return Ok(new { success = true, message = "Actualización de salida exitoso" });

                    }
                    else
                    {
                        return BadRequest(ModelState);

                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Ocurrió un error inesperado." });
            }
        }


        public IActionResult ListadoBono()
        {
            return View();
        }

        public IActionResult CrearBono()
        {
            return View();
        }

    }
}
