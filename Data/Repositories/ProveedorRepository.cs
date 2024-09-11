using Models.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
//Importamos el dto
using DTO.Proveedor;

namespace Data.Repositories
{
    public class ProveedorRepository
    {
        private readonly string _connectionString;

        public ProveedorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        //aqui ponemos los codErr y desErr para poder trabajarlos
        public int codError;
        public string desError;

        //----------------------------------------------------------eliminar proveedores por ID-------------------------------------------------
        public async Task<(int codErr, string desErr)> EliminarProveedor(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_EliminarProveedor", sql))
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

        //----------------------------------------------------------Insertar proveedor----------------------------------------------------------
        public async Task<(int codErr, string desErr)> InsertarProveedor(ProveedorInsertDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarProveedor", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //añadimos valores a los parametros
                    cmd.Parameters.Add(new SqlParameter("@NOMBRE", value.NOMBRE));
                    cmd.Parameters.Add(new SqlParameter("@RAZON_SOCIAL", value.RAZON_SOCIAL));
                    cmd.Parameters.Add(new SqlParameter("@RUT", value.RUT));
                    cmd.Parameters.Add(new SqlParameter("@DV", value.DV));
                    cmd.Parameters.Add(new SqlParameter("@NOMBRE_CONTACTO_PRINCIPAL", value.NOMBRE_CONTACTO_PRINCIPAL));
                    cmd.Parameters.Add(new SqlParameter("@NUMERO_CONTACTO_PRINCIPAL", value.NUMERO_CONTACTO_PRINCIPAL));
                    cmd.Parameters.Add(new SqlParameter("@NOMBRE_CONTACTO_SECUNDARIO", value.NOMBRE_CONTACTO_SECUNDARIO));
                    cmd.Parameters.Add(new SqlParameter("@NUMERO_CONTACTO_SECUNDARIO", value.NUMERO_CONTACTO_SECUNDARIO));
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

        //----------------------------------------------------------Actualizar TipoParametro----------------------------------------------------
        public async Task<(int codErr, string desErr)> ActualizarProveedor(int id,ProveedorUpdateDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarProveedor", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //con esto agregamos valores a los parametros
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@NOMBRE", value.NOMBRE));
                    cmd.Parameters.Add(new SqlParameter("@RAZON_SOCIAL", value.RAZON_SOCIAL));
                    cmd.Parameters.Add(new SqlParameter("@RUT", value.RUT));
                    cmd.Parameters.Add(new SqlParameter("@DV", value.DV));
                    cmd.Parameters.Add(new SqlParameter("@NOMBRE_CONTACTO_PRINCIPAL", value.NOMBRE_CONTACTO_PRINCIPAL));
                    cmd.Parameters.Add(new SqlParameter("@NUMERO_CONTACTO_PRINCIPAL", value.NUMERO_CONTACTO_PRINCIPAL));
                    cmd.Parameters.Add(new SqlParameter("@NOMBRE_CONTACTO_SECUNDARIO", value.NOMBRE_CONTACTO_SECUNDARIO));
                    cmd.Parameters.Add(new SqlParameter("@NUMERO_CONTACTO_SECUNDARIO", value.NUMERO_CONTACTO_SECUNDARIO));
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

        //----------------------------------------------------------Función para listar----------------------------------------------------------
        public async Task<List<Proveedor>> ListAll() //revisar para ver si debe usar el mapeo o el DTO
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerProveedor", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<Proveedor>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToProveedor(reader));
                        }
                    }
                    return response;
                }
            }
        }

        //----------------------------------------------------------Listar por ID----------------------------------------------------------------
        public async Task<Proveedor> ListarPorIdProveedor(int id) //revisar para ver si debe usar el mapeo o el DTO
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerProveedorPorId", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    Proveedor response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            response = MapToProveedor(reader);
                        }
                    }

                    return response;
                }
            }
        }
        
        //...........................................................MAPEO (recordar sacar lo de vaores nulos)....................................................
        private Proveedor MapToProveedor(SqlDataReader reader)
        {
            return new Proveedor()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                NOMBRE = reader.GetString(reader.GetOrdinal("NOMBRE")),
                RAZON_SOCIAL = reader.GetString(reader.GetOrdinal("RAZON_SOCIAL")),
                RUT = reader.GetString(reader.GetOrdinal("RUT")),
                DV = reader.GetString(reader.GetOrdinal("DV")),
                NOMBRE_CONTACTO_PRINCIPAL = reader.GetString(reader.GetOrdinal("NOMBRE_CONTACTO_PRINCIPAL")),
                NUMERO_CONTACTO_PRINCIPAL = reader.GetInt32(reader.GetOrdinal("NUMERO_CONTACTO_PRINCIPAL")),
                NOMBRE_CONTACTO_SECUNDARIO = reader.GetString(reader.GetOrdinal("NOMBRE_CONTACTO_SECUNDARIO")),
                NUMERO_CONTACTO_SECUNDARIO = reader.GetInt32(reader.GetOrdinal("NUMERO_CONTACTO_SECUNDARIO")),
                ESTADO = reader.GetInt32(reader.GetOrdinal("ESTADO")),
                //eliminar mas adelante cuando usuario creacion este habilitado ya que el mapeo no acepta valores nulos y los metodos de insercion no contemplan insertar usuario_creacion
                USUARIO_CREACION = reader.IsDBNull(reader.GetOrdinal("USUARIO_CREACION")) ? null : reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))


            };
        }
    }
}