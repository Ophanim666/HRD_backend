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

        //---------------------------------------------------------------Eliminar TipoParametro por ID---------------------------------------------------------------
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


        //---------------------------------------------------------------Insertar TipoParametro--------------------------------------------------------------- 
        public async Task<int> InsertarTipoParametro(TipoParametro tipoParametro)
        {
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

        //---------------------------------------------------------------Actualizar TipoParametro---------------------------------------------------------------
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

        //---------------------------------------------------------------Función para listar---------------------------------------------------------------
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
                ////OJO el mapeo solo sirve para listar si son nulos si se quiere insertar un dato nulo eso se debe ver en otra situacion 
                //ID = reader.GetInt32(reader.GetOrdinal("ID")),
                ////esto se hace para que se puedan aceptar valores nulos
                //TIPO_PARAMETRO = reader.IsDBNull(reader.GetOrdinal("TIPO_PARAMETRO")) ? null : reader.GetString(reader.GetOrdinal("TIPO_PARAMETRO")),
                //ESTADO = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                ////para los valores nulos se hace el mismo procedimiento
                //USUARIO_CREACION = reader.IsDBNull(reader.GetOrdinal("USUARIO_CREACION")) ? null : reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                ////aqui pondra la fecha actual ya que datetime no puede qudar nulo
                //FECHA_CREACION = reader.IsDBNull(reader.GetOrdinal("FECHA_CREACION")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))

                //En caso de errores se veia asi antes de la aceptacion de los valores nulos
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                TIPO_PARAMETRO = reader.GetString(reader.GetOrdinal("TIPO_PARAMETRO")),
                ESTADO = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                USUARIO_CREACION = reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }
    }
}
