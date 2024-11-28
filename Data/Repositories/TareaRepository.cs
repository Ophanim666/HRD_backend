using Models.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using DTO.Tarea;

namespace Data.Repositories
{
    public class TareaRepository
    {
        private readonly string _connectionString;

        public TareaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //aqui ponemos los codErr y desErr para poder trabajarlos
        public int codError;
        public string desError;

        //---------------------------------------------------Función para listar---------------------------------------------------
        public async Task<List<TareaDTO>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerTareas", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<TareaDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToTareaDTO(reader));
                        }
                    }

                    return response;
                }
            }
        }

        //---------------------------------------------------Función para añadir---------------------------------------------------
        public async Task<(int codErr, string desErr)> AñadirTarea(TareaInsertDTO value, string usuarioCreacion)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarTarea", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@NOMBRE", value.NOMBRE));
                    cmd.Parameters.Add(new SqlParameter("@ESTADO", value.ESTADO));
                    cmd.Parameters.Add(new SqlParameter("@USUARIO_CREACION", usuarioCreacion)); // Asignamos el usuario que está creando


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

        //---------------------------------------------------Eliminar tarea---------------------------------------------------
        public async Task<(int codErr, string desErr)> EliminarTarea(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_EliminarTarea", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

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

        //---------------------------------------------------Actualizar tarea---------------------------------------------------
        public async Task<(int codErr, string desErr)> ActualizarTarea(int id, TareaUpdateDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarTarea", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@NOMBRE", value.NOMBRE));
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

        //---------------------------------------------------Mapeo de la tarea---------------------------------------------------
        private TareaDTO MapToTareaDTO(SqlDataReader reader)
        {
            return new TareaDTO()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                NOMBRE = reader.IsDBNull(reader.GetOrdinal("NOMBRE")) ? null : reader.GetString(reader.GetOrdinal("NOMBRE")),
                ESTADO = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                USUARIO_CREACION = reader.IsDBNull(reader.GetOrdinal("USUARIO_CREACION")) ? null : reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                FECHA_CREACION = reader.IsDBNull(reader.GetOrdinal("FECHA_CREACION")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }

        // Mapeo simple para listar nombre e id
        private LstTareaDTO MapToTareaSimp(SqlDataReader reader)
        {
            return new LstTareaDTO()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                NOMBRE = reader.IsDBNull(reader.GetOrdinal("NOMBRE")) ? null : reader.GetString(reader.GetOrdinal("NOMBRE")),
            };
        }

    }
}
