using CrediV1_Prueba.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace CrediV1_Prueba.Controllers
{
    public class ReporteController(IReporteService _reportService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.VentasMensuales = _reportService.VentasMensuales();

            ViewBag.VentasPorMetodoPagoCantidad = _reportService.VentasPorMetodoPagoCantidad();
            ViewBag.VentasPorMetodoPago = _reportService.VentasPorMetodoPago();



            return View();
        }

        public async Task<IActionResult> VentasMensuales()
        {
            try
            {
                var ventasMensuales = await _reportService.VentasMensuales();
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

    }
}
