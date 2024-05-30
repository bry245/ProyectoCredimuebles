using System.Data;
using System.Data.SqlClient;

namespace CrediV1_Prueba.Context
{
    public class DapperContext
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("CadenaSQL");

        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
