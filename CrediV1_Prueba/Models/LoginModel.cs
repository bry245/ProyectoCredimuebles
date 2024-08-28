using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;


namespace CrediV1_Prueba.Models
{
    public class LoginModel : ILogin
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _connection;
        private readonly IOtherServices _otherServices;

        public LoginModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IOtherServices otherServices)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("Connection");
            _otherServices = otherServices;
        }


        public UsuarioEnt? IniciarSesion(UsuarioEnt entidad)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var dato = con.Query<UsuarioEnt>("getUserbyEmail",
                        new { entidad.correo },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();


                    return dato;


                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<bool> VerificarContraseña(string contraseña, string hashContraseña)
        {
            try
            {
                
                bool validatepassword = BCrypt.Net.BCrypt.Verify(contraseña, hashContraseña);

                Console.WriteLine("Resultado de la verificación: " + validatepassword);
                return validatepassword;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al verificar la contraseña: " + ex.Message);
                return false;
            }
        }




    }
}
