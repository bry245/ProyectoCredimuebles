using CrediV1_Prueba.Interfaces;
using CrediV1_Prueba.Models;
using CrediV1_Prueba.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CrediV1_Prueba.Controllers
{
    [Authorize(Roles = "Administrador,Gerente")]
    public class ControlCajaController : Controller
    {
        private readonly IControlCajaModel _controlCajaModel;
        private readonly IGastosModel _gastosModel;

        public ControlCajaController(IControlCajaModel controlCajaModel, IGastosModel gastosModel)
        {
            _controlCajaModel = controlCajaModel;
            _gastosModel = gastosModel;
        }

        // Acción para mostrar la vista principal del control de caja
        public async Task<IActionResult> Index()
        {
            try
            {
                var controlCajaData = await _controlCajaModel.ObtenerControlCaja();
                return View(controlCajaData);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener datos del control de caja: " + ex.Message;
                return View(new List<ControlCaja>());
            }
        }

        // Acción para mostrar la vista de agregar gasto
        [Authorize(Roles = "Administrador,Gerente")]
        public IActionResult AgregarGasto()
        {
            return View();
        }

        // Acción para guardar un nuevo gasto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarGasto([FromBody] Gasto gasto)
        {
            try
            {
                var result = await _gastosModel.AgregarGasto(gasto);

                if (result)
                {
                    // Actualizar el control de caja
                    var controlCaja = await _controlCajaModel.BuscarControlCajaPorFecha(gasto.fecha);
                    if (controlCaja != null)
                    {
                        controlCaja.idGasto = gasto.idGasto;
                        controlCaja.totalCaja -= gasto.monto;
                        await _controlCajaModel.ActualizarControlCaja(controlCaja);
                    }
                    else
                    {
                        // Si no existe un registro de control de caja para esa fecha, lo creamos
                        ControlCaja nuevoControl = new ControlCaja
                        {
                            fechaID = gasto.fecha,
                            idGasto = gasto.idGasto,
                            totalCaja = -gasto.monto
                        };
                        await _controlCajaModel.CrearControlCaja(nuevoControl);
                    }

                    return RedirectToAction("Index", "ControlCaja");
                }
                else
                {
                    return NotFound("No se pudo registrar el gasto.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar el gasto: {ex.Message}");
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}

