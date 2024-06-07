using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;

namespace CrediV1_Prueba.Entities
{
    public class UsuarioEnt
    {
        public long idUsuario { get; set; }
        public string nombre { get; set; }
        public string Usuario { get; set; } 
        public string apellidos { get; set; }
        public string nombreCompleto { get; set; }
        public string contrasenna { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public bool estado { get; set; }
        public long idRol { get; set; }
   
    }
}
