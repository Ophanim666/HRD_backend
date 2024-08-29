using Models.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Data.Repositories
{
    public class TipoParametroRepository
    {
        private readonly string _connectionString;

        public TipoParametroRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        // Método para eliminar TipoParametro por ID
        public async Task<int> EliminarTipoParametro(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_EliminarTipoParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        // Insertar TipoParametro con validación
        public async Task<int> InsertarTipoParametro(TipoParametro tipoParametro)
        {
            //if (await TipoParametroExists(tipoParametro.TIPO_PARAMETRO))
            //{
            //    throw new Exception("El tipo de parámetro ya existe.");
            //}

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarTipoParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TIPO_PARAMETRO", tipoParametro.TIPO_PARAMETRO);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", tipoParametro.USUARIO_CREACION);
                    cmd.Parameters.AddWithValue("@ESTADO", tipoParametro.ESTADO);
                    cmd.Parameters.AddWithValue("@FECHA_CREACION", tipoParametro.FECHA_CREACION);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Actualizar TipoParametro con validación
        public async Task<int> ActualizarTipoParametro(TipoParametro tipoParametro)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                // AQUI SE CAMBIO EL PROCEDIMIENTO ALMACENADO YA QUE ESTE ACTUALIZA Y NO DA ERROR SI EL TIPOPARAMETRO NO SE ALTERA
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarTipoParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", tipoParametro.ID);
                    cmd.Parameters.AddWithValue("@TIPO_PARAMETRO", tipoParametro.TIPO_PARAMETRO);
                    cmd.Parameters.AddWithValue("@ESTADO", tipoParametro.ESTADO);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", tipoParametro.USUARIO_CREACION);
                    cmd.Parameters.AddWithValue("@FECHA_CREACION", tipoParametro.FECHA_CREACION);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Función para listar
        public async Task<List<TipoParametro>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerTipoParametros", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<TipoParametro>();
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

//...........................................................MAPEO....................................................

        private TipoParametro MapToTipoParametro(SqlDataReader reader)
        {
            return new TipoParametro()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                TIPO_PARAMETRO = reader.GetString(reader.GetOrdinal("TIPO_PARAMETRO")),
                ESTADO = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                USUARIO_CREACION = reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }
    }
}
