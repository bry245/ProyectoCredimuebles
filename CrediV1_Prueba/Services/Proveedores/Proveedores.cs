using CrediV1_Prueba.Context;
using CrediV1_Prueba.Models;
using Dapper;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrediV1_Prueba.Services.Proveedores
{
    public class Proveedores : IProveedores
    {

        private readonly DapperContext _context;


        public Proveedores(DapperContext context) => _context = context;

        public async Task AddProveedores(Proveedor proveedor)
        {

            var query = "INSERT INTO Proveedor (nombre,telefono,correo,direccion,Estado) VALUES(@nombre,@telefono,@correo,@direccion,@Estado)"
                + "SELECT CAST(SCOPE_IDENTITY() AS int)";
            var parameters = new DynamicParameters();
            parameters.Add("nombre", proveedor.nombre, DbType.String);
            parameters.Add("telefono", proveedor.telefono, DbType.String);
            parameters.Add("correo", proveedor.correo, DbType.String);
            parameters.Add("direccion", proveedor.direccion, DbType.String);
            parameters.Add("Estado", true, DbType.Boolean);
            using (var connection = _context.CreateConnection())
            {

                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdProveedor = new Proveedor
                {
                    idProveedor = id,
                    nombre = proveedor.nombre,
                    telefono = proveedor.telefono,
                    correo = proveedor.correo,
                    direccion = proveedor.direccion,
                    Estado = proveedor.Estado


                };
              
            }

        }
		public async Task DesactivarProveedor(int idProveedor)
		{
			var query = "UPDATE Proveedor SET Estado = 0 WHERE idProveedor = @idProveedor";
			var parameters = new DynamicParameters();
			parameters.Add("idProveedor", idProveedor, DbType.Int32);

			using (var connection = _context.CreateConnection())
			{
				await connection.ExecuteAsync(query, parameters);
			}
		}
		public async Task<IEnumerable<Proveedor>> GetProveedores()
        {

            var query = "SELECT * FROM Proveedor";
            using (var connection = _context.CreateConnection())
            {
                var proveedores = await connection.QueryAsync<Proveedor>(query);
                return proveedores.ToList();
            }
               
        }

		public async Task<Proveedor> GetProveedoresID(int idProveedor)
		{
			var query = "SELECT * FROM Proveedor WHERE idProveedor = @idProveedor";
			using (var connection = _context.CreateConnection())
			{
				var parameters = new DynamicParameters();
				parameters.Add("idProveedor", idProveedor, DbType.Int32);

				var proveedor = await connection.QuerySingleOrDefaultAsync<Proveedor>(query, parameters);
				return proveedor;
			}
		}

		public async Task<IEnumerable<Proveedor>> GetProveedoresNombre(string nombre)
		{
			var query = "SELECT * FROM Proveedor WHERE nombre LIKE @nombre + '%'";
			using (var connection = _context.CreateConnection())
			{
				var parameters = new DynamicParameters();
				parameters.Add("{", nombre, DbType.String);

				var proveedor = await connection.QueryAsync<Proveedor>(query, parameters);
				return proveedor.ToList();
			} 
		}

        public async Task UpdateProveedor(Proveedor proveedor)
        {
            var query = "UPDATE Proveedor SET nombre = @nombre, telefono = @telefono, direccion = @direccion, correo = @correo, Estado = @Estado WHERE idProveedor = @idProveedor";
            var parameters = new DynamicParameters();
            parameters.Add("idProveedor", proveedor.idProveedor, DbType.Int32);
            parameters.Add("nombre", proveedor.nombre, DbType.String);
            parameters.Add("telefono", proveedor.telefono, DbType.String);
            parameters.Add("direccion", proveedor.direccion, DbType.String);
            parameters.Add("correo", proveedor.correo, DbType.String);
            parameters.Add("Estado", proveedor.Estado, DbType.Boolean); // Asegúrate de que Estado se maneje como booleano

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

    }
}
