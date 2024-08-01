using CrediV1_Prueba.Entities.Otros;
using CrediV1_Prueba.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using CrediV1_Prueba.Entities.DTO;

namespace CrediV1_Prueba.Models
{
    public class InventarioModel : IInventarioModel
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _connection;

        public InventarioModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("Connection");
        }


        public async Task<IEnumerable<ProductosBajoStock>> ConsultarProductosBajosStock()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<ProductosBajoStock>("ConsultarBajoStock", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
        }

        public async  Task<IEnumerable<ProductosBajoStock>> ConsultarRecomendacionestock()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<ProductosBajoStock>("ConsultarRecomendacionesStock", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
        }

        public async Task<bool> RestablecerStockProducto(RestablecerCantidadDTO ent)
        {
            using (var connection = new SqlConnection(_connection))
            {
                var result = await connection.ExecuteAsync("RestablecerStockProducto",
                    new {ent.idProducto, ent.cantidadStock  },
                    commandType: System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    return true;
                }
                return false;

            }    
        }
    }
}
