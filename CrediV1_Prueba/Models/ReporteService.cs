using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace CrediV1_Prueba.Models
{
    public class ReporteService : IReporteService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _connection;


        public ReporteService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("Connection");
        }

        public async  Task<IEnumerable<SalidasEnt>> GetProductos()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<SalidasEnt>("GetAllProductos", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
        }

        public async Task<IEnumerable<ReporteEnt>> VentasMensuales()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<ReporteEnt>("GetAllProductos", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
        }

        public async Task<IEnumerable<SalidasEnt>> VentasPorMetodoPago()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<SalidasEnt>("GetAllProductos", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
        }

        public async Task<IEnumerable<SalidasEnt>> VentasPorMetodoPagoCantidad()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<SalidasEnt>("GetAllProductos", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
        }
    }
}
