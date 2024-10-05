using Data.Repositorios;
using DTO.Proveedor;
using DTO.TipoParametro;
using DTO.Usuario;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositorios
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //aqui ponemos los codErr y desErr para poder trabajarlos
        public int codError;
        public string desError;


        //Funciion listar
        public async Task<List<UsuarioDTO>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                // Lo llama por procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerUsuarios", sql))
                {
                    var response = new List<UsuarioDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToUsuarioDTO(reader));
                        }
                    }

                    return response;
                }
            }
        }

        //---------------------------------------------------------------Insertar Usuario--------------------------------------------------------------- 
        public async Task<(int codErr, string desErr)> InsertarUsuario(UsuarioInsertDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarUsuario", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@PRIMER_NOMBRE", value.Primer_nombre));
                    cmd.Parameters.Add(new SqlParameter("@SEGUNDO_NOMBRE", value.Segundo_nombre));
                    cmd.Parameters.Add(new SqlParameter("@PRIMER_APELLIDO", value.Primer_apellido));
                    cmd.Parameters.Add(new SqlParameter("@SERGUNDO_APELLIDO", value.Segundo_apellido));
                    cmd.Parameters.Add(new SqlParameter("@RUT", value.Rut));
                    cmd.Parameters.Add(new SqlParameter("@DV", value.Dv));
                    cmd.Parameters.Add(new SqlParameter("@EMAIL", value.Email));
                    cmd.Parameters.Add(new SqlParameter("@PASSWORD", value.Password)); // Recordar hacerle un hash a las contraseñas mas adelante //
                    cmd.Parameters.Add(new SqlParameter("@ES_ADMINISTRADOR", value.Es_administrador));
                    cmd.Parameters.Add(new SqlParameter("@ROL_ID", value.Rol_id));
                    cmd.Parameters.Add(new SqlParameter("@ESTADO", value.Estado));
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

        //----------------------------------------------------------Actualizar Usuario----------------------------------------------------
        public async Task<(int codErr, string desErr)> ActualizarUsuario(int id, UsuarioUpdateDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarUsuario", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@PRIMER_NOMBRE", value.Primer_nombre));
                    cmd.Parameters.Add(new SqlParameter("@SEGUNDO_NOMBRE", value.Segundo_nombre));
                    cmd.Parameters.Add(new SqlParameter("@PRIMER_APELLIDO", value.Primer_apellido));
                    cmd.Parameters.Add(new SqlParameter("@SEGUNDO_APELLIDO", value.Segundo_apellido));
                    cmd.Parameters.Add(new SqlParameter("@RUT", value.Rut));
                    cmd.Parameters.Add(new SqlParameter("@DV", value.Dv));
                    cmd.Parameters.Add(new SqlParameter("@EMAIL", value.Email));
                    cmd.Parameters.Add(new SqlParameter("@PASSWORD", value.Password));
                    cmd.Parameters.Add(new SqlParameter("@ES_ADMINISTRADOR", value.Es_administrador));
                    cmd.Parameters.Add(new SqlParameter("@ROL_ID", value.Rol_id));
                    cmd.Parameters.Add(new SqlParameter("@ESTADO", value.Estado));

                    // Manejo de errores
                    cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.VarChar, 100)).Direction = ParameterDirection.Output;

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Retornar los códigos de error
                    codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    desError = cmd.Parameters["@des_err"].Value.ToString();

                    return (codError, desError);
                }
            }
        }

        //----------------------------------------------------------eliminar Usuario por ID-------------------------------------------------
        public async Task<(int codErr, string desErr)> EliminarUsuario(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_EliminarUsuario", sql))
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

        //...........................................................MAPEO (recorddar cambios donde se dejan pasar datos nulos)....................................................
        private UsuarioDTO MapToUsuarioDTO(SqlDataReader reader)
        {
            return new UsuarioDTO()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Primer_nombre = reader.GetString(reader.GetOrdinal("Primer_Nombre")),
                Segundo_nombre = reader.GetString(reader.GetOrdinal("Segundo_Nombre")),
                Primer_apellido = reader.GetString(reader.GetOrdinal("Primer_Apellido")),
                Segundo_apellido = reader.GetString(reader.GetOrdinal("Segundo_apellido")),
                Rut = reader.GetString(reader.GetOrdinal("Rut")),
                Dv = reader.GetString(reader.GetOrdinal("Dv")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Rol_id = reader.GetInt32(reader.GetOrdinal("Rol_id")),
                Estado = reader.GetInt32(reader.GetOrdinal("Estado")),
                //
                Usuario_creacion = reader.IsDBNull(reader.GetOrdinal("Usuario_creacion")) ? null : reader.GetString(reader.GetOrdinal("Usuario_creacion")),
                Fecha_creacion = reader.GetDateTime(reader.GetOrdinal("Fecha_creacion")),
            };
        }
    }
}


