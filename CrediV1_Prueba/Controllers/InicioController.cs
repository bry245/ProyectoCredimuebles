using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Entities.Otros;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Data.SqlClient;
using CrediV1_Prueba.Models;
using CrediV1_Prueba.Interfaces;


namespace CrediV1_Prueba.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class InicioController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private string _connection;
        private readonly IUsuarioModel _usuarioModel;

        public InicioController(IHttpClientFactory clientFactory, IConfiguration configuration, IUsuarioModel usuarioModel)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connection = _configuration.GetConnectionString("Connection");
            _usuarioModel = usuarioModel;
        }





		[HttpGet]
		public async Task<IActionResult> InicioDeSesion()
		{
			var currentDate = DateTime.Now;
			ViewData["CurrentDate"] = currentDate.ToString("dd/MM/yyyy");

			string apiKey = "565e57a6f4ee1cdf4a1fb330";
			string baseCurrency = "USD";
			string url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{baseCurrency}";

			var client = _clientFactory.CreateClient();
			var response = await client.GetAsync("https://api.exchangerate-api.com/v4/latest/USD");

			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsAsync<ExchangeRateResponse>();
				var exchangeRate = data.Rates.CRC; // Colones (CRC)
				ViewData["ExchangeRate"] = exchangeRate;
				return View();
			}
			else
			{
				ViewData["ExchangeRate"] = "Error al obtener el tipo de cambio";
				return View();
			}
		}


		        [HttpPost]

        public IActionResult IniciarSesion(UsuarioEnt usuario)
        {
            var dato = _usuarioModel.IniciarSesion(usuario);
            if (dato!= null){
                HttpContext.Session.SetString("NombreUsuario", dato.Usuario);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Mensaje"]= "Error al ingresar, valide sus datos";
                return RedirectToAction("InicioDeSesion","Inicio");
            }


        }        [HttpPost]


        



        [HttpGet]
        public IActionResult RecuperarContrasenna()
        {


            return View();
        }

        [HttpPost]
        public IActionResult GestionarCambioContrasenna(UsuarioEnt entidad)
        {
            var dato = _usuarioModel.ConsultarCorreo(entidad);  
            if (dato!= null)
            {
                //falta arma el correo
                //lo redirige
            }
            return View();
        }


        public async Task<IActionResult> TipoCambioEnColones()
        {
            string apiKey = "565e57a6f4ee1cdf4a1fb330"; // Reemplaza con tu clave de API
            string baseCurrency = "USD";
            string targetCurrency = "CRC";
            string url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{baseCurrency}";

            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://api.exchangerate-api.com/v4/latest/USD");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<ExchangeRateResponse>();
                var exchangeRate = data.Rates; // Colones (CRC)
                ViewData["ExchangeRate"] = exchangeRate;
                return View();
            }
            else
            {
                ViewData["ExchangeRate"] = "Error al obtener el tipo de cambio";
                return View();
            }
        }

















    }
}

