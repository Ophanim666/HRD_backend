using Models.Entidades;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
//Importamos el dto
using DTO.TipoParametro;

namespace Data.Repositories
{
    public class TipoParametroRepository
    {
        private readonly string _connectionString;

        public TipoParametroRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        //aqui ponemos los codErr y desErr para poder trabajarlos
        public int codError;
        public string desError; 

        //---------------------------------------------------------------Eliminar TipoParametro por ID---------------------------------------------------------------
        public async Task<(int codErr, string desErr)> EliminarTipoParametro(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_EliminarTipoParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    //agregamos nuestro manejo de errores
                    cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.VarChar, 100)).Direction = ParameterDirection.Output;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    //lo que debemos retornar
                    codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    desError = (cmd.Parameters["@des_err"].Value).ToString();

                    return (codError, desError);
                }
            }
        }


        //---------------------------------------------------------------Insertar TipoParametro--------------------------------------------------------------- 
        public async Task<(int codErr, string desErr)> InsertarTipoParametro(TipoParametroInsertDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarTipoParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TIPO_PARAMETRO", value.TIPO_PARAMETRO);
                    cmd.Parameters.AddWithValue("@ESTADO", value.ESTADO);
                    //se deshabilitaron en la bd
                    //cmd.Parameters.AddWithValue("@FECHA_CREACION", value.FECHA_CREACION);
                    //cmd.Parameters.AddWithValue("@USUARIO_CREACION", value.USUARIO_CREACION);

                    //agregamos nuestro manejo de errores
                    cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.VarChar, 100)).Direction = ParameterDirection.Output;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    //lo que debemos retornar
                    codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    desError = (cmd.Parameters["@des_err"].Value).ToString();

                    return (codError, desError);
                }
            }
        }

        //---------------------------------------------------------------Actualizar TipoParametro---------------------------------------------------------------
        public async Task<(int codErr, string desErr)> ActualizarTipoParametro(int id, TipoParametroUpdateDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                // AQUI SE CAMBIO EL PROCEDIMIENTO ALMACENADO YA QUE ESTE ACTUALIZA Y NO DA ERROR SI EL TIPOPARAMETRO NO SE ALTERA
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarTipoParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@TIPO_PARAMETRO", value.TIPO_PARAMETRO));
                    cmd.Parameters.Add(new SqlParameter("@ESTADO", value.ESTADO));

                    //agregamos nuestro manejo de errores
                    cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.VarChar, 100)).Direction = ParameterDirection.Output;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    //lo que debemos retornar
                    codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    desError = (cmd.Parameters["@des_err"].Value).ToString();

                    return (codError, desError);
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

//...........................................................MAPEO (recorddar cambios donde se dejan pasar datos nulos)....................................................

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
                USUARIO_CREACION = reader.IsDBNull(reader.GetOrdinal("USUARIO_CREACION")) ? null : reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }
    }
}
