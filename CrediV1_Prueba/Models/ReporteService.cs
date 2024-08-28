using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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

        public async  Task<IEnumerable<ReporteEnt>> GetProductos()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<ReporteEnt>("GetAllProductos", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
        }
     
        public async Task<IEnumerable<ReporteEnt>> ObtenerAbonosMensuales()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var abonos = await connection.QueryAsync<ReporteEnt>("ConsultarSaldosPagadosMes", commandType: CommandType.StoredProcedure);
                return abonos.ToList();

            }
        }

        public async Task<ReporteEnt> ObtenerAbonosSemanales()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryFirstOrDefaultAsync<ReporteEnt>("ObtenerAbonosSemana", commandType: CommandType.StoredProcedure);
                return productos;

            }
        }

            public async Task<ReporteEnt> VentasDia()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryFirstOrDefaultAsync<ReporteEnt>("ObtenerVentasUltimas24Horas", commandType: CommandType.StoredProcedure);
                return productos;
            }
        }


        public async Task<IEnumerable<ReporteEnt>> VentasMensuales()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<ReporteEnt>("sp_VentasMensuales", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
        }

        public async Task<IEnumerable<ReporteEnt>> VentasPorMetodoPago()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<ReporteEnt>("sp_VentasPorMetodoPago", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
        }

        public async Task<IEnumerable<ReporteEnt>> VentasPorMetodoPagoCantidad()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<ReporteEnt>("sp_VentasPorMetodoPago", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
        }
    }
}
