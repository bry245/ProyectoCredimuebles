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


        [HttpGet]
        public async Task <IActionResult> ListadoProveedor()
        {

            try
            {
                var proveedores = await _proveedores.GetProveedores();

                return View(proveedores);

            }
            catch (Exception ex)
            {

            }
            return View();
        }

		public IActionResult AgregarProveedor()
		{
            
           

            return View();

			
		}

		[HttpPost]
		public async Task<IActionResult> GuardarProveedor([FromBody] Proveedor proveedor)
		{

			try
			{
				await _proveedores.AddProveedores(proveedor);
				return Ok();

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al guardar el proveedor: {ex.Message}");
				return StatusCode(500, "Error interno del servidor.");
			}


		}



		[HttpGet]
		public async Task<IActionResult> EditarProveedor(int Proveedor)
		{
			// Asegúrate de que el parámetro 'id' coincide con el nombre del parámetro en tu vista
			var proveedorEditar = await _proveedores.GetProveedoresID(Proveedor);
			
			return View(proveedorEditar); // Pasa el proveedor a la vista
		}

        [HttpPost]
        public async Task<IActionResult> GuardarEditarProveedor([FromBody] Proveedor proveedor)
        {
            if (proveedor == null || !ModelState.IsValid)
            {
                return BadRequest("Datos inválidos.");
            }

            try
            {
                await _proveedores.UpdateProveedor(proveedor);
                return Ok();
            }
            catch (Exception ex)
            {
                // Registra el error para fines de depuración
                Console.WriteLine($"Error al guardar el proveedor: {ex.Message}");
                return StatusCode(500, "Error interno del servidor.");
            }
        }






    }
}
