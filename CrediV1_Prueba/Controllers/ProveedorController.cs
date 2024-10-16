using CrediV1_Prueba.Interfaces;
using CrediV1_Prueba.Models;
using CrediV1_Prueba.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CrediV1_Prueba.Controllers

{
    [Authorize(Roles = "Administrador,Gerente,Vendedor")]
    [ResponseCache(NoStore = true, Duration = 0)]
    public class ProveedorController : Controller
	{

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private string _connection;
        private readonly IProveedoresModel _proveedorModel;
        private readonly IInventarioModel _inventarioModel;

        public ProveedorController(IHttpClientFactory clientFactory, IConfiguration configuration, IProveedoresModel proveedorModel, IInventarioModel inventarioModel)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connection = _configuration.GetConnectionString("Connection");
            _proveedorModel = proveedorModel;
            _inventarioModel = inventarioModel;
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
        [Authorize(Roles = "Administrador,Gerente,Vendedor")]
        public IActionResult AgregarProveedor()
		{

			return View();
		}
        [Authorize(Roles = "Administrador,Gerente,Vendedor")]
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
        [Authorize(Roles = "Administrador,Gerente,Vendedor")]
        [HttpGet]
		public async Task<IActionResult> EditarProveedor(int Proveedor)
		{

           


            var proveedorEditar = await _proveedorModel.GetProveedoresID(Proveedor);

			return View(proveedorEditar); // Pasa el proveedor a la vista
		}
        [Authorize(Roles = "Administrador,Gerente,Vendedor")]
        [HttpPost]
		public async Task<IActionResult> GuardarEditarProveedor([FromBody] ProveedorEnt proveedor)
		{
			if (proveedor == null || !ModelState.IsValid)
			{
				return BadRequest("Datos inválidos.");
			}

            int idUsuario = (int?)HttpContext.Session.GetInt32("idUsuario") ?? 0;
            BitacoraProveedor bit = new BitacoraProveedor
            {
                idProveedor = proveedor.idProveedor,
                idUsuario = idUsuario,
                accion = "El usuario Editó un  proveedor",
                fecha = DateTime.Now

            };
            _inventarioModel.RegistrarBitacoraProveedor(bit);



            try
			{
				proveedor.estado = true;
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

        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
		public async Task<IActionResult> DesactivarProveedor([FromBody] ProveedorEnt idProveedor)
		{
			try
			{

                int idUsuario = (int?)HttpContext.Session.GetInt32("idUsuario") ?? 0;
                BitacoraProveedor bit = new BitacoraProveedor
                {
                    idProveedor = idProveedor.idProveedor,
                    idUsuario = idUsuario,
                    accion = "El usuario Desactivó el  proveedor",
                    fecha = DateTime.Now

                };
                _inventarioModel.RegistrarBitacoraProveedor(bit);



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
