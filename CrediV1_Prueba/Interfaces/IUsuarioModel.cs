using CrediV1_Prueba.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrediV1_Prueba.Interfaces
{
    public interface IUsuarioModel
    {
        public UsuarioEnt? IniciarSesion(UsuarioEnt entidad);


        public  Task<bool> VerificarContraseña(string contraseña, string hashContraseña);
      


        public Task<UsuarioEnt> consultarUsuariobyId(int idUsuario);

        public Task<IEnumerable<RolEnt>> ConsultarRoles();
        public  Task? RegistrarUsuario(UsuarioEnt usuario);
        public List<UsuarioEnt>? ListarUsuarios();
       
        public Task ActualizarUsuario(UsuarioEnt usuario);

        public List<UsuarioEnt>? ListarClientes();

        public  Task<string> DesactivarActivarUsuario(UsuarioEnt usuario);

    }
}
