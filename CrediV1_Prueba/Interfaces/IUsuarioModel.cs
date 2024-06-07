using CrediV1_Prueba.Entities;

namespace CrediV1_Prueba.Interfaces
{
    public interface IUsuarioModel
    {
        public UsuarioEnt? IniciarSesion(UsuarioEnt entidad);
        public UsuarioEnt? ConsultarCorreo(UsuarioEnt entidad);
    }
}
