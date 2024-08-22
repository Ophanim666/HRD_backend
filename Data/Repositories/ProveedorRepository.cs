using Models.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Data.Repositories
{
    public class ProveedorRepository
    {
        private readonly string _connectionString;

        public ProveedorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        // Método para eliminar proveedores por ID
        public async Task<int> EliminarProveedor(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarProveedor", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        // Insertar proveedor con validación
        public async Task<int> InsertarProveedor(Proveedor proveedor)
        {
       

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarProveedor", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                   // cmd.Parameters.AddWithValue("@ID", proveedor.ID);
                    cmd.Parameters.AddWithValue("@NOMBRE", proveedor.NOMBRE);
                    cmd.Parameters.AddWithValue("@RAZON_SOCIAL", proveedor.RAZON_SOCIAL);
                    cmd.Parameters.AddWithValue("@RUT", proveedor.RUT);
                    cmd.Parameters.AddWithValue("@DV", proveedor.DV);
                    cmd.Parameters.AddWithValue("@NOMBRE_CONTACTO_PRINCIPAL", proveedor.NOMBRE_CONTACTO_PRINCIPAL);
                    cmd.Parameters.AddWithValue("@NUMERO_CONTACTO_PRINCIPAL", proveedor.NUMERO_CONTACTO_PRINCIPAL);
                    cmd.Parameters.AddWithValue("@NOMBRE_CONTACTO_SECUNDARIO", proveedor.NOMBRE_CONTACTO_SECUNDARIO);
                    cmd.Parameters.AddWithValue("@NUMERO_CONTACTO_SECUNDARIO", proveedor.NUMERO_CONTACTO_SECUNDARIO);
                    cmd.Parameters.AddWithValue("@ESTADO", proveedor.ESTADO);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", proveedor.USUARIO_CREACION);
                    cmd.Parameters.AddWithValue("@FECHA_CREACION", proveedor.FECHA_CREACION);


                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Actualizar TipoParametro con validación
        public async Task<int> ActualizarProveedor(Proveedor proveedor)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarProveedor", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", proveedor.ID);
                    cmd.Parameters.AddWithValue("@NOMBRE", proveedor.NOMBRE);
                    cmd.Parameters.AddWithValue("@RAZON_SOCIAL", proveedor.RAZON_SOCIAL);
                    cmd.Parameters.AddWithValue("@RUT", proveedor.RUT);
                    cmd.Parameters.AddWithValue("@DV", proveedor.DV);
                    cmd.Parameters.AddWithValue("@NOMBRE_CONTACTO_PRINCIPAL", proveedor.NOMBRE_CONTACTO_PRINCIPAL);
                    cmd.Parameters.AddWithValue("@NUMERO_CONTACTO_PRINCIPAL", proveedor.NUMERO_CONTACTO_PRINCIPAL);
                    cmd.Parameters.AddWithValue("@NOMBRE_CONTACTO_SECUNDARIO", proveedor.NOMBRE_CONTACTO_SECUNDARIO);
                    cmd.Parameters.AddWithValue("@NUMERO_CONTACTO_SECUNDARIO", proveedor.NUMERO_CONTACTO_SECUNDARIO);
                    cmd.Parameters.AddWithValue("@ESTADO", proveedor.ESTADO);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", proveedor.USUARIO_CREACION);
                    cmd.Parameters.AddWithValue("@FECHA_CREACION", proveedor.FECHA_CREACION);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Función para listar
        public async Task<List<Proveedor>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ListarProveedor", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<Proveedor>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToTipoParametro(reader));
                        }
                    }

                    return response;
                }
            }
        }
        //Listar por ID -------------------------------------------------------ELIMINAR SI NO FUNCIONA---------------------------------
        public async Task<Proveedor> ListarPorIdProveedor(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ListarProveedorPorId", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    Proveedor response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            response = MapToTipoParametro(reader);
                        }
                    }

                    return response;
                }
            }
        }
        //...........................................................MAPEO....................................................

        private Proveedor MapToTipoParametro(SqlDataReader reader)
        {
            return new Proveedor()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                NOMBRE = reader.GetString(reader.GetOrdinal("NOMBRE")),
                RAZON_SOCIAL = reader.GetString(reader.GetOrdinal("RAZON_SOCIAL")),
                RUT = reader.GetString(reader.GetOrdinal("RUT")),
                DV = reader.GetString(reader.GetOrdinal("DV")),
                NOMBRE_CONTACTO_PRINCIPAL = reader.GetString(reader.GetOrdinal("NOMBRE_CONTACTO_PRINCIPAL")),
                NUMERO_CONTACTO_PRINCIPAL = reader.GetInt32(reader.GetOrdinal("NUMERO_CONTACTO_PRINCIPAL")),
                NOMBRE_CONTACTO_SECUNDARIO = reader.GetString(reader.GetOrdinal("NOMBRE_CONTACTO_SECUNDARIO")),
                NUMERO_CONTACTO_SECUNDARIO = reader.GetInt32(reader.GetOrdinal("NUMERO_CONTACTO_SECUNDARIO")),
                ESTADO = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                USUARIO_CREACION = reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))


            };
        }
    }
}