using CrediV1_Prueba.Interfaces;
using CrediV1_Prueba.Models;
using CrediV1_Prueba.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers

{

    [ResponseCache(NoStore = true, Duration = 0)]
    public class ProveedorController : Controller
	{

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private string _connection;
        private readonly IProveedoresModel _proveedorModel;

        public ProveedorController(IHttpClientFactory clientFactory, IConfiguration configuration, IProveedoresModel proveedorModel)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connection = _configuration.GetConnectionString("Connection");
            _proveedorModel = proveedorModel;
        }


        [HttpGet]
		public async Task<IActionResult> ListadoProveedor()
		{
			try
			{
				var proveedores = await _proveedorModel.GetProveedores();

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
		public async Task<IActionResult> GuardarProveedor([FromBody] ProveedorEnt proveedor)
		{
			try
			{
				await _proveedorModel.AddProveedor(proveedor);
				return RedirectToAction("ListadoProveedor", "Proveedor");
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
			
			var proveedorEditar = await _proveedorModel.GetProveedoresID(Proveedor);

			return View(proveedorEditar); // Pasa el proveedor a la vista
		}

		[HttpPost]
		public async Task<IActionResult> GuardarEditarProveedor([FromBody] ProveedorEnt proveedor)
		{
			if (proveedor == null || !ModelState.IsValid)
			{
				return BadRequest("Datos inválidos.");
			}
		
			try
			{
				await _proveedorModel.UpdateProveedor(proveedor);
				return Ok();
			}
			catch (Exception ex)
			{
				// Registra el error para fines de depuración
				Console.WriteLine($"Error al guardar el proveedor: {ex.Message}");
				return StatusCode(500, "Error interno del servidor.");
			}
		}


		[HttpPost]
		public async Task<IActionResult> DesactivarProveedor([FromBody] ProveedorEnt idProveedor)
		{
			try
			{
				await _proveedorModel.DesactivarProveedor(idProveedor);
				return Ok();
			}
			catch (Exception ex)
			{
				// Registra el error para fines de depuración
				Console.WriteLine($"Error al desactivar el proveedor: {ex.Message}");
				return StatusCode(500, "Error interno del servidor.");
			}
		}

	}
}
