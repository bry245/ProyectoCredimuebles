using CrediV1_Prueba.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers
{
    public class ChatController(IChatModel ChatModel) : Controller
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


	}
}
