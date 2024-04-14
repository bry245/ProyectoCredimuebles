using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers
{
    public class CajaController : Controller
    {
        [HttpGet]
        public IActionResult IndexCaja()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Gastos()
        {
            return View();
        }
    }
}
