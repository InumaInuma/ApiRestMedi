using ApiRestMedi.Context;
using ApiRestMedi.ManejoErrores;
using ApiRestMedi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ApiRestMedi.Repository
{
    public class Repositorio:IRepository
    {
        private readonly AppDbContext _context;

        public Repositorio(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> InsertarDatosAsync (Productos productos)
        {
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand()) {
                command.CommandText = "sp_insertar_producto";
                command.CommandType= CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@NombreProducto", productos.NombreProducto));
                command.Parameters.Add(new SqlParameter("@CodigoProducto", productos.CodigoProducto));
                command.Parameters.Add(new SqlParameter("@Precio", productos.Precio));
                command.Parameters.Add(new SqlParameter("@Estado", productos.Estado));
                command.Parameters.Add(new SqlParameter("@IdCategoria", productos.IdCategoria));
                var resultado = await command.ExecuteNonQueryAsync();
                return resultado > 0;
            }

        }

        public async Task<IEnumerable<Productos>> ListarProductosAsync()
        {
            var productosList = new List<Productos>();

            //// Suponiendo que tienes un procedimiento almacenado llamado "sp_ListarProductos"
            //var sql = "EXEC sp_ListarProductos"; // Reemplaza con el nombre de tu procedimiento
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "sp_listar_productos";
                    command.CommandType = System.Data.CommandType.Text; // O CommandType.StoredProcedure si es necesario

                    _context.Database.OpenConnection();

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (await result.ReadAsync())
                        {
                            var producto = new Productos
                            {
                                // Mapea tus columnas a las propiedades de la clase Productos
                                IdProducto = result.GetInt32(result.GetOrdinal("IdProducto")),
                                NombreProducto = result.GetString(result.GetOrdinal("NombreProducto")),
                                CodigoProducto = result.GetString(result.GetOrdinal("CodigoProducto")),
                                Precio = result.GetDecimal(result.GetOrdinal("Precio")),
                                Estado = result.GetInt32(result.GetOrdinal("Estado")),
                                IdCategoria = result.GetInt32(result.GetOrdinal("IdCategoria"))
                                // Agrega el resto de las propiedades según tu modelo
                            };
                            productosList.Add(producto);
                        }
                    }
                }
               
            }
            catch (SqlException ex)
            {

                throw new ProductoException("Error al listar productos desde la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new ProductoException("Error inesperado al listar productos.", ex);
            }
            _context.Database.CloseConnection();

            return productosList;

        }
        public async Task<IEnumerable<Productos>> BuscarProductosPorNombreAsync(string nombre)
        {
            var productosList = new List<Productos>();

            try
            {
                //var sql = "EXEC sp_buscar_productos_nombre @Nombre";
                var parameter = new SqlParameter("@Nombre", nombre);

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "sp_buscar_productos_nombre @Nombre";
                    command.CommandType = System.Data.CommandType.Text; // O CommandType.StoredProcedure si es necesario
                    command.Parameters.Add(parameter);

                    _context.Database.OpenConnection();

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (await result.ReadAsync())
                        {
                            var producto = new Productos
                            {
                                IdProducto = result.GetInt32(result.GetOrdinal("IdProducto")),
                                NombreProducto = result.GetString(result.GetOrdinal("NombreProducto")),
                                CodigoProducto = result.GetString(result.GetOrdinal("CodigoProducto")),
                                Precio = result.GetDecimal(result.GetOrdinal("Precio")),
                                Estado = result.GetInt32(result.GetOrdinal("Estado")),
                                IdCategoria = result.GetInt32(result.GetOrdinal("IdCategoria"))
                                // Mapea las demás propiedades
                            };
                            productosList.Add(producto);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ProductoException("Error al buscar productos por nombre en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new ProductoException("Error inesperado al buscar productos por nombre.", ex);
            }

            return productosList;
        }

        public async Task<bool> ActualizarProductoAsync(Productos productos)
        {
            try
            {
                var sql = "sp_actualizar_producto"; // Nombre del procedimiento
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.Add(new SqlParameter("@IdProducto", productos.IdProducto));
                    command.Parameters.Add(new SqlParameter("@NombreProducto", productos.NombreProducto));
                    command.Parameters.Add(new SqlParameter("@CodigoProducto", productos.CodigoProducto));
                    command.Parameters.Add(new SqlParameter("@Precio", productos.Precio));
                    command.Parameters.Add(new SqlParameter("@Estado", productos.Estado));
                    command.Parameters.Add(new SqlParameter("@IdCategoria", productos.IdCategoria));

                    // Agregar un parámetro de retorno
                    var returnParameter = new SqlParameter("@ReturnValue", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(returnParameter);

                    _context.Database.OpenConnection();
                    await command.ExecuteNonQueryAsync();

                    // Captura el valor de retorno
                    var returnValue = (int)returnParameter.Value;
                    return returnValue == 1; // Solo retorna verdadero si la actualización fue exitosa
                }
            }
            catch (SqlException ex)
            {
                throw new ProductoException("Error al actualizar el producto en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new ProductoException("Error inesperado al actualizar el producto.", ex);
            }
        }

        public async Task<bool> DardeBajaProducto(Productos productos)
        {
            try
            {
                var sql = "sp_dar_baja_producto"; // Nombre del procedimiento
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.Add(new SqlParameter("@IdProducto", productos.IdProducto));
                    

                    // Agregar un parámetro de retorno
                    var returnParameter = new SqlParameter("@ReturnValue", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(returnParameter);

                    _context.Database.OpenConnection();
                    await command.ExecuteNonQueryAsync();

                    // Captura el valor de retorno
                    var returnValue = (int)returnParameter.Value;
                    return returnValue == 1; // Solo retorna verdadero si la actualización fue exitosa
                }
            }
            catch (SqlException ex)
            {
                throw new ProductoException("Error al dar de baja al producto en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new ProductoException("Error inesperado al dar de baja al producto.", ex);
            }
        }

    }
}

