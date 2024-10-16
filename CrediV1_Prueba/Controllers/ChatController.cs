using CrediV1_Prueba.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers
{
    public class ChatController(IChatModel ChatModel, IReporteService iReporteService, IProveedoresModel iProveedorModel) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Chat()
        {
            return View();

        }


		

		[HttpGet]
		public async Task<IActionResult> TraerDatosDB()
		{
			try
			{
				var datos = await ChatModel.TraerDatosDB();

				return Json(datos);
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
        public async Task<IActionResult> ObtenerVentasTotales()
        {
            try
            {
                var datos = await iReporteService.ObtenerRegistroFinancieroTotal();

                return Json(datos);
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
        public async Task<IActionResult> ListadoProveedor()
        {
            try
            {
                var proveedores = await iProveedorModel.GetProveedores();

                return Json(proveedores);
            }
            catch (Exception ex)
            {
            }
            return View();
        }






    }
}
