using Models.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using DTO.Especialidad;

namespace Data.Repositories
{
    public class EspecialidadRepository
    {
        private readonly string _connectionString;

        public EspecialidadRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //aqui ponemos los codErr y desErr para poder trabajarlos
        public int codError;
        public string desError;

        //---------------------------------------------------Función para listar---------------------------------------------------
        public async Task<List<EspecialidadDTO>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_ObtenerEspecialidad", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<EspecialidadDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToEspecialidadDTO(reader));
                        }
                    }

                    return response;
                }
            }
        }

        //---------------------------------------------------Listar especialidades simple (nombre, id) para listarlos y asignar a proveedores---------------------------------------------------
        //preguntar al profe si es mejor utilizar DTO para listar o cual es la mejor practica
        public async Task<List<LstEspecialidadDTO>> LstEspecialidad()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_LstEspecialidades", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<LstEspecialidadDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToEspecialidadSimp(reader));
                        }
                    }

                    return response;
                }
            }
        }

        //---------------------------------------------------Función para añadir---------------------------------------------------
        public async Task<(int codErr, string desErr)> AñadirEspecialidad(EspecialidadInsertDTO value, string usuarioCreacion)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_InsertarEspecialidad", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@NOMBRE", value.NOMBRE));
                    cmd.Parameters.Add(new SqlParameter("@ESTADO", value.ESTADO));
                    // se comenta por cambios de procediemitno almacenado ya no recibe estso campos
                    cmd.Parameters.Add(new SqlParameter("@USUARIO_CREACION", usuarioCreacion)); // Asignamos el usuario que está creando
                    //cmd.Parameters.AddWithValue("@FECHA_CREACION", especialidad.FECHA_CREACION);

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

        //---------------------------------------------------Eliminar especialidad---------------------------------------------------
        public async Task<(int codErr, string desErr)> EliminarEspecialidad(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_EliminarEspecialidad", sql))
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

        //---------------------------------------------------Actualizar especialidad---------------------------------------------------
        public async Task<(int codErr, string desErr)> ActualizarEspecialidad(int id, EspecialidadUpdateDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_ActualizarEspecialidad", sql))
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



        //---------------------------------------------------Mapeo de la especialidad (recordar sacar lo de vaores nulos)---------------------------------------------------
        private EspecialidadDTO MapToEspecialidadDTO(SqlDataReader reader)
        {
            return new EspecialidadDTO()
            {
                //OJO el mapeo solo sirve para listar si son nulos si se quiere insertar un dato nulo eso se debe ver en otra situacion, lo que hara la fecha creacion en esta situcaion 
                //seria poner la fecha actual en el listado para que no quede nulo y el listado si funcione
                // SE MODDIFICO PARA QUE PUEDA ACEPTAR NULOS
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                NOMBRE = reader.IsDBNull(reader.GetOrdinal("NOMBRE")) ? null : reader.GetString(reader.GetOrdinal("NOMBRE")),
                // DE MOENTO ESTADO NO PUEDE QUEDAR SIENDO NULO POR SER UN INT ------> nota: Tipos Nullable: En caso de tipos de datos como int, DateTime, etc., que no pueden ser nulos de forma predeterminada, los manejamos utilizando tipos nullable (por ejemplo, int?, DateTime?), que permiten asignar null.
                ESTADO = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                //ESTOS OBLIGATOEIMENTE DEBEN QUEDAR COMO NULOS (IsDBNull confirma que esta nullo en la d) 
                USUARIO_CREACION = reader.IsDBNull(reader.GetOrdinal("USUARIO_CREACION")) ? null : reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                //implementacion de DateTime.Now como este valor no uede ser nulo se entregara la fecha actual como solucion a null
                //FECHA_CREACION utiliza la fecha y hora actuales en caso de que el valor de la base de datos sea nulo, se asignara DateTime.Now cuando se detecte un valor nulo.
                FECHA_CREACION = reader.IsDBNull(reader.GetOrdinal("FECHA_CREACION")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }

        //se utilizo dto en vez del modelo, preguntar al profesor que es mejor para hacer el mapeo dto o el modelo
        private LstEspecialidadDTO MapToEspecialidadSimp(SqlDataReader reader)
        {
            return new LstEspecialidadDTO()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                NOMBRE = reader.IsDBNull(reader.GetOrdinal("NOMBRE")) ? null : reader.GetString(reader.GetOrdinal("NOMBRE")),
            };
        }

    }
}
