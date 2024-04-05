using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult ListaClientes()
        {

            return View();
        }
    }
}
