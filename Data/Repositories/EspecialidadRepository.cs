using Models.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Data.Repositories
{
    public class EspecialidadRepository
    {
        private readonly string _connectionString;

        public EspecialidadRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Función para listar
        public async Task<List<Especialidad>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_ObtenerEspecialidad", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<Especialidad>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToEspecialidad(reader));
                        }
                    }

                    return response;
                }
            }
        }

        // Función para añadir
        public async Task<int> AñadirEspecialidad(Especialidad especialidad)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_AñadirEspecialidad", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NOMBRE", especialidad.NOMBRE);
                    cmd.Parameters.AddWithValue("@ESTADO", especialidad.ESTADO);
                    // se comenta por cambios de procediemitno almacenado ya no recibe estso campos
                    //cmd.Parameters.AddWithValue("@USUARIO_CREACION", especialidad.USUARIO_CREACION);
                    //cmd.Parameters.AddWithValue("@FECHA_CREACION", especialidad.FECHA_CREACION);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Procedimiento almacenado para eliminar especialidad
        public async Task<int> EliminarEspecialidad(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_EliminarEspecialidad", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        
        // Procedimiento almacenado para actualizar especialidad
        public async Task<int> ActualizarEspecialidad(Especialidad especialidad)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_ActualizarEspecialidad", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", especialidad.ID);
                    cmd.Parameters.AddWithValue("@NOMBRE", especialidad.NOMBRE);
                    cmd.Parameters.AddWithValue("@ESTADO", especialidad.ESTADO);
                    // se comenta por cambios de procediemitno almacenado ya no recibe estso campos
                    //cmd.Parameters.AddWithValue("@USUARIO_CREACION", especialidad.USUARIO_CREACION);
                    //cmd.Parameters.AddWithValue("@FECHA_CREACION", especialidad.FECHA_CREACION);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }



        //Mapeo de la especialidad
        private Especialidad MapToEspecialidad(SqlDataReader reader)
        {
            return new Especialidad()
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

    }
}
