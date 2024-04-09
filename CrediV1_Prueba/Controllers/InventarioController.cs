using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers
{
    public class InventarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AgregarProducto()
        {
            return View();
        }

		public IActionResult ListadoProducto()
		{
			return View();
		}

        public IActionResult EditarProducto() { 
        
        
            return View();
        }   



	}
}
