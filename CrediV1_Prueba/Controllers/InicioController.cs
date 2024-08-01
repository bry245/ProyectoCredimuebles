using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Entities.Otros;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Data.SqlClient;
using CrediV1_Prueba.Models;
using CrediV1_Prueba.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Authorization;


namespace CrediV1_Prueba.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class InicioController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private string _connection;
        private readonly IUsuarioModel _usuarioModel;
        private readonly ILogin _loginModel;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordResetService _passwordResetService;

        public InicioController(IHttpClientFactory clientFactory, IPasswordResetService passwordResetService,
            IHttpContextAccessor httpContextAccessor, ILogin loginModel, IConfiguration configuration, IUsuarioModel usuarioModel)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connection = _configuration.GetConnectionString("Connection");
            _usuarioModel = usuarioModel;
            _loginModel = loginModel;
            _httpContextAccessor = httpContextAccessor;
            _passwordResetService = passwordResetService;
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        [HttpGet]
		public async Task<IActionResult> InicioDeSesion()
		{
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IniciarSesion([FromBody] UsuarioEnt usuario)
        {
            try
            {
               

                var usuarioValidar = _loginModel.IniciarSesion(usuario);
                if (usuarioValidar == null)
                {
                    TempData["Mensaje"] = "Usuario no registrado";
                    return BadRequest("Usuario no registrado.");
                }


                bool auth = await _loginModel.VerificarContraseña(usuario.contrasenna, usuarioValidar.contrasenna);

                if (auth)
                {



                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuarioValidar.nombre),
                        new Claim("Correo", usuarioValidar.correo),
                        new Claim("UserId", usuarioValidar.idUsuario.ToString())
                    };
                    string nombreRol = usuarioValidar.descripcionRol;
                    claims.Add(new Claim(ClaimTypes.Role, nombreRol));


                    HttpContext.Session.SetInt32("idUsuario", (int)usuarioValidar.idUsuario);
                    HttpContext.Session.SetString("Email", usuarioValidar.correo);
                    HttpContext.Session.SetString("Apellido", usuarioValidar.apellidos);
                    HttpContext.Session.SetString("Nombre", usuarioValidar.nombre);

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));





                    TempData["Mensaje"] = "Inicio de sesión exitoso.";
                    return Ok("Inicio de sesión exitoso.");
                }
                else
                {
                    TempData["Mensaje"] = "Credenciales incorrectas.";
                    return BadRequest("Credenciales incorrectas.");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Hubo un problema al iniciar sesión.";
                return StatusCode(500, $"Hubo un problema al iniciar sesión: {ex.Message}");
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult CambiarContrasennaVista(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CambiarContrasenna([FromBody] ResetPasswordViewModel model)
        {
            try
            {
               

                await _passwordResetService.ResetPasswordAsync(model.Email, model.Token);
                return Ok("Contraseña cambiada exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al cambiar la contraseña: " + ex.Message);
            }
        }






        [AllowAnonymous]
        [HttpGet]
        public IActionResult RecuperarContrasenna()
        {



            return View();
        }

   
        [AllowAnonymous]
        [HttpGet]
        public IActionResult VerificacionRecuperarContrasenna(string email)
        {
            ViewBag.Email = email;
            return View();
        }




        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> VerificacionCodigoRecuperarContrasenna([FromBody] ResetPasswordViewModel model)
        {
            try
            {
                var codigoExiste = await _passwordResetService.VerifyPasswordResetTokenAsync(model.Email, model.Token);

                if (codigoExiste)
                {
                    // Redirigir a la vista para cambiar la contraseña
                    return Json(new { success = true });
                }
                else
                {
                    return BadRequest(new { message = "Código incorrecto o vencido." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al procesar la solicitud: " + ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GestionarCambioContrasenna([FromBody] UsuarioEnt entidad)
        {
            try
            {
                var usuarioExistente = _usuarioModel.consultarUsuariobyCorreo(entidad.correo);

                if (usuarioExistente != null)
                {
                    await _passwordResetService.GeneratePasswordResetTokenAsync(entidad.correo);
                    TempData["Email"] = entidad.correo;
                    return Json(new { success = true, redirectUrl = Url.Action("VerificacionRecuperarContrasenna", "Inicio") });
                }
                else
                {
                    return Json(new { success = false, message = "El correo electrónico no está registrado." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al procesar la solicitud: " + ex.Message);
                return Json(new { success = false, message = "Error al procesar la solicitud: " + ex.Message });
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



        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear(); // Limpiar datos de la sesión si es necesario

            TempData["Mensaje"] = "Sesión cerrada correctamente.";
            return RedirectToAction("InicioDeSesion", "Inicio"); // Redirigir a la página de inicio de sesión u otra página
        }













    }
}

