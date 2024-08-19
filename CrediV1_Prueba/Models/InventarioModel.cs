using CrediV1_Prueba.Entities.Otros;
using CrediV1_Prueba.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using CrediV1_Prueba.Entities.DTO;
using CrediV1_Prueba.Entities;

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

        public void ActualizarPedido(RegistrarPedidoDTO ent)
        {
            using (var connection = new SqlConnection(_connection))
            {
                var idPedido = connection.Query("ActualizarPedido", new
                {
                    ent.idPedido,
                    ent.idDetalle,
                    ent.idProducto,
                    ent.idProveedor,
                    ent.cantidad,
                    ent.montoUnitario,
                    ent.montoTotalProducto,
                    ent.montoTotalPedido,
                    ent.fechaRecibido,
                    ent.EmpleadoRecibido
                }, commandType: System.Data.CommandType.StoredProcedure);


            }
        }

        public async Task<string> ConfirmarPedido(RegistrarPedidoDTO ent)
        {
            using (var connection = new SqlConnection(_connection))
            {
                try
                {
                    var result = await connection.QuerySingleAsync<string>("ConfirmarRecibimientoPedido", new
                    {
                        ent.idDetalle,
                        ent.EmpleadoRecibido,
                        ent.fechaRecibido,
                    }, commandType: CommandType.StoredProcedure);
                    Console.WriteLine($"Rows Affected: {result}");
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<IEnumerable<PedidoEnt>> ConsultarPedidoDetallesPorID(long id)
        {
            using (var connection = new SqlConnection(_connection))
            {
                var pedido = await connection.QueryAsync<PedidoEnt>(
                    "ConsultarDetallePedido",
                    new { idPedido = id },
                    commandType: CommandType.StoredProcedure
                );

                return pedido.ToList();
            }
        }



        public async  Task<IEnumerable<PedidoEnt>> ConsultarPedidos()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<PedidoEnt>("ConsultarPedidos", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
        }

        public async Task<IEnumerable<PedidoEnt>> ConsultarPedidosDetalles()
        {
            using (var connection = new SqlConnection(_connection))
            {
                var productos = await connection.QueryAsync<PedidoEnt>("ConsultarPedidosDetalles", commandType: CommandType.StoredProcedure);
                return productos.ToList();
            }
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
        public int RegistrarPedido(RegistrarPedidoDTO ent)
        {
            using (var connection = new SqlConnection(_connection))
            {
                var idPedido = connection.QuerySingleOrDefault<int>("InsertarPedido", new
                {
                    ent.idEmpleado,
                    ent.fechaEncargo,
                    ent.Estado,
                    ent.montoTotalPedido
                }, commandType: System.Data.CommandType.StoredProcedure);

                return idPedido;
            }
        }


        public void RegistrarPedidoDetalle(RegistrarPedidoDTO ent)
        {
            using (var connection = new SqlConnection(_connection))
            {
                var idPedido = connection.Query("InsertarPedidoDetalle", new
                {
                    ent.idPedido,
                    ent.idProducto,
                    ent.idProveedor,
                    ent.montoUnitario,
                    ent.montoTotalProducto,
                    ent.cantidad
                }, commandType: System.Data.CommandType.StoredProcedure);

           
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
