using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private string _connection;
        private readonly IUsuarioModel _usuarioModel;
        private readonly IOtherServices _otherServices;

        public UsuarioController(IHttpClientFactory clientFactory, IConfiguration configuration, IUsuarioModel usuarioModel,
            IOtherServices otherServices)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connection = _configuration.GetConnectionString("Connection");
            _usuarioModel = usuarioModel;
            _otherServices = otherServices;
        }
        [HttpGet]
        public IActionResult ListaClientes()
        {
            ViewBag.Usuarios = _usuarioModel.ListarUsuarios();
            ViewBag.Clientes = _usuarioModel.ListarClientes();
            ViewBag.Roles = _usuarioModel.ConsultarRoles();
            return View();
        }

        [HttpGet]
        public IActionResult ConsultarRoles()
        {
            ViewBag.Roles = _usuarioModel.ConsultarRoles();

            return View();
        }


        public async Task<IActionResult> RegistrarUsuario(UsuarioEnt usuario)
        {
            var salida = _usuarioModel.RegistrarUsuario(usuario);

            if (salida != null)
            {
                var mensaje = _otherServices.GenerarHtmlBienvenida(salida);

                _otherServices.EnviarCorreo(salida.correo, "Bienvenido a Credimuebles CR", mensaje);
                TempData["Mensaje"] = "Usuario registrado correctamente";

            }
            else
            {
                TempData["Mensaje"] = "Ocurrió un error al ingresar los datos, intente de nuevo";

            }
            return RedirectToAction("ListaClientes", "Usuario");
        }


        [HttpGet]
        public IActionResult ActualizarUsuario(long q)
        {

            ViewBag.Roles = _usuarioModel.ConsultarRoles();
            var usuario = _usuarioModel.ConsultarUsuario(q);

            if (usuario != null)
            {
                usuario.idUsuario = q;
                return View(usuario);
            }
            else
            {
                TempData["Mensaje"] = "Error al ver detalles del usuario";
                return RedirectToAction("ListaClientes", "Usuario");
            }
        }
        [HttpPost]

        public IActionResult ActualizarUsuario(UsuarioEnt usuario)
        {
         
            var dato = _usuarioModel.ActualizarUsuario(usuario);

            if (dato>=0)
            {
                return RedirectToAction("ListaClientes","Usuario");
            }
            else
            {
                return View(usuario);
            }
        }

        public async Task<IActionResult> CambiarEstadoUsuario(long q)
        {
            _usuarioModel.CambiarEstado(q);

            return RedirectToAction("ActualizarUsuario", "Usuario", new { q = q });
        }
    }
}

