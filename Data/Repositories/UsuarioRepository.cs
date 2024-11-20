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
//
using BCrypt.Net;
using System.Diagnostics;


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


        //---------------------------------------------------Función para listar---------------------------------------------------
        public async Task<List<UsuarioDTO>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerUsuarios", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

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
        // Insertar Usuario (modificado para incluir usuario_creacion)
        public async Task<(int codErr, string desErr)> InsertarUsuario(UsuarioInsertDTO value, string usuarioCreacion)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarUsuario", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string hashedPassword = HashPasswordBCrypt(value.Password);

                    cmd.Parameters.Add(new SqlParameter("@PRIMER_NOMBRE", value.Primer_nombre));
                    cmd.Parameters.Add(new SqlParameter("@SEGUNDO_NOMBRE", value.Segundo_nombre));
                    cmd.Parameters.Add(new SqlParameter("@PRIMER_APELLIDO", value.Primer_apellido));
                    cmd.Parameters.Add(new SqlParameter("@SEGUNDO_APELLIDO", value.Segundo_apellido));
                    cmd.Parameters.Add(new SqlParameter("@RUT", value.Rut));
                    cmd.Parameters.Add(new SqlParameter("@DV", value.Dv));
                    cmd.Parameters.Add(new SqlParameter("@EMAIL", value.Email));
                    cmd.Parameters.Add(new SqlParameter("@PASSWORD", hashedPassword));
                    cmd.Parameters.Add(new SqlParameter("@ES_ADMINISTRADOR", value.Es_administrador));
                    cmd.Parameters.Add(new SqlParameter("@ROL_ID", value.Rol_id));
                    cmd.Parameters.Add(new SqlParameter("@ESTADO", value.Estado));
                    cmd.Parameters.Add(new SqlParameter("@USUARIO_CREACION", usuarioCreacion)); // Asignamos el usuario que está creando
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

        //----------------------------------------------------------Eliminar Usuario por ID-------------------------------------------------
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

        //----------------------------------------------------------LogInUsuarios------------------------------------------------
        // Para hashear una contraseña:
        public string HashPasswordBCrypt(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        //Para verificar una contraseña(compara el hash guardado con la contraseña que el usuario ingresó)
        public bool VerifyPasswordBCrypt(string password, string hashedPassword)
        {
            
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        //-----------------------------------------------------LogIn Admin----------------------------------------------------------
        public async Task<(UsuarioTokenDTO usuario, int codErr, string desErr)> ObtenerUsuarioPorEmail(string email, string password)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerUsuarioPorEmailYPassword", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Email", email));

                    cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output });

                    await sql.OpenAsync();

                    UsuarioTokenDTO usuario = null;
                    string passwordHash = null;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Crear el objeto usuario
                            usuario = new UsuarioTokenDTO
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                Email = reader["Email"].ToString(),
                                EsAdministrador = reader["es_administrador"] != DBNull.Value ? Convert.ToInt32(reader["es_administrador"]) : 0,
                            };

                            // Obtener el hash de la contraseña desde la base de datos
                            passwordHash = reader["password"] != DBNull.Value ? reader["password"].ToString().Trim() : null;
                        }
                    }

                    // Obtener los parámetros de salida
                    int codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    string desError = cmd.Parameters["@des_err"].Value.ToString();

                    // Validaciones adicionales
                    if (usuario == null)
                    {
                        return (null, codError, desError);  // Usuario no encontrado o error en el procedimiento almacenado
                    }

                    // Bloquear el acceso si el usuario no es administrador
                    if (usuario.EsAdministrador == 0)
                    {
                        return (null, 10004, "Acceso denegado: El usuario no es administrador.");
                    }

                    // Verificación de la contraseña usando BCrypt
                    if (VerifyPasswordBCrypt(password, passwordHash))
                    {
                        return (usuario, 0, "OK"); // Usuario válido y contraseña correcta
                    }
                    else
                    {
                        return (null, 10001, "Credenciales inválidas."); // Contraseña incorrecta
                    }
                }
            }
        }

        //-----------------------------------------------------LogIn Usuarios----------------------------------------------------------
        public async Task<(UsuarioTokenDTO usuario, int codErr, string desErr)> LoginEstandar(string email, string password)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerUsuarioPorEmailYPassword", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Email", email));

                    cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output });

                    await sql.OpenAsync();

                    UsuarioTokenDTO usuario = null;
                    string passwordHash = null;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Crear el objeto usuario
                            usuario = new UsuarioTokenDTO
                            {
                                Email = reader["Email"].ToString(),
                                EsAdministrador = reader["es_administrador"] != DBNull.Value ? Convert.ToInt32(reader["es_administrador"]) : 0,
                            };

                            // Obtener el hash de la contraseña desde la base de datos
                            passwordHash = reader["password"] != DBNull.Value ? reader["password"].ToString().Trim() : null;
                        }
                    }

                    // Obtener los parámetros de salida
                    int codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    string desError = cmd.Parameters["@des_err"].Value.ToString();

                    // Validaciones adicionales
                    if (usuario == null)
                    {
                        return (null, codError, desError);  // Usuario no encontrado o error en el procedimiento almacenado
                    }

                    // Verificación de la contraseña usando BCrypt
                    if (VerifyPasswordBCrypt(password, passwordHash))
                    {
                        return (usuario, 0, "OK"); // Usuario válido y contraseña correcta
                    }
                    else
                    {
                        return (null, 10001, "Credenciales inválidas."); // Contraseña incorrecta
                    }
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
                Es_administrador = reader.GetInt32(reader.GetOrdinal("Es_administrador")), 
                Usuario_creacion = reader.IsDBNull(reader.GetOrdinal("Usuario_creacion")) ? null : reader.GetString(reader.GetOrdinal("Usuario_creacion")),
                Fecha_creacion = reader.IsDBNull(reader.GetOrdinal("Fecha_creacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("Fecha_creacion"))
            };
        }
    }
}


//----------------------------------------------------------ESTE ES UN JEMPLO DE QUE MUESTRA COMO USAR LA DEPURACION DENTRO DE EL CODIGO
//public async Task<(UsuarioTokenDTO usuario, int codErr, string desErr)> ObtenerUsuarioPorEmail(string email, string password)
//{
//    using (SqlConnection sql = new SqlConnection(_connectionString))
//    {
//        using (SqlCommand cmd = new SqlCommand("usp_ObtenerUsuarioPorEmailYPassword", sql))
//        {
//            cmd.CommandType = CommandType.StoredProcedure;
//            cmd.Parameters.Add(new SqlParameter("@Email", email));

//            cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int) { Direction = ParameterDirection.Output });
//            cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output });

//            await sql.OpenAsync();

//            UsuarioTokenDTO usuario = null;
//            string passwordHash = null;


//            using (var reader = await cmd.ExecuteReaderAsync())
//            {
//                if (await reader.ReadAsync())
//                {

//                    usuario = new UsuarioTokenDTO
//                    {
//                        Email = reader["Email"].ToString(),
//                        EsAdministrador = reader["es_administrador"] != DBNull.Value ? Convert.ToInt32(reader["es_administrador"]) : 0,
//                    };

//                    passwordHash = reader["password"].ToString().Trim(); // Obtén el hash de la BD
//                }

//            }

//            int codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
//            string desError = cmd.Parameters["@des_err"].Value.ToString();


//            Debug.WriteLine("Password Hash desde la base de datos: " + passwordHash + "contraseña en texto " + password);
//            Debug.WriteLine(codError);
//            Debug.WriteLine(VerifyPasswordBCrypt(password, passwordHash));


//            // Utiliza VerifyPasswordBCrypt para comparar
//            if (usuario != null && codError == 0 && VerifyPasswordBCrypt(password, passwordHash))
//            {
//                return (usuario, 0, "OK");
//            }
//            else
//            {
//                return (null, codError, "Credenciales inválidas o usuario no encontrado");
//            }
//        }
//    }
//}