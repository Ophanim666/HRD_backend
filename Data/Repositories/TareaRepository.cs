using Models.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Data.Repositories
{
    public class TareaRepository
    {
        private readonly string _connectionString;

        public TareaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para eliminar Tarea por ID
        public async Task<int> EliminarTarea(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarTarea", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Insertar Tarea
        public async Task<int> InsertarTarea(Tarea tarea)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarTarea", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NOMBRE", tarea.Nombre);
                    cmd.Parameters.AddWithValue("@CODIGO", tarea.Codigo);
                    cmd.Parameters.AddWithValue("@ESTADO", tarea.Estado);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", tarea.Usuario_Creacion);
                    cmd.Parameters.AddWithValue("@FECHA_CREACION", tarea.Fecha_Creacion);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Actualizar Tarea
        public async Task<int> ActualizarTarea(Tarea tarea)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarTarea", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", tarea.ID);
                    cmd.Parameters.AddWithValue("@NOMBRE", tarea.Nombre);
                    cmd.Parameters.AddWithValue("@CODIGO", tarea.Codigo);
                    cmd.Parameters.AddWithValue("@ESTADO", tarea.Estado);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", tarea.Usuario_Creacion);
                    cmd.Parameters.AddWithValue("@FECHA_CREACION", tarea.Fecha_Creacion);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Función para listar todas las tareas
        public async Task<List<Tarea>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ListarTareas", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<Tarea>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToTarea(reader));
                        }
                    }

                    return response;
                }
            }
        }

        // Listar Tarea por ID
        public async Task<Tarea> ListarPorId(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ListarTareaPorId", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    Tarea response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            response = MapToTarea(reader);
                        }
                    }

                    return response;
                }
            }
        }

        // Mapeo de datos desde el DataReader a la entidad Tarea
        private Tarea MapToTarea(SqlDataReader reader)
        {
            return new Tarea()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                Nombre = reader.GetString(reader.GetOrdinal("NOMBRE")),
                Codigo = reader.GetString(reader.GetOrdinal("CODIGO")),
                Estado = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                Usuario_Creacion = reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                Fecha_Creacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }
    }
}
