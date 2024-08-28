using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices.Marshalling;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrediV1_Prueba.Models
{
	public class ChatModel(HttpClient _httpClient, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor): IChatModel
	{

		private string _connection;

		

		public async Task<List<ProductoDB>>? TraerDatosDB()
		{
			_connection = _configuration.GetConnectionString("Connection");

			try
			{
				using (var con = new SqlConnection(_connection))
				{
					var dato = con.Query<ProductoDB>("ConsultarProductos",
					   new { },
					   commandType: CommandType.StoredProcedure).ToList();

					return dato;

				}
			}
			catch (SqlException exs)
			{
				throw new Exception("Error : " + exs.Message, exs);


			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				throw new Exception("Error: " + ex.Message, ex);
			}
		}



	}
}
