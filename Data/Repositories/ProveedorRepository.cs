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
        //agregamos para traer el id de proveedor
        public int proveedorId;

        //----------------------------------------------------------Listar proveedores----------------------------------------------------------
        public async Task<List<ProveedorDTO>> ListAll() //revisar para ver si debe usar el mapeo del modelo o de el DTO
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerProveedor", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<ProveedorDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToProveedorDTOListar(reader));
                        }
                    }
                    return response;
                }
            }
        }


        //----------------------------------------------------------Listar proveedores con sus especialidades por id de proveedor---------------------------------------------------------- 
        public async Task<List<ProveedorConEspecialidadDTO>> ListarProveedoresConEspecialidades(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ListarProveedoresConEspecialidadesPorID", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDproveedor", id);

                    var response = new List<ProveedorConEspecialidadDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Obtener el ID y el nombre del proveedor
                            //SE SUPONE QUE SE DEBE HACER UNA VARIABLE PARA GAURDAR ESTO PERO NO SE PUEDE GAURDAR UNA VARIABLE DENTRO DE OTRA VARIABLE PARA SER ENTREGADA
                            var proveedorID = reader.GetInt32(reader.GetOrdinal("ID proveedor"));
                            var proveedorNombre = reader.GetString(reader.GetOrdinal("Nombre Proveedor"));
                            var especialidadID = reader.GetInt32(reader.GetOrdinal("ID especialidad"));
                            var especialidadNombre = reader.GetString(reader.GetOrdinal("Nombre Especialidad"));


                            // FUNCIONA PERO HAY QUE DESHACERCE DE ESTA METODOLOGIA 

                            // Busca si ya existe un proveedor en la lista
                            var proveedor = response.FirstOrDefault(p => p.IDproveedor == proveedorID);

                            if (proveedor == null)
                            {
                                // Si no existe, añade un nuevo proveedor
                                proveedor = new ProveedorConEspecialidadDTO
                                {
                                    IDproveedor = proveedorID,
                                    ProveedorNombre = proveedorNombre,
                                };

                                // Añadir la especialidad inicial
                                proveedor.IDespecialidades.Add(especialidadID);
                                proveedor.EspecialidadNombres.Add(especialidadNombre);

                                // Agregar a la lista de respuesta
                                response.Add(proveedor);
                            }
                            else
                            {
                                // Si ya existe, añade la especialidad a las listas
                                proveedor.IDespecialidades.Add(especialidadID);
                                proveedor.EspecialidadNombres.Add(especialidadNombre);
                            }
                        }
                    }
                    return response;
                }
            }
        }

        //----------------------------------------------------------Listar proveedores con sus especialidades por GENERAL PARA LISTAR Y COMPROBAR------------------
        public async Task<List<ProveedorEspecialidadGeneralDTO>> ObtenerProveedoresEspecialidadesGeneral()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerProveedorConEspecialidad", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<ProveedorEspecialidadGeneralDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Obtener el ID y el nombre del proveedor
                            int proveedorID = reader.GetInt32(reader.GetOrdinal("ID proveedor"));
                            string proveedorNombre = reader.GetString(reader.GetOrdinal("Nombre Proveedor"));
                            int especialidadID = reader.GetInt32(reader.GetOrdinal("ID especialidad"));
                            string especialidadNombre = reader.GetString(reader.GetOrdinal("Nombre Especialidad"));

                            // Buscar si ya existe un proveedor en la lista
                            var proveedor = response.FirstOrDefault(p => p.IDproveedor == proveedorID);

                            if (proveedor == null)
                            {
                                // Si no existe, añadir un nuevo proveedor con sus listas de especialidades
                                proveedor = new ProveedorEspecialidadGeneralDTO
                                {
                                    IDproveedor = proveedorID,
                                    ProveedorNombre = proveedorNombre,
                                };

                                // Añadir la especialidad inicial
                                proveedor.IDespecialidades.Add(especialidadID);
                                proveedor.EspecialidadNombres.Add(especialidadNombre);

                                // Agregar a la lista de respuesta
                                response.Add(proveedor);
                            }
                            else
                            {
                                // Si ya existe, solo añadir la especialidad a las listas
                                proveedor.IDespecialidades.Add(especialidadID);
                                proveedor.EspecialidadNombres.Add(especialidadNombre);
                            }
                        }
                    }
                    return response;
                }
            }
        }

        //----------------------------------------------------------Insertar proveedor----------------------------------------------------------
        public async Task<(int codErr, string desErr, int? proveedorId)> InsertarProveedor(ProveedorInsertDTO value) //recordar el ?
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
                    //PARA tratar especialidades
                    cmd.Parameters.Add(new SqlParameter("@ID_PROVEEDOR", SqlDbType.Int)).Direction = ParameterDirection.Output;


                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    //lo que debemos retornar
                    codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    desError = (cmd.Parameters["@des_err"].Value).ToString();
                    //
                    int? proveedorId = codError == 0 ? (int?)Convert.ToInt32(cmd.Parameters["@ID_PROVEEDOR"].Value) : null;

                    return (codError, desError, proveedorId);
                }
            }
        }

        //----------------------------------------------------------Insertar proveedor x especialidad-----------------------------------------------
        public async Task<(int codErr, string desErr)> InsertarProveedorXEspecialidad(int proveedorId, List<int> especialidadesIds)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_InsertarProveedorEspecialidades", sql)) //cambiar procedimiento
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //añadimos valores a los parametros
                    cmd.Parameters.Add(new SqlParameter("@ID_PROVEEDOR", proveedorId));
                    string especialidadesIdsStr = string.Join(",", especialidadesIds);
                    cmd.Parameters.Add(new SqlParameter("@LISTA_ESPECIALIDADES", especialidadesIdsStr));


                    // Manejo de errores
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

        //----------------------------------------------------------Actualizar Proveedor----------------------------------------------------
        public async Task<(int codErr, string desErr)> ActualizarProveedor(int id, ProveedorUpdateDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarProveedor", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
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

        //----------------------------------------------------------Actualizar Proveedor con Especialidades----------------------------------------------------
        public async Task<(int codErr, string desErr)> ActualizarProveedorXEspecialidad(int proveedorId, List<int> especialidadesIds)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarProveedorEspecialidades", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros
                    cmd.Parameters.Add(new SqlParameter("@ID_PROVEEDOR", proveedorId));
                    string especialidadesIdsStr = string.Join(",", especialidadesIds);
                    cmd.Parameters.Add(new SqlParameter("@LISTA_ESPECIALIDADES", especialidadesIdsStr));

                    // Manejo de errores
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

        //...........................................................MAPEO (recordar sacar lo de vaores nulos)....................................................

        private ProveedorDTO MapToProveedorDTOListar(SqlDataReader reader)
        {
            return new ProveedorDTO()
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