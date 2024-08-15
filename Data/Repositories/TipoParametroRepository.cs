using Models.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace Data.Repositories
{
    public class TipoParametroRepository
    {
        private readonly string _connectionString;

        public TipoParametroRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Función para insertar un nuevo TipoParametro
        public async Task<int> InsertarTipoParametro(TipoParametro tipoParametro)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarTipoParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Añadir parámetros al procedimiento almacenado
                    cmd.Parameters.AddWithValue("@TIPO_PARAMETRO", tipoParametro.TIPO_PARAMETRO);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", tipoParametro.USUARIO_CREACION);
                    cmd.Parameters.AddWithValue("@ESTADO", tipoParametro.ESTADO);
                    cmd.Parameters.AddWithValue("@FECHA_CREACION", tipoParametro.FECHA_CREACION);

                    await sql.OpenAsync();
                    // Ejecutar el comando y obtener el número de filas afectadas
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        //funcion para actualizar
        public async Task<int> ActualizarTipoParametro(TipoParametro tipoParametro)
        {
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

        //Funcion para listar
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

