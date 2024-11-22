using Models.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using DTO.Archivo;
using DTO.Tarea;

namespace Data.Repositories
{
    public class ArchivoRepository
    {
        private readonly string _connectionString;

        public ArchivoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int codError;
        public string desError;

        //---------------------------------------------------Listar Archivos---------------------------------------------------
        public async Task<List<ArchivoDTO>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerArchivos", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<ArchivoDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToArchivoDTO(reader));
                        }
                    }

                    return response;
                }
            }
        }
        //---------------------------------------------------Añadir Archivo---------------------------------------------------
        public async Task<(int codErr, string desErr)> AñadirArchivo(ArchivoInsertDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarArchivoConGrupo", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Convertir el contenido Base64 a byte[]
                    byte[] contenidoBytes = Convert.FromBase64String(value.ContenidoBase64);

                    cmd.Parameters.Add(new SqlParameter("@grupo_tarea_id", value.Grupo_Tarea_Id));
                    cmd.Parameters.Add(new SqlParameter("@nombre_archivo", value.Nombre_Archivo));
                    cmd.Parameters.Add(new SqlParameter("@ruta_archivo", value.Ruta_Archivo));
                    cmd.Parameters.Add(new SqlParameter("@tipo_imagen", value.Tipo_Imagen));
                    cmd.Parameters.Add(new SqlParameter("@contenido", contenidoBytes));

                    cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.VarChar, 200)).Direction = ParameterDirection.Output;

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    desError = cmd.Parameters["@des_err"].Value.ToString();

                    return (codError, desError);
                }
            }
        }


        //---------------------------------------------------Eliminar Archivo---------------------------------------------------
        public async Task<(int codErr, string desErr)> EliminarArchivo(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_EliminarArchivo", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.VarChar, 100)).Direction = ParameterDirection.Output;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    desError = (cmd.Parameters["@des_err"].Value).ToString();

                    return (codError, desError);
                }
            }
        }

        public async Task<(int codErr, string desErr)> ActualizarArchivo(int id, ArchivoUpdateDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarArchivo", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Pasar los parámetros al procedimiento almacenado
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@NOMBRE_ARCHIVO", value.Nombre_Archivo));
                    cmd.Parameters.Add(new SqlParameter("@RUTA_ARCHIVO", value.Ruta_Archivo));
                    cmd.Parameters.Add(new SqlParameter("@TIPO_IMAGEN", value.Tipo_Imagen));

                    // Manejo de errores (parámetros de salida)
                    cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.VarChar, 200)).Direction = ParameterDirection.Output;

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Retorno de los valores de salida
                    int codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    string desError = cmd.Parameters["@des_err"].Value.ToString();

                    return (codError, desError);
                }
            }
        }


        //para obtener la foto
        public async Task<string> ObtenerArchivoBase64(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerArchivoBase64", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ArchivoID", id);

                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Convertir el contenido en byte[] de vuelta a Base64
                            byte[] contenidoBytes = (byte[])reader["contenido"];
                            return Convert.ToBase64String(contenidoBytes);
                        }
                    }
                }
            }

            return null; // Retornar null si no se encuentra el archivo
        }




        //obtener archivos por grupo de tareas
        public async Task<List<string>> ObtenerArchivosPorGrupoTarea(int grupoTareaId)
        {
            var archivosBase64 = new List<string>();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerArchivosPorGrupoTarea", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GrupoTareaID", grupoTareaId);

                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            byte[] contenidoBytes = (byte[])reader["contenido"];
                            archivosBase64.Add(Convert.ToBase64String(contenidoBytes));
                        }
                    }
                }
            }

            return archivosBase64;
        }



        //---------------------------------------------------Mapeo de Archivo---------------------------------------------------
        private ArchivoDTO MapToArchivoDTO(SqlDataReader reader)
        {
            return new ArchivoDTO()
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                Nombre_Archivo = reader.GetString(reader.GetOrdinal("NOMBRE_ARCHIVO")),
                Ruta_Archivo = reader.GetString(reader.GetOrdinal("RUTA_ARCHIVO")),
                Tipo_Imagen = reader.GetString(reader.GetOrdinal("TIPO_IMAGEN")),
                Fecha_Creacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }
    }
}

