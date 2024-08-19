using CrediV1_Prueba.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace CrediV1_Prueba.Controllers
{
    public class ReporteController(IReporteService _reportService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.VentasMensuales = await _reportService.VentasMensuales();
            ViewBag.VentasDelDia = await _reportService.VentasDia();
            ViewBag.AbonoSemanal = await _reportService.ObtenerAbonosSemanales();// Asegúrate de usar await aquí
            ViewBag.VentasPorMetodoPagoCantidad = await _reportService.VentasPorMetodoPagoCantidad();
            ViewBag.VentasPorMetodoPago = await _reportService.VentasPorMetodoPago();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> VentasMensuales()
        {
            try
            {
                var ventasMensuales = await _reportService.VentasMensuales();
                foreach( var ventas in ventasMensuales)
                {
                    Console.WriteLine(ventas.TotalVentas);
                }
                return Json(ventasMensuales);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework)
                Console.WriteLine($"Error: {ex.Message}");
                // Return a proper error response
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpGet]
        public async Task<IActionResult> VentasPorPagos()
        {
            try
            {
                var ventasPago = await _reportService.VentasPorMetodoPagoCantidad();
              
                return Json(ventasPago);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework)
                Console.WriteLine($"Error: {ex.Message}");
                // Return a proper error response
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


    }
}
