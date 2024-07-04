using CrediV1_Prueba.Entities;

namespace CrediV1_Prueba.Interfaces
{
    public interface ILogin
    {

        public UsuarioEnt? IniciarSesion(UsuarioEnt entidad);


        public Task<bool> VerificarContraseña(string contraseña, string hashContraseña);


    }
}
