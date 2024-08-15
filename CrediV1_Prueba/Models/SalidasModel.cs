using CrediV1_Prueba.Entities;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using CrediV1_Prueba.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace CrediV1_Prueba.Models
{
    public class SalidasModel : ISalidasModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _connection;

        public SalidasModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("Connection");
        }

        public async Task<IPagedList<SalidasEnt>> ListarSalidas(int paginas, int tamaño)
        {
            using (var connection = new SqlConnection(_connection))
            {
                var salidas = await connection.QueryAsync<SalidasEnt>("ConsultarSalidas", commandType: CommandType.StoredProcedure);
                var pagedSalidas = salidas.ToPagedList(paginas, tamaño);

                return pagedSalidas;
            }
        }
        public async Task<IPagedList<SalidasEnt>> ConsultarSalidasOrdenadas(int paginas, int tamaño)
        {
            using (var connection = new SqlConnection(_connection))
            {
                var salidas = await connection.QueryAsync<SalidasEnt>("ConsultarSalidasOrdenadas", commandType: CommandType.StoredProcedure);
                var pagedSalidas = salidas.ToPagedList(paginas, tamaño);

                return pagedSalidas;
            }
        }

        public List<SelectListItem>? ConsultarVendedores()
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var dato = con.Query<SelectListItem>("ConsultarVendedores", new { },
                       commandType: CommandType.StoredProcedure).ToList();
                    return dato;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public List<SelectListItem>? ConsultarMetodosPago()
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var dato = con.Query<SelectListItem>("ConsultarMetodosPago", new { },
                       commandType: CommandType.StoredProcedure).ToList();
                    return dato;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ProductoEnt> ObtenerProductos()
        {
            List<ProductoEnt> productos = new List<ProductoEnt>();

            using (SqlConnection connection = new SqlConnection(_connection))
            {
                SqlCommand command = new SqlCommand("ConsultarProductosParaSalidas", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductoEnt producto = new ProductoEnt();
                    producto.idProducto = (long)reader["idProducto"];
                    producto.nombre = (string)reader["nombre"];
                    producto.costo = (decimal)reader["costo"];
                    productos.Add(producto);
                }

                reader.Close();
            }

            return productos;
        }

        public List<ProductoEnt> ObtenerProductoDeSalida(long idsalida)
        {
            List<ProductoEnt> productos = new List<ProductoEnt>();

            using (SqlConnection connection = new SqlConnection(_connection))
            {
                SqlCommand command = new SqlCommand("ConsultarProductosDeSalida", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@idsalida", SqlDbType.BigInt)).Value = idsalida;


                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductoEnt producto = new ProductoEnt();
                    producto.idProducto = (long)reader["idProducto"];
                    producto.nombre = (string)reader["nombre"];
                    producto.costo = (decimal)reader["costo"];
                    producto.cantidadSalida = (int)reader["cantidad"];
                    productos.Add(producto);
                }

                reader.Close();
            }

            return productos;
        }

        public long RegistrarSalida(SalidasEnt salida)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idMetodoPago", salida.idMetodoPago);
                    parameters.Add("@MontoDeVenta", salida.MontoDeVenta);
                    parameters.Add("@idVendedor", salida.idVendedor);
                    parameters.Add("@fecha", salida.fecha);
                    parameters.Add("@cedulaCliente", salida.cedulaCliente);
                    parameters.Add("@nombreCliente", salida.nombreCliente);
                    parameters.Add("@apellidosCliente", salida.apellidosCliente);
                    parameters.Add("@direccionCliente", salida.direccionCliente);
                    parameters.Add("@telefonoCliente", salida.telefonoCliente);
                    parameters.Add("@numeroFactura", salida.numeroFactura);
                    parameters.Add("@SalidaID", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    con.Execute("RegistrarSalida", parameters, commandType: CommandType.StoredProcedure);

                    var idSalida = parameters.Get<long>("@SalidaID");
                    return idSalida;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public long ActualizarSalida(SalidasEnt salida)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idsalida", salida.idSalida);
                    parameters.Add("@cedulaCliente", salida.cedulaCliente);
                    parameters.Add("@nombreCliente", salida.nombreCliente);
                    parameters.Add("@apellidosCliente", salida.apellidosCliente);
                    parameters.Add("@telefonoCliente", salida.telefonoCliente);
                    parameters.Add("@direccionCliente", salida.direccionCliente);
                    parameters.Add("@numeroFactura", salida.numeroFactura);

                    parameters.Add("@idMetodoPago", salida.idMetodoPago);
                    parameters.Add("@idVendedor", salida.idVendedor);
                    parameters.Add("@fecha", salida.fecha);
                    parameters.Add("@MontoDeVenta", salida.MontoDeVenta);

                    con.Execute("ActualizarSalida", parameters, commandType: CommandType.StoredProcedure);

                    return salida.idSalida;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int ActualizarInventarioEditarSalida(ProductoEnt producto)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idproducto", producto.idProducto);
                    parameters.Add("@cantidadSalida", producto.cantidadSalida);

                    con.Execute("ActualizarInventarioEditarSalida", parameters, commandType: CommandType.StoredProcedure);

                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<bool> RegistrarProductosSalida(ProductoEnt producto)
        {
            {
                try
                {
                    var parametro = new DynamicParameters();
                    parametro.Add("@idSalida", producto.idSalida, DbType.Int64);
                    parametro.Add("@idProducto", producto.idProducto, DbType.Int64);
                    parametro.Add("@cantidadSalida", producto.cantidadSalida, DbType.Int32);
                    //parameters.Add("@costo", producto.costo, DbType.Decimal);


                    using (var connection = new SqlConnection(_connection))
                    {
                        await connection.ExecuteAsync("RegistrarProductosSalida", parametro, commandType: CommandType.StoredProcedure);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al registrar producto de salida.", ex);
                }
            }

        }
        public async Task<bool> ActualizarProductosSalida(ProductoEnt producto)
        {
            {
                try
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@idSalida", producto.idSalida, DbType.Int64);
                    parametros.Add("@idProducto", producto.idProducto, DbType.Int64);
                    parametros.Add("@cantidadSalida", producto.cantidadSalida, DbType.Int32);
                    //parameters.Add("@costo", producto.costo, DbType.Decimal);


                    using (var connection = new SqlConnection(_connection))
                    {
                        await connection.ExecuteAsync("ActualizaProductosSalida", parametros, commandType: CommandType.StoredProcedure);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al Actualzar los productos de salida.", ex);
                }
            }

        }
        public SalidasEnt VerSalida(long idSalida)
        {
            try
            {
                using (var connection = new SqlConnection(_connection))
                {
                    var salida = connection.Query<SalidasEnt>("VerSalida", new { idSalida },
                       commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return salida;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al registrar producto de salida.", ex);
            }
        }

        public int AnularSalida(long idsalida)
        {


            try
            {
                using (var connection = new SqlConnection(_connection))
                {
                    var salida = connection.Execute("AnularSalida", new { idsalida },
                       commandType: CommandType.StoredProcedure);
                    return salida;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al anular la salida.", ex);
            }

        }


        //Cuentas por cobrar
        public long RegistrarCuentaCredito(SalidasEnt salida)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idMetodoPago", salida.idMetodoPago);
                    parameters.Add("@MontoVenta", salida.MontoDeVenta);
                    parameters.Add("@idVendedor", salida.idVendedor);
                    parameters.Add("@fechaCompra", salida.fecha);
                    parameters.Add("@cedulaCliente", salida.cedulaCliente);
                    parameters.Add("@nombreCliente", salida.nombreCliente);
                    parameters.Add("@apellidosCliente", salida.apellidosCliente);
                    parameters.Add("@direccionCliente", salida.direccionCliente);
                    parameters.Add("@telefonoCliente", salida.telefonoCliente);
                    parameters.Add("@numeroFactura", salida.numeroFactura);
                    parameters.Add("@prima", salida.prima);
                    parameters.Add("@plazo", salida.plazo);
                    parameters.Add("@SalidaID", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    con.Execute("RegistrarCuentaCredito", parameters, commandType: CommandType.StoredProcedure);

                    var idSalida = parameters.Get<long>("@SalidaID");
                    return idSalida;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        public async Task<IPagedList<SalidasEnt>> ListarCuentasPorCobrar(int paginas, int tamaño)
        {
            using (var connection = new SqlConnection(_connection))
            {
                var salidas = await connection.QueryAsync<SalidasEnt>("ListarCuentasPorCobrar", commandType: CommandType.StoredProcedure);
                var pagedSalidas = salidas.ToPagedList(paginas, tamaño);

                return pagedSalidas;
            }
        }

        public SalidasEnt VerCuentaPorCobrar(long idCuenta)
        {
            try
            {
                using (var connection = new SqlConnection(_connection))
                {
                    var salida = connection.Query<SalidasEnt>("VerCuentaPorCobrar", new { idCuenta },
                       commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return salida;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al consultar la información", ex);
            }
        }

        public List<SalidasEnt> ObtenerPagosRealizados(long idCuenta)
        {
            List<SalidasEnt> productos = new List<SalidasEnt>();

            using (SqlConnection connection = new SqlConnection(_connection))
            {
                SqlCommand command = new SqlCommand("ConsultarPagosACuenta", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@idCuenta", SqlDbType.BigInt)).Value = idCuenta;


                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SalidasEnt producto = new SalidasEnt();
                    producto.idLinea = (long)reader["idLinea"];
                    producto.idCuenta = (long)reader["idCuenta"];
                    producto.fechaAbono = (DateTime)reader["fechaPago"];
                    producto.MontoDeVenta = (decimal)reader["montoVenta"];
                    producto.abono = (decimal)reader["montoPago"];
                    producto.numeroFactura = (string)reader["numeroFactura"];
                    productos.Add(producto);
                }

                reader.Close();
            }

            return productos;
        }

        public int AgregarAbono(SalidasEnt cuenta)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idCuenta", cuenta.idCuenta);
                    parameters.Add("@abono", cuenta.abono);
                    parameters.Add("@numeroFactura", cuenta.numeroFactura);

                    con.Execute("AgregarAbono", parameters, commandType: CommandType.StoredProcedure);

                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}