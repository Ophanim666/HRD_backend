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

        // Verificar unicidad
        //public async Task<bool> TipoParametroExists(string tipoParametro)
        //{
        //    if (string.IsNullOrWhiteSpace(tipoParametro))
        //    {
        //        throw new ArgumentNullException(nameof(tipoParametro), "El tipo de parámetro no puede ser nulo o vacío.");
        //    }

        //    using (SqlConnection sql = new SqlConnection(_connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM TIPO_PARAMETRO2 WHERE TIPO_PARAMETRO_LOWER = @TipoParametro", sql))
        //        {
        //            cmd.Parameters.AddWithValue("@TipoParametro", tipoParametro.ToLower());
        //            await sql.OpenAsync();

        //            var count = (int)await cmd.ExecuteScalarAsync();
        //            return count > 0;
        //        }
        //    }
        //}


        // Método para eliminar TipoParametro por ID
        public async Task<int> EliminarTipoParametro(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarTipoParametro", sql))
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
                using (SqlCommand cmd = new SqlCommand("InsertarTipoParametro", sql))
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
            // Verificar la existencia del tipo de parámetro antes de actualizar (opcional, si se necesita)
            //if (await TipoParametroExists(tipoParametro.TIPO_PARAMETRO))
            //{
            //    throw new Exception("El tipo de parámetro ya existe.");
            //}

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarTipoParametro", sql))
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
                using (SqlCommand cmd = new SqlCommand("ListarTipoParametros", sql))
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
