using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers
{
    public class InventarioController : Controller
    {
        public IActionResult Index()
        {
            //codigo de inventario
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



        public IActionResult ListadoProveedor()
        {
            return View();
        }

		public IActionResult AgregarProveedor()
		{
			return View();
		}


		public IActionResult EditarProveedor()
		{


			return View();
		}





	}
}
