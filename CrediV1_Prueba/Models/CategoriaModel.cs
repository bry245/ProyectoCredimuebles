using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace CrediV1_Prueba.Models
{
    public class CategoriaModel : ICategoria
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _connection;

        public CategoriaModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("Connection");
        }

        public async Task<IEnumerable<CategoriaEnt>> GetCategorias()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var result = await connection.QueryAsync<CategoriaEnt>(
                    "GETALLCATEGORIAS",
                    commandType: CommandType.StoredProcedure);

                return result;
            }
        }
    }
}