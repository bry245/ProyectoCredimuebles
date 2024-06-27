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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrediV1_Prueba.Models
{
    public class UsuarioModel : IUsuarioModel
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _connection;
        private readonly IOtherServices _otherServices;

        public UsuarioModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,IOtherServices otherServices)
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
            }catch( Exception ex )
            {
                return null;
            }
        }
       



        public async Task RegistrarUsuario(UsuarioEnt usuario)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("Connection");
                using (var connection = new SqlConnection(connectionString))
                {
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(usuario.contrasenna);

                    var parameters = new DynamicParameters();
                    parameters.Add("@nombre", usuario.nombre);
                    parameters.Add("@apellidos", usuario.apellidos);
                    parameters.Add("@contrasenna", passwordHash);
                    parameters.Add("@correo", usuario.correo);
                    parameters.Add("@telefono", usuario.telefono);
                    parameters.Add("@direccion", usuario.direccion);
                    parameters.Add("@estado", usuario.estado);
                    parameters.Add("@idRol", usuario.idRol);
                    parameters.Add("@cedula", usuario.cedula);

                    await connection.ExecuteAsync("registrarCliente", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar el usuario: " + ex.Message);
                throw new Exception("Error al registrar el usuario: " + ex.Message, ex);
            }
        }


        public List<UsuarioEnt>? ListarUsuarios()
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var dato = con.Query<UsuarioEnt>("ListarUsuarios",
                       new {  },
                       commandType: CommandType.StoredProcedure).ToList();

                    return dato;
                    
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public List<UsuarioEnt>? ListarClientes()
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var dato = con.Query<UsuarioEnt>("ListarClientes",
                       new { },
                       commandType: CommandType.StoredProcedure).ToList();

                    return dato;

                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

      
        public async Task ActualizarUsuario(UsuarioEnt usuario)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idUsuario", usuario.idUsuario, DbType.Int32);
                parameters.Add("@nombre", usuario.nombre, DbType.String);
                parameters.Add("@apellidos", usuario.apellidos, DbType.String);
                parameters.Add("@cedula", usuario.cedula, DbType.String);
                parameters.Add("@telefono", usuario.telefono, DbType.String);
                parameters.Add("@correo", usuario.correo, DbType.String);
                parameters.Add("@direccion", usuario.direccion, DbType.String);
                parameters.Add("@Estado", usuario.estado, DbType.Boolean);
                parameters.Add("@idRol", usuario.idRol, DbType.Int32);

                using (var connection = new SqlConnection(_connection))
                {
                    await connection.ExecuteAsync("ActualizarUsuario", parameters, commandType: CommandType.StoredProcedure);
                }


            }
            
            catch (Exception ex)
            {
                
            }
        }
      

        public async Task<bool> VerificarContraseña(string contraseña, string hashContraseña)
        {
            try
            {

                bool validatepassword = BCrypt.Net.BCrypt.Verify(contraseña, hashContraseña);


                return validatepassword;

            }
            catch (Exception ex)
            {

                return false;
               
            }

        }

        public async Task<string> DesactivarActivarUsuario(UsuarioEnt usuario)
        {
            var parameters = new DynamicParameters();
            parameters.Add("idUsuario", usuario.idUsuario, DbType.Int64); // Asegúrate de que el tipo sea Int64 si BIGINT en SQL

            using (var connection = new SqlConnection(_connection))
            {

                var result = await connection.QuerySingleAsync<string>(
                    "DesactivarActivarUsuario",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<UsuarioEnt> consultarUsuariobyId(int idUsuario)
        {
            using (var connection = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("idUsuario", idUsuario, DbType.Int64);

                var usuario = await connection.QueryFirstOrDefaultAsync<UsuarioEnt>(
                    "ConsultarUsuarioById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return usuario;
            }
        }

        public async Task<IEnumerable<RolEnt>> ConsultarRoles()
        {
            try
            {
                using (var connection = new SqlConnection(_connection))
                {
                    var parameters = new DynamicParameters();
                    var roles = await connection.QueryAsync<RolEnt>("ConsultarRoles",commandType: CommandType.StoredProcedure);
                    return roles;
                }

            }catch (Exception ex)
            {
                return null;

            }
        }
    }
}
