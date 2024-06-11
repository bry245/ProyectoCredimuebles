using Dapper;
using static CrediV1_Prueba.Entities.ProveedorEnt;
using static CrediV1_Prueba.Models.ProveedoresModel;

using System.Data;
using System.Configuration;
using System.Net.Http;
using CrediV1_Prueba.Interfaces;
using CrediV1_Prueba.Entities;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

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


		public async Task<bool> AddProveedor(ProveedorEnt proveedor)
		{
			var parameters = new DynamicParameters();
			parameters.Add("nombre", proveedor.nombre, DbType.String);
			parameters.Add("telefono", proveedor.telefono, DbType.String);
			parameters.Add("correo", proveedor.correo, DbType.String);
			parameters.Add("direccion", proveedor.direccion, DbType.String);
			parameters.Add("Estado", proveedor.Estado, DbType.Boolean);

			using (var connection = new SqlConnection(_connection))
			{
				var filasAfectadas = await connection.ExecuteAsync("AddProveedor", parameters, commandType: CommandType.StoredProcedure);

				// Verificar si se insertó alguna fila
				if (filasAfectadas > 0)
				{
					// Obtener el ID del proveedor insertado (opcional)
					// var idProveedor = await connection.ExecuteScalarAsync<int>("SELECT CAST(SCOPE_IDENTITY() AS int)");

					return true; // Se insertó correctamente
				}
				else
				{
					return false; // No se insertó ninguna fila
				}
			}
		}






		public async Task<IEnumerable<ProveedorEnt>> GetProveedores()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var proveedores = await connection.QueryAsync<ProveedorEnt>("GetProveedores", commandType: CommandType.StoredProcedure);
                return proveedores.ToList();
            }
        }

        public async Task<ProveedorEnt> GetProveedoresID(int idProveedor)
        {
            var parameters = new DynamicParameters();
            parameters.Add("idProveedor", idProveedor, DbType.Int32);

            using (var connection = new SqlConnection(_connection))
            {
                var proveedor = await connection.QuerySingleOrDefaultAsync<ProveedorEnt>("GetProveedorByID", parameters, commandType: CommandType.StoredProcedure);
                return proveedor;
            }
        }

        public async Task UpdateProveedor(ProveedorEnt proveedor)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idProveedor", proveedor.idProveedor, DbType.Int32);
            parameters.Add("@nombre", proveedor.nombre, DbType.String);
            parameters.Add("@telefono", proveedor.telefono, DbType.String);
            parameters.Add("@correo", proveedor.correo, DbType.String);
            parameters.Add("@direccion", proveedor.direccion, DbType.String);
            parameters.Add("@Estado", proveedor.Estado, DbType.Boolean);

            using (var connection = new SqlConnection(_connection))
            {
                await connection.ExecuteAsync("UpdateProveedorBD", parameters, commandType: CommandType.StoredProcedure);
            }
        }

		public async Task DesactivarProveedor(ProveedorEnt proveedor)
		{
			var parameters = new DynamicParameters();
			parameters.Add("idProveedor", proveedor.idProveedor, DbType.Int32);

			using (var connection = new SqlConnection(_connection))
			{
				await connection.ExecuteAsync("DesactivarActivarProveedorBD", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		public Task SearchProveedorByEmail(ProveedorEnt proveedor)
		{
			throw new NotImplementedException();
		}
	}
}

