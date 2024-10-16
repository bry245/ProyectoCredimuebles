using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrediV1_Prueba.Controllers
{
    [Authorize(Roles = "Administrador,Gerente,Vendedor")]
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
        [Authorize(Roles = "Administrador,Gerente,Vendedor")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Administrador,Gerente,Vendedor")]
        [HttpGet]
        public async Task<IActionResult> ListadoSalidas()
        {
            try
            {
                int pageSize = 4; // Número de elementos por página
                var salidas = await _salidasModel.ListarSalidas();

                return View(salidas);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeSalidas = "Error al listar las salidas";
                return View();
            }
        }

      


        [Authorize(Roles = "Administrador,Gerente")]
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

        [Authorize(Roles = "Administrador,Gerente")]
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

                            return Ok(new { success = true, message = "Registro de salida exitoso" });
                       
                       
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

        [Authorize(Roles = "Administrador,Gerente")]
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

        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        public IActionResult EditarSalida([FromBody] SalidasEnt datos)
        {

            if (datos == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //Devuelve la cantidad al stock de inventario

                List<ProductoEnt> productosActaulizarStock = _salidasModel.ObtenerProductoDeSalida(datos.idSalida);

                foreach (var item in productosActaulizarStock)
                {
                    ProductoEnt productoStock = new ProductoEnt
                    {

                        idProducto = item.idProducto,
                        cantidadSalida = item.cantidadSalida

                    };
                    _salidasModel.ActualizarInventarioEditarSalida(productoStock);
                }

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


        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        public async Task<IActionResult> AnularSalida([FromBody] SalidasEnt datos)
        {
            try
            {
                //Suma el stock de los productos en el inventario 
                List<ProductoEnt> productosActaulizarStock = _salidasModel.ObtenerProductoDeSalida(datos.idSalida);
                foreach (var item in productosActaulizarStock)
                {
                    ProductoEnt productoStock = new ProductoEnt
                    {

                        idProducto = item.idProducto,
                        cantidadSalida = item.cantidadSalida

                    };
                    _salidasModel.ActualizarInventarioEditarSalida(productoStock);
                }
                //seguir con la bd eliminando la salida
                var respuesta = _salidasModel.AnularSalida(datos.idSalida);

                if (respuesta != -1)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "Error interno del servidor.");
                }





            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }



        //CUENTRAS POR COBRAR
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpGet]
        public IActionResult CrearCuentaCredito()
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




        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        public IActionResult RegistrarCuentaCredito([FromBody] SalidasEnt datos)
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
                    var registroCuenta = _salidasModel.RegistrarCuentaCredito(datos);
                    if (registroCuenta != -1)
                    {
                        foreach (var item in datos.productosCompra)
                        {
                            ProductoEnt productoSalida = new ProductoEnt
                            {
                                idSalida = registroCuenta,
                                idProducto = item.idProducto,
                                cantidadSalida = item.cantidadSalida,
                                costo = item.costo
                            };
                            _salidasModel.RegistrarProductosSalida(productoSalida);
                        }

                        return Ok(new { success = true, message = "Registro de cuenta exitoso" });
                    
                    }
                    else
                    {
                        return StatusCode(500, new { success = false, message = "Ocurrió un error inesperado." });

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


        [Authorize(Roles = "Administrador,Gerente,Vendedor")]
        [HttpGet]
        public async Task<IActionResult> ListadoBono(int page = 1)
        {
            try
            {
                int pageSize = 8; // Número de elementos por página
                var creditos = await _salidasModel.ListarCuentasPorCobrar(page, pageSize);

                return View(creditos);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeCuentas = "Error al listar las cuentas";
                return View();
            }
        }




        [Authorize(Roles = "Administrador,Gerente")]
        [HttpGet]
        public IActionResult CrearAbono(long idCuenta)
        {
            try
            {
                var salida = _salidasModel.VerCuentaPorCobrar(idCuenta);
                if (salida != null)
                {
                    ViewBag.Vendedores = _salidasModel.ConsultarVendedores();
                    ViewBag.MetodosPago = _salidasModel.ConsultarMetodosPago();
                    List<SalidasEnt> productos = _salidasModel.ObtenerPagosRealizados(idCuenta);
                    foreach (var producto in productos)
                    {
                        producto.fecha2 = producto.fechaAbono.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (productos.Count() <= 0)
                    {

                        ViewBag.cuentas = productos;
                    }
                    ViewBag.cuentas = productos;

                    ViewBag.Fecha = salida.fecha;
                    ViewBag.MetodoPago = salida.idMetodoPago;
                    ViewBag.Vendedor = salida.idVendedor;
                    //cargar los productos
                    //List<ProductoEnt> productoSalida = _salidasModel.ObtenerProductoDeSalida(id);
                    // ViewBag.ProductosSalida = productoSalida;

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



            return View();
        }

        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        public IActionResult CrearAbonos([FromBody] SalidasEnt datos)
        {

            if (datos == null || datos.abono<=0)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //Devuelve la cantidad al stock de inventario


                var registroAbono = _salidasModel.AgregarAbono(datos);
                if (registroAbono != -1)
                {

                    return Ok(new { success = true, message = "Pago exitoso" });
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

    }
}