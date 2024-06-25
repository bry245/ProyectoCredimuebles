using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace CrediV1_Prueba.Models
{
	public class ProductosModel : IProducto
	{

		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private string _connection;


		public ProductosModel(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_connection = _configuration.GetConnectionString("Connection");
		}

		public async  Task<IEnumerable<ProductoEnt>> GetProductos()
		{
			using (var connection = new SqlConnection(_connection))
			{
				var productos = await connection.QueryAsync<ProductoEnt>("GetAllProductos", commandType: CommandType.StoredProcedure);
					return productos.ToList();
			}
			
		}


		public async Task<string> DesactivarProducto(ProductoEnt producto)
		{
			using (var connection = new SqlConnection(_connection))
			{
				var parameters = new DynamicParameters();
				parameters.Add("idProducto", producto.idProducto, DbType.Int64);

				var result = await connection.QuerySingleAsync<string>(
					"DesactivarActivarProducto",
					parameters,
					commandType: CommandType.StoredProcedure);

				return result;
			}

		}


        }

}
