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
        //public async Task<int> InsertarTipoParametro(TipoParametro tipoParametro)
        //{
        //    //if (await TipoParametroExists(tipoParametro.TIPO_PARAMETRO))
        //    //{
        //    //    throw new Exception("El tipo de parámetro ya existe.");
        //    //}

        //    using (SqlConnection sql = new SqlConnection(_connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("InsertarTipoParametro", sql))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@TIPO_PARAMETRO", tipoParametro.TIPO_PARAMETRO);
        //            cmd.Parameters.AddWithValue("@USUARIO_CREACION", tipoParametro.USUARIO_CREACION);
        //            cmd.Parameters.AddWithValue("@ESTADO", tipoParametro.ESTADO);
        //            cmd.Parameters.AddWithValue("@FECHA_CREACION", tipoParametro.FECHA_CREACION);

        //            await sql.OpenAsync();
        //            return await cmd.ExecuteNonQueryAsync();
        //        }
        //    }
        //}

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
