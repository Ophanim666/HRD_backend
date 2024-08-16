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

        // Funci�n para listar
        public async Task<List<Especialidad>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_ListarEspecialidad", sql))
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

        // Funci�n para a�adir
        public async Task<int> A�adirEspecialidad(Especialidad especialidad)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_A�adirEspecialidad", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NOMBRE", especialidad.NOMBRE);
                    cmd.Parameters.AddWithValue("@ESTADO", especialidad.ESTADO);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", especialidad.USUARIO_CREACION);
                    cmd.Parameters.AddWithValue("@FECHA_CREACION", especialidad.FECHA_CREACION);

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
        private Especialidad MapToEspecialidad(SqlDataReader reader)
        {
            return new Especialidad()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                NOMBRE = reader.GetString(reader.GetOrdinal("NOMBRE")),
                ESTADO = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                USUARIO_CREACION = reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }
    }
}
