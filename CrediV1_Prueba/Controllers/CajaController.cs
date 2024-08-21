using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using CrediV1_Prueba.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrediV1_Prueba.Controllers
{
    [Authorize(Roles = "Administrador,Gerente,Vendedor")]
    public class CajaController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private string _connection;
        private readonly ISalidasModel _salidasModel;
        private readonly ICajaModel _cajaModel;

        public CajaController(IHttpClientFactory clientFactory, IConfiguration configuration, ISalidasModel salidasModel, ICajaModel cajaModel)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connection = _configuration.GetConnectionString("Connection");
            _salidasModel = salidasModel;
            _cajaModel = cajaModel;

        }

        [Authorize(Roles = "Administrador,Gerente,Vendedor")]
        [HttpGet]
        public async Task<IActionResult> IndexCaja(int page = 1)
        {

            try
            {
                int pageSize = 31; // Número de elementos por página
                var salidas = await _cajaModel.ListarCajasDiarias(page, pageSize);
                CajaEnt datosCajas = _cajaModel.ObtenerDatosCaja();

                ViewBag.datosCajas = datosCajas;


                return View(salidas);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeCajas = "Error al listar las Cajas diarias";
                return View();
            }
        }
        [Authorize(Roles = "Administrador,Gerente,Vendedor")]
        [HttpGet]
        public async Task<IActionResult> Gastos(DateTime fecha)
        {
            try
            {
                int page = 1;
                int pageSize = 31; // Número de elementos por página
                var gastos = await _cajaModel.ListarGastos(page, pageSize, fecha);
                ViewBag.fecha = fecha.ToString("yyyy-MM-dd");

               


                return View(gastos);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeCajas = "Error al listar los  gastos de esta fecha";
                return View();
            }
        }

        [Authorize(Roles = "Administrador,Gerente,Vendedor")]
        [HttpPost]
        public IActionResult RegistrarGasto([FromBody] CajaEnt datos)
        {
            if (datos == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (datos != null)
                {
                    var registroGasto = _cajaModel.RegistrarGasto(datos);
                    if (registroGasto != -1)
                    {

                        return Ok(new { success = true, message = "Registro exitoso" });
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

        [Authorize(Roles = "Administrador,Gerente,Vendedor")]
        [HttpPost]

        public IActionResult EliminarGasto([FromBody] CajaEnt datos)
        {
            if (datos == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (datos != null)
                {
                    var registroGasto = _cajaModel.EliminarGasto(datos);
                    if (registroGasto != -1)
                    {

                        return Ok(new { success = true, message = "Anulación exitosa" });
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
    }
}


