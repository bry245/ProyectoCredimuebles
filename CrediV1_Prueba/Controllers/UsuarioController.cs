using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using CrediV1_Prueba.Models;
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
        public  IActionResult ListaClientes()
        {
            try
            {
                var usuarios =  _usuarioModel.ListarClientes();

                return View(usuarios);

            }catch (Exception ex)
            {

            }
            return View();
        }

        [HttpGet]
        public IActionResult ListaEmpleados()
        {
            try
            {
                var usuarios = _usuarioModel.ListarUsuarios();

                return View(usuarios);

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpGet]
        public IActionResult Usuarios()
        {
    
            return View();
        }

        [HttpGet]
        public IActionResult ConsultarRoles()
        {
           

            return View();
        }


        [HttpGet]
        public IActionResult AgregarCLiente()
        {
          

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AgregarEmpleado()
        {

            var roles = await _usuarioModel.ConsultarRoles();
            ViewData["roles"] = roles;


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GuardarEmpleado([FromBody] UsuarioEnt user)
        {

            try
            {

                if (user == null)
                {
                    return BadRequest(string.Empty);
                }

                Console.WriteLine("ASDSA" + user.cedula);
                Console.WriteLine("ASDSA" + user.apellidos);
                Console.WriteLine("ASDSA" + user.telefono);
                Console.WriteLine("ASDSA" + user.contrasenna);
                Console.WriteLine("ASDSA" + user.direccion);
                Console.WriteLine("ASDSA" + user.nombre);
                user.estado = true;
                user.contrasenna = "1232131";
                await _usuarioModel.RegistrarUsuario(user);

                return Ok();


            }
            catch (Exception ex)
            {

                return BadRequest(ex);

            }
        }



        [HttpPost]
        public async Task <IActionResult> GuardarCliente([FromBody] UsuarioEnt user)
        {

            try
            {

                if (user == null)
                {
                    return BadRequest(string.Empty);
                }

                Console.WriteLine("ASDSA" + user.cedula);
                Console.WriteLine("ASDSA" + user.apellidos);
                Console.WriteLine("ASDSA" + user.telefono);
                Console.WriteLine("ASDSA" + user.contrasenna);
                Console.WriteLine("ASDSA" + user.direccion);
                Console.WriteLine("ASDSA" + user.nombre);
                user.idRol = 5;
                user.estado = true;
                user.contrasenna = "1232131";
                await _usuarioModel.RegistrarUsuario(user);

                return Ok();


            }
            catch (Exception ex)
            {

                return BadRequest(ex);

            }
        }

        [HttpGet]
        public async Task <IActionResult> EditarUsuario(int Usuario)
        {

            try
            {
                Console.WriteLine("AAAAAAAAGHJDS" + Usuario);
                var usuarioEditar = await _usuarioModel.consultarUsuariobyId(Usuario);
                var roles = await _usuarioModel.ConsultarRoles();
                ViewData["roles"] = roles;

                return View(usuarioEditar);
              

            }catch (Exception ex)
            {

            }


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GuardarEditarUsuario([FromBody] UsuarioEnt user)
        {

            try
            {

                if (user == null)
                {
                    return BadRequest(string.Empty);
                }

                Console.WriteLine("ASDSA" + user.cedula);
                Console.WriteLine("ASDSA" + user.apellidos);
                Console.WriteLine("ASDSA" + user.telefono);
                Console.WriteLine("ASDSA" + user.contrasenna);
                Console.WriteLine("ASDSA" + user.direccion);
                Console.WriteLine("ASDSA" + user.nombre);
                await _usuarioModel.ActualizarUsuario(user);

                return Ok();


            }
            catch (Exception ex)
            {

                return BadRequest(ex);

            }
        }


        public async Task<IActionResult> RegistrarUsuario(UsuarioEnt usuario)
        {
            var salida = _usuarioModel.RegistrarUsuario(usuario);

          
          
            return RedirectToAction("ListaClientes", "Usuario");
        }


        [HttpGet]
        public IActionResult ActualizarUsuario(long q)
        {

            return View();
        }
    

        public async Task<IActionResult> CambiarEstadoUsuario(long q)
        {
           

            return RedirectToAction("ActualizarUsuario", "Usuario", new { q = q });
        }


        [HttpPost]
        public async Task<IActionResult>DesactivarActivarUsuario([FromBody] UsuarioEnt usuario)
        {
            try
            {
                Console.WriteLine("ID USUARIO "+usuario.idUsuario);
                var mensaje = await _usuarioModel.DesactivarActivarUsuario(usuario);

                if (mensaje == "Usuario desactivado exitosamente" || mensaje == "Usuario activado exitosamente")
                {
                    return Ok(mensaje);
                }
                else
                {
                    return NotFound(mensaje);
                }
               
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

