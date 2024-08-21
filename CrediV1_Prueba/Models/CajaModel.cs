using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using X.PagedList;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrediV1_Prueba.Models
{
    public class CajaModel: ICajaModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _connection;

        public CajaModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("Connection");
        }


        public async Task<IPagedList<CajaEnt>> ListarCajasDiarias(int paginas, int tamaño)
        {
            using (var connection = new SqlConnection(_connection))
            {
                var salidas = await connection.QueryAsync<CajaEnt>("ListarControlCaja", commandType: CommandType.StoredProcedure);
                var pagedSalidas = salidas.ToPagedList(paginas, tamaño);

                return (IPagedList<CajaEnt>)pagedSalidas;
            }
        }

        public async Task<IPagedList<CajaEnt>> ListarGastos(int paginas, int tamaño, DateTime fecha)
        {
            using (var connection = new SqlConnection(_connection))
            {



                var gastos = await connection.QueryAsync<CajaEnt>("VerGastos", new { fecha = fecha }, commandType: CommandType.StoredProcedure);
                var pagedSalidas = gastos.ToPagedList(paginas, tamaño);

                return (IPagedList<CajaEnt>)pagedSalidas;
            }
        }

        public long RegistrarGasto(CajaEnt Gasto)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@fecha", Gasto.fechaID);
                    parameters.Add("@monto", Gasto.montoGastos);
                    parameters.Add("@descripcion", Gasto.descripcion);
                    parameters.Add("@idGasto", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    con.Execute("RegistrarGasto", parameters, commandType: CommandType.StoredProcedure);

                    var idGasto = parameters.Get<long>("@idGasto");
                    return idGasto;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int EliminarGasto (CajaEnt datos)
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idGasto", datos.idGasto);

                    con.Execute("EliminarGasto", parameters, commandType: CommandType.StoredProcedure);

                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public CajaEnt ObtenerDatosCaja()
        {
            try
            {
                using (var con = new SqlConnection(_connection))
                {

                    var gastos = con.Query<CajaEnt>("ObtenerDatosCaja", new { }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return gastos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
}
    }
}
