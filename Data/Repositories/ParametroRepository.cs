using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ParametroRepository
    {
        private readonly string _connectionString;

        public ParametroRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Insertar TipoParametro con validación
        public async Task<int> InsertarParametro(Parametro Parametro)
        {
            //if (await TipoParametroExists(tipoParametro.TIPO_PARAMETRO))
            //{
            //    throw new Exception("El tipo de parámetro ya existe.");
            //}

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PARAMETRO", Parametro.PARAMETRO);
                    cmd.Parameters.AddWithValue("@VALOR", Parametro.VALOR);
                    cmd.Parameters.AddWithValue("@ID_TIPO_PARAMETRO", Parametro.ID_TIPO_PARAMETRO);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", Parametro.USUARIO_CREACION);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Actualizar Parametro
        public async Task<int> ActualizarParametro(Parametro Parametro)
        {
            

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", Parametro.ID);
                    cmd.Parameters.AddWithValue("@PARAMETRO", Parametro.PARAMETRO);
                    cmd.Parameters.AddWithValue("@VALOR", Parametro.VALOR);
                    cmd.Parameters.AddWithValue("@ID_TIPO_PARAMETRO", Parametro.ID_TIPO_PARAMETRO);
                    cmd.Parameters.AddWithValue("@ESTADO", Parametro.ESTADO);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Función para listar
        public async Task<List<Parametro>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerParametros", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<Parametro>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToParametro(reader));
                        }
                    }

                    return response;
                }
            }
        }

        // eliminar Parametro
        public async Task<int> EliminarParametro(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    await sql.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private Parametro MapToParametro(SqlDataReader reader)
        {
            return new Parametro()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                PARAMETRO = reader.GetString(reader.GetOrdinal("PARAMETRO")),
                VALOR = reader.GetString(reader.GetOrdinal("VALOR")),
                ID_TIPO_PARAMETRO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_PARAMETRO")),
                ESTADO = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                USUARIO_CREACION = reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }


    }
}
