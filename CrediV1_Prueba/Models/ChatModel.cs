using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices.Marshalling;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrediV1_Prueba.Models
{
	public class ChatModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
	{

		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private string _connection;
		private readonly IOtherServices _otherServices;

		public async Task<List<ReporteEnt>>? TraerDatosDB()
		{
			try
			{
				using (var con = new SqlConnection(_connection))
				{
					var dato = con.Query<ReporteEnt>("ConsultarProductos",
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
