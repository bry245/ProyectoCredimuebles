using CrediV1_Prueba.Models;
using CrediV1_Prueba.Services.Proveedores;
using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers
{
    public class InventarioController : Controller
    {
        private readonly IProveedores _proveedores;

        public InventarioController(IProveedores proveedorService)
        {
            _proveedores = proveedorService;
        }
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
