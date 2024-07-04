using CrediV1_Prueba.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrediV1_Prueba.Interfaces
{
    public interface IUsuarioModel
    {
     
      


        public Task<UsuarioEnt> consultarUsuariobyId(int idUsuario);

        public Task<UsuarioEnt> consultarUsuariobyCorreo(string correo);

        public Task<IEnumerable<RolEnt>> ConsultarRoles();
        public  Task? RegistrarUsuario(UsuarioEnt usuario);
        public  List<UsuarioEnt>? ListarUsuarios();
       
        public Task ActualizarUsuario(UsuarioEnt usuario);

        public Task <List<UsuarioEnt>>? ListarClientes();

        public  Task<string> DesactivarActivarUsuario(UsuarioEnt usuario);

    }
}
