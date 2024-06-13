using CrediV1_Prueba.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrediV1_Prueba.Interfaces
{
    public interface IUsuarioModel
    {
        public UsuarioEnt? IniciarSesion(UsuarioEnt entidad);
        public UsuarioEnt? ConsultarCorreo(UsuarioEnt entidad);
        public int validarCambioContrasenna(UsuarioEnt usuario);
        public List<SelectListItem>? ConsultarRoles();
        public UsuarioEnt? RegistrarUsuario(UsuarioEnt usuario);
        public List<UsuarioEnt>? ListarUsuarios();
        public UsuarioEnt? ConsultarUsuario(long q);
        public int ActualizarUsuario(UsuarioEnt usuario);
        public int CambiarEstado(long q);
        public List<UsuarioEnt>? ListarClientes();
    }
}
