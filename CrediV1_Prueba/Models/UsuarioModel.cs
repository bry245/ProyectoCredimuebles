using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data;
using System.Data.SqlClient;

namespace CrediV1_Prueba.Models
{
    public class UsuarioModel : IUsuarioModel
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _connection;

        public UsuarioModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("Connection");
        }

        public UsuarioEnt? IniciarSesion(UsuarioEnt entidad)
        {
            using (var con = new SqlConnection(_connection))
            {
                var dato = con.Query<UsuarioEnt>("IniciarSesion",
                    new { entidad.correo, entidad.contrasenna },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                
                if(dato!= null)
                {
                    return dato;
                }
                else
                {
                    return null;
                }
            }
        }

     public UsuarioEnt? ConsultarCorreo(UsuarioEnt entidad)
        {
            using (var con = new SqlConnection(_connection))
            {
                var dato = con.Query<UsuarioEnt>("consultarCorreo",
                    new { entidad.correo},
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (dato != null)
                {
                    return dato;
                }
                else
                {
                    return null;
                }

            }
        }

        
    }
}
