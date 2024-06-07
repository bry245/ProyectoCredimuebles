using Dapper;
using static CrediV1_Prueba.Entities.ProveedorEnt;
using static CrediV1_Prueba.Models.ProveedoresModel;

using System.Data;
using System.Configuration;
using System.Net.Http;
using CrediV1_Prueba.Interfaces;
using CrediV1_Prueba.Entities;

namespace CrediV1_Prueba.Models
{
    
        public class ProveedoresModel : IProveedoresModel
        {

            private readonly HttpClient _httpClient;
            private readonly IConfiguration _configuration;
            private string _connection;


            public ProveedoresModel(HttpClient httpClient, IConfiguration configuration)
            {
                _httpClient = httpClient;
                _configuration = configuration;
                _connection = _configuration.GetConnectionString("Connection");
            }

        /*Método de ejemplo*/
            public int AddProveedor(ProveedorEnt proveedor)
            {
                return 1;
            }
            /*
            public async Task AddProveedores(Proveedor proveedor)
            {

                var query = "INSERT INTO Proveedor (nombre,telefono,correo,direccion) VALUES(@nombre,@telefono,@correo,@direccion)"
                    + "SELECT CAST(SCOPE_IDENTITY() AS int)";
                var parameters = new DynamicParameters();
                parameters.Add("nombre", proveedor.nombre, DbType.String);
                parameters.Add("telefono", proveedor.telefono, DbType.String);
                parameters.Add("correo", proveedor.correo, DbType.String);
                parameters.Add("direccion", proveedor.direccion, DbType.String);
                using (var connection = _context.CreateConnection())
                {

                    var id = await connection.QuerySingleAsync<int>(query, parameters);

                    var createdProveedor = new Proveedor
                    {
                        idProveedor = id,
                        nombre = proveedor.nombre,
                        telefono = proveedor.telefono,
                        correo = proveedor.correo,
                        direccion = proveedor.direccion


                    };

                }

            }


            public async Task<IEnumerable<Proveedor>> GetProveedores()
            {

                var query = "SELECT * FROM Proveedor";
                using (var connection = _context.CreateConnection())
                {
                    var proveedores = await connection.QueryAsync<Proveedor>(query);
                    return proveedores.ToList();
                }

            }

            public async Task<Proveedor> GetProveedoresID(int idProveedor)
            {
                var query = "SELECT * FROM Proveedor WHERE idProveedor = @idProveedor";
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("idProveedor", idProveedor, DbType.Int32);

                    var proveedor = await connection.QuerySingleOrDefaultAsync<Proveedor>(query, parameters);
                    return proveedor;
                }
            }

            public async Task<IEnumerable<Proveedor>> GetProveedoresNombre(string nombre)
            {
                var query = "SELECT * FROM Proveedor WHERE nombre LIKE @nombre + '%'";
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("{", nombre, DbType.String);

                    var proveedor = await connection.QueryAsync<Proveedor>(query, parameters);
                    return proveedor.ToList();
                }
            }

            public async Task UpdateProveedor(Proveedor proveedor)
            {
                var query = "UPDATE Proveedor SET nombre = @nombre, telefono = @telefono, direccion = @direccion, correo = @correo WHERE idProveedor = @idProveedor";
                var parameters = new DynamicParameters();
                parameters.Add("idProveedor", proveedor.idProveedor, DbType.Int32);
                parameters.Add("nombre", proveedor.nombre, DbType.String);
                parameters.Add("telefono", proveedor.telefono, DbType.String);
                parameters.Add("direccion", proveedor.direccion, DbType.String);
                parameters.Add("correo", proveedor.correo, DbType.String);

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }
            }*/

        }
    }

