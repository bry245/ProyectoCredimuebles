using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers
{
    public class ChatController : Controller
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



	}
}
