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
        private readonly IOtherServices _otherServices;

        public InicioController(IHttpClientFactory clientFactory, IConfiguration configuration, IUsuarioModel usuarioModel,
            IOtherServices otherServices)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connection = _configuration.GetConnectionString("Connection");
            _usuarioModel = usuarioModel;
            _otherServices = otherServices;
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
            if (dato != null)
            {
                HttpContext.Session.SetString("NombreUsuario", dato.Usuario);
                HttpContext.Session.SetString("Rol", dato.descripcionRol);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Mensaje"] = "Error al ingresar, valide sus datos";
                return RedirectToAction("InicioDeSesion", "Inicio");
            }
        }

        [HttpGet]
        public IActionResult RecuperarContrasenna()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GestionarCambioContrasenna(UsuarioEnt entidad)
        {
            var dato = _usuarioModel.ConsultarCorreo(entidad);
            if (dato != null)
            {
                string bodyMsg = _otherServices.GenerarHtml(dato);
                //falta arma el html y enviar el  correo

                _otherServices.EnviarCorreo(entidad.correo, "Recuperar Contraseña", bodyMsg);
                TempData["Mensaje"] = "Se ha enviado un mensaje a su correo para continuar con la recuperación de su contraseña";
                //lo redirige
                return RedirectToAction("InicioDeSesion", "Inicio");
            }
            else
            {
                TempData["MensajeCorreo"] = "Verifique que el correo ingresado sea correcto";

            }
            return RedirectToAction("RecuperarContrasenna", "Inicio");
        }


        [HttpGet]
        public IActionResult CambiarContrasenna(string q)
        {

            var usuario = new UsuarioEnt();
            var id=long.Parse(_otherServices.Decrypt(q));
            usuario.idUsuario = id;
            // var id= usuario.idEncriptado = q;
            return View(usuario);
        }
        [HttpPost]
        public IActionResult ActualizarContrasenna(UsuarioEnt usuario)
        {
            //var dato = long.Parse(_otherServices.Decrypt(usuario.idEncriptado));
           // usuario.idUsuario = dato;
            var validacion = _usuarioModel.validarCambioContrasenna(usuario);
            if (validacion > 0)
            {
                TempData["Mensaje"] = "El cambió de contraseña se ha realizado con éxito";
                return RedirectToAction("InicioDeSesion", "Inicio");
            }
            else
            {
                TempData["Mensaje"] = "Ocurrió un error, valide su usuario y correo";
                return RedirectToAction("CambiarContrasenna", "Inicio");
            }
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

