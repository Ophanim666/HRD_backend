using DTO.Acta;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using System.Diagnostics;

namespace Data.Repositories
{
    public class ActaRepository
    {
        private readonly string _connectionString;

        public ActaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //aqui ponemos los codErr y desErr para poder trabajarlos
        public int codError;
        public string desError;

        //---------------------------------------------------------------Listar parametro----------------------------------------------------------------- 
        public async Task<List<ActaDTO>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerActas", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<ActaDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var Acta = new ActaDTO
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                OBRA_ID = reader.GetInt32(reader.GetOrdinal("OBRA_ID")),
                                PROVEEDOR_ID = reader.GetInt32(reader.GetOrdinal("PROVEEDOR_ID")),
                                ESPECIALIDAD_ID = reader.GetInt32(reader.GetOrdinal("ESPECIALIDAD_ID")),
                                ESTADO_ID = reader.GetInt32(reader.GetOrdinal("ESTADO_ID")),
                                FECHA_APROBACION = reader.IsDBNull(reader.GetOrdinal("FECHA_APROBACION")) ? (DateTime?) null : reader.GetDateTime(reader.GetOrdinal("FECHA_APROBACION")),
                                OBSERVACION = reader.IsDBNull(reader.GetOrdinal("OBSERVACION")) ? null : reader.GetString(reader.GetOrdinal("OBSERVACION")),
                                REVISOR_ID = reader.GetInt32(reader.GetOrdinal("REVISOR_ID")),
                                USUARIO_CREACION = reader.IsDBNull(reader.GetOrdinal("USUARIO_CREACION")) ? null : reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION")),
                            };
                            response.Add(Acta);
                        }
                    }
                    return response;
                }
            }
        }

        //---------------------------------------------------------------Insertar Parametro--------------------------------------------------------------- FUNCIONANDO
        public async Task<(int codErr, string desErr)> InsertarActa(ActaInsertDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarActa", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@OBRA_ID", value.OBRA_ID));
                    cmd.Parameters.Add(new SqlParameter("@PROVEEDOR_ID", value.PROVEEDOR_ID));
                    cmd.Parameters.Add(new SqlParameter("@ESPECIALIDAD_ID", value.ESPECIALIDAD_ID));
                    //cmd.Parameters.Add(new SqlParameter("@ESTADO_ID", value.ESTADO_ID));
                    //cmd.Parameters.Add(new SqlParameter("@FECHA_APROBACION", value.FECHA_APROBACION));
                    // Asigna DBNull.Value si FECHA_APROBACION es null
                    cmd.Parameters.Add(new SqlParameter("@FECHA_APROBACION", value.FECHA_APROBACION ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@OBSERVACION", value.OBSERVACION ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@REVISOR_ID", value.REVISOR_ID));
                    //demomento mantenemos el usaurio creacion para insertar
                    //cmd.Parameters.Add(new SqlParameter("@USUARIO_CREACION", value.USUARIO_CREACION));

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

        //---------------------------------------------------------------Actualizar Parametro------------------------------------------------------------- FUNCIONANDO
        public async Task<(int codErr, string desErr)> ActualizarActa(int id, ActaUpdateDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarActa", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@OBRA_ID", value.OBRA_ID));
                    cmd.Parameters.Add(new SqlParameter("@PROVEEDOR_ID", value.PROVEEDOR_ID));
                    cmd.Parameters.Add(new SqlParameter("@ESPECIALIDAD_ID", value.ESPECIALIDAD_ID));
                    //cmd.Parameters.Add(new SqlParameter("@ESTADO_ID", value.ESTADO_ID));
                    //cmd.Parameters.Add(new SqlParameter("@FECHA_APROBACION", value.FECHA_APROBACION));
                    cmd.Parameters.Add(new SqlParameter("@FECHA_APROBACION", value.FECHA_APROBACION ?? (object)DBNull.Value));

                    cmd.Parameters.Add(new SqlParameter("@OBSERVACION", value.OBSERVACION ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@REVISOR_ID", value.REVISOR_ID));

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

        //---------------------------------------------------------------Eliminar Parametro--------------------------------------------------------------- FUNCIONANDO
        public async Task<(int codErr, string desErr)> EliminarActa(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_EliminarActa", sql))
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

        //funcion acta usuario:
        public async Task<List<ActaUsuarioDTO>> ObtenerActasPorUsuario(int id)
        {
            Debug.WriteLine($"Repositorio - ID del usuario recibido: {id}");

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerActasPorEncargado", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario_entrante_id", id);

                    var response = new List<ActaUsuarioDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var acta = new ActaUsuarioDTO
                            {
                                Grupo = reader.GetInt32(reader.GetOrdinal("grupo")),
                                Acta = reader.GetInt32(reader.GetOrdinal("acta")),
                                Rol = reader.GetInt32(reader.GetOrdinal("rol")),
                                Encargado = reader.GetInt32(reader.GetOrdinal("encargado")),
                                Tarea = reader.GetInt32(reader.GetOrdinal("tarea"))
                            };
                            response.Add(acta);
                        }
                    }

                    return response;
                }
            }
        }




        //...........................................................MAPEO.................................................... NO SE ESTA UTILIZANDO PERO BUSCAR SOLUCION O MANTENER METODO ACTUAL PARA LISTAR
        private ActaDTO MapToActaDTO(SqlDataReader reader)
        {
            return new ActaDTO()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                OBRA_ID = reader.GetInt32(reader.GetOrdinal("OBRA_ID")),
                PROVEEDOR_ID = reader.GetInt32(reader.GetOrdinal("PROVEEDOR_ID")),
                ESPECIALIDAD_ID = reader.GetInt32(reader.GetOrdinal("ESPECIALIDAD_ID")),
                ESTADO_ID = reader.GetInt32(reader.GetOrdinal("ESTADO_ID")),
                FECHA_APROBACION = reader.IsDBNull(reader.GetOrdinal("FECHA_APROBACION")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FECHA_APROBACION")),
                OBSERVACION = reader.IsDBNull(reader.GetOrdinal("OBSERVACION")) ? null : reader.GetString(reader.GetOrdinal("OBSERVACION")),
                REVISOR_ID = reader.GetInt32(reader.GetOrdinal("REVISOR_ID")),
                USUARIO_CREACION = reader.IsDBNull(reader.GetOrdinal("USUARIO_CREACION")) ? null : reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }
    }
}