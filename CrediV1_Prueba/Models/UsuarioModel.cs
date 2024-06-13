using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
                    var dato = con.Query<UsuarioEnt>("IniciarSesion",
                        new { entidad.correo, entidad.contrasenna },
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
            }catch( Exception ex )
            {
                return null;
            }
        }
        public UsuarioEnt? ConsultarCorreo(UsuarioEnt entidad)
        {
            using (var con = new SqlConnection(_connection))
            {
                var dato = con.Query<UsuarioEnt>("consultarCorreo",
                    new { entidad.correo },
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

        public int validarCambioContrasenna(UsuarioEnt usuario)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                     var dato = con.Execute("validarCambioContrasenna",
                        new { usuario.correo, usuario.contrasenna, usuario.Usuario,usuario.idUsuario},
                        commandType: CommandType.StoredProcedure);

                    return dato;

                }
            } catch (Exception ex)
            {
                return -1;
            }
        }



        // Gestión Usuarios 

        public List<SelectListItem>? ConsultarRoles()
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var dato = con.Query<SelectListItem>("ConsultarRoles", new {},
                       commandType: CommandType.StoredProcedure).ToList();
                    return dato;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
      
        public UsuarioEnt? RegistrarUsuario(UsuarioEnt usuario)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var dato = con.Query<UsuarioEnt>("registrarUsuario",
                       new { usuario.nombre, usuario.apellidos, usuario.correo, usuario.telefono,usuario.direccion,usuario.idRol,usuario.cedula },
                       commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return dato;

                }
            }
            catch (Exception ex)
            {
                return null;
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

        public UsuarioEnt? ConsultarUsuario(long q)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var dato = con.Query<UsuarioEnt>("ConsultarUsuario",
                       new {q},
                       commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return dato;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int ActualizarUsuario(UsuarioEnt usuario)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var dato = con.Execute("ActualizarUsuario",
                       new { usuario.idRol,usuario.idUsuario,usuario.nombre,usuario.apellidos,usuario.correo,usuario.telefono,usuario.direccion
                       , usuario.cedula},
                       commandType: CommandType.StoredProcedure);

                    return dato;
    

                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public int CambiarEstado(long q)
        {
             try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var dato = con.Execute("CambiarEstadoUsuario",
                       new {q },
                       commandType: CommandType.StoredProcedure);

                    return dato;
    

                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

    }
}
