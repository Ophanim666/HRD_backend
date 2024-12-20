﻿using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Importamos el dto
using DTO.Parametro;
using DTO.Proveedor;


namespace Data.Repositories
{
    public class ParametroRepository
    {
        private readonly string _connectionString;

        public ParametroRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //aqui ponemos los codErr y desErr para poder trabajarlos
        public int codError;
        public string desError;

        //---------------------------------------------------------------Listar parametro----------------------------------------------------------------- FUNCIONANDO LISDATO PERO NO SE PUEDE USAR MAPEO POR EL TEMA DE QUE ID_TIPO_PARAMETRO ES UND FK REVISARr p
        public async Task<List<ParametroDTO>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerParametros", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<ParametroDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var Parametro = new ParametroDTO
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                PARAMETRO = reader.GetString(reader.GetOrdinal("PARAMETRO")),
                                VALOR = reader.GetString(reader.GetOrdinal("VALOR")),
                                ID_TIPO_PARAMETRO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_PARAMETRO")),
                                ESTADO = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                                USUARIO_CREACION = reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION")),
                            };
                            response.Add(Parametro);
                        }
                    }
                    return response;
                }
            }
        }

        //---------------------------------------------------------------Insertar Parametro--------------------------------------------------------------- FUNCIONANDO
        public async Task<(int codErr, string desErr)> InsertarParametro(ParametroInsertDTO value, string usuarioCreacion)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@PARAMETRO", value.PARAMETRO));
                    cmd.Parameters.Add(new SqlParameter("@VALOR", value.VALOR));
                    cmd.Parameters.Add(new SqlParameter("@ID_TIPO_PARAMETRO", value.ID_TIPO_PARAMETRO));
                    cmd.Parameters.Add(new SqlParameter("@ESTADO", value.ESTADO));
                    //demomento mantenemos el usaurio creacion para insertar
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

        //---------------------------------------------------------------Actualizar Parametro------------------------------------------------------------- FUNCIONANDO
        public async Task<(int codErr, string desErr)> ActualizarParametro(int id, ParametroUpdateDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarParametro", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@PARAMETRO", value.PARAMETRO));
                    cmd.Parameters.Add(new SqlParameter("@VALOR", value.VALOR));
                    cmd.Parameters.Add(new SqlParameter("@ID_TIPO_PARAMETRO", value.ID_TIPO_PARAMETRO));
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

        //---------------------------------------------------------------Eliminar Parametro--------------------------------------------------------------- FUNCIONANDO
        public async Task<(int codErr, string desErr)> EliminarParametro(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_EliminarParametro", sql))
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

        //...........................................................MAPEO.................................................... NO SE ESTA UTILIZANDO PERO BUSCAR SOLUCION O MANTENER METODO ACTUAL PARA LISTAR
        private ParametroDTO MapToParametroDTO(SqlDataReader reader)
        {
            return new ParametroDTO()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                PARAMETRO = reader.GetString(reader.GetOrdinal("PARAMETRO")),
                VALOR = reader.GetString(reader.GetOrdinal("VALOR")),
                ID_TIPO_PARAMETRO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_PARAMETRO")),
                ESTADO = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                USUARIO_CREACION = reader.IsDBNull(reader.GetOrdinal("USUARIO_CREACION")) ? null : reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }



    }
}

// se aplicaron cambios de main