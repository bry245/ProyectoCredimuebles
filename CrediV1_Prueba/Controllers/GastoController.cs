using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using CrediV1_Prueba.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CrediV1_Prueba.Controllers
{
    [Authorize(Roles = "Gerente")]
    public class GastoController : Controller
    {
        private readonly IGastosModel _gastosModel;
        private readonly IControlCajaModel _controlCajaModel;

        public GastoController(IGastosModel gastosModel, IControlCajaModel controlCajaModel)
        {
            _gastosModel = gastosModel;
            _controlCajaModel = controlCajaModel;
        }

        // Acción para mostrar la vista de agregar gasto
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Acción para guardar un nuevo gasto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gasto gasto)
        {
            if (ModelState.IsValid)
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
                            // Actualizar el registro existente
                            controlCaja.totalCaja -= gasto.monto;
                            await _controlCajaModel.ActualizarControlCaja(controlCaja);
                        }
                        else
                        {
                            // Crear un nuevo registro en ControlCaja
                            var nuevoControl = new ControlCaja
                            {
                                fechaID = gasto.fecha,
                                totalCaja = -gasto.monto
                            };
                            await _controlCajaModel.CrearControlCaja(nuevoControl);
                        }

                        return RedirectToAction("IndexCaja", "ControlCaja");
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
            return View(gasto);
        }
    }
}
