
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.GrupoTareas;

namespace Data.Repositories
{
    public class GrupoTareasRepository
    {
        private readonly string _connectionString;

        public GrupoTareasRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //aqui ponemos los codErr y desErr para poder trabajarlos
        public int codError;
        public string desError;
        //agregamos para traer el id de proveedor
        public int grupoTareaId;

        //----------------------------------------------------------Listar Grupo Tareas----------------------------------------------------------
        public async Task<List<GrupoTareasDTO>> ListAll() 
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerGrupoTarea", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<GrupoTareasDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToGrupoTareasDTOListar(reader));
                        }
                    }
                    return response;
                }
            }
        }

        //---------------------------------------------------------------listado...............................................................................
        public async Task<List<ListarTareasXGrupoTareasDTO>> ListAllGrupoTareaxTareas()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ListarTareasxGrupoTareas", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var response = new List<ListarTareasXGrupoTareasDTO>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Obtener el ID del grupo y su información
                            int idGrupoTarea = reader.GetInt32(reader.GetOrdinal("IDGrupoTarea"));
                            int idActa = reader.GetInt32(reader.GetOrdinal("IDActa"));
                            int idRol = reader.GetInt32(reader.GetOrdinal("IDRol"));
                            int idEncargado = reader.GetInt32(reader.GetOrdinal("IDEncargado"));
                            //agregar fecha
                            DateTime fechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"));
                            string usuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? null : reader.GetString(reader.GetOrdinal("UsuarioCreacion"));

                            // Obtener el ID y nombre de la tarea
                            int idTarea = reader.GetInt32(reader.GetOrdinal("IDTarea"));

                            // Buscar si el grupoi ya está en la lista
                            var grupoTarea = response.FirstOrDefault(p => p.IDGrupoTarea == idGrupoTarea);

                            if (grupoTarea == null)
                            {
                                // Si el proveedor no existe, lo agregamos con la primera tarea
                                grupoTarea = new ListarTareasXGrupoTareasDTO
                                {
                                    IDGrupoTarea = idGrupoTarea,
                                    IDActa = idActa,
                                    IDRol = idRol,
                                    IDEncargado = idEncargado,
                                    FechaCreacion = fechaCreacion,
                                    UsuarioCreacion = usuarioCreacion,

                                    // Inicializar listas de tares
                                    IDTarea = new List<int> { idTarea }
                                };

                                // Añadir el grupo a la lista de respuesta
                                response.Add(grupoTarea);
                            }
                            else
                            {
                                // Si el grupo ya existe, solo agregamos la tarae a las listas
                                grupoTarea.IDTarea.Add(idTarea);
                            }
                        }
                    }
                    return response;
                }
            }
        }

        //----------------------------------------------------------Listar proveedores con sus especialidades por id de proveedor---------------------------------------------------------- 
        //public async Task<List<ProveedorConEspecialidadDTO>> ListarProveedoresConEspecialidades(int id)
        //{
        //    using (SqlConnection sql = new SqlConnection(_connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("usp_ListarProveedoresConEspecialidadesPorID", sql))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@IDproveedor", id);

        //            var response = new List<ProveedorConEspecialidadDTO>();
        //            await sql.OpenAsync();

        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    // Obtener el ID y el nombre del proveedor
        //                    //SE SUPONE QUE SE DEBE HACER UNA VARIABLE PARA GAURDAR ESTO PERO NO SE PUEDE GAURDAR UNA VARIABLE DENTRO DE OTRA VARIABLE PARA SER ENTREGADA
        //                    var proveedorID = reader.GetInt32(reader.GetOrdinal("ID proveedor"));
        //                    var proveedorNombre = reader.GetString(reader.GetOrdinal("Nombre Proveedor"));
        //                    var especialidadID = reader.GetInt32(reader.GetOrdinal("ID especialidad"));
        //                    var especialidadNombre = reader.GetString(reader.GetOrdinal("Nombre Especialidad"));


        //                    // FUNCIONA PERO HAY QUE DESHACERCE DE ESTA METODOLOGIA 

        //                    // Busca si ya existe un proveedor en la lista
        //                    var proveedor = response.FirstOrDefault(p => p.IDproveedor == proveedorID);

        //                    if (proveedor == null)
        //                    {
        //                        // Si no existe, añade un nuevo proveedor
        //                        proveedor = new ProveedorConEspecialidadDTO
        //                        {
        //                            IDproveedor = proveedorID,
        //                            ProveedorNombre = proveedorNombre,
        //                        };

        //                        // Añadir la especialidad inicial
        //                        proveedor.IDespecialidades.Add(especialidadID);
        //                        proveedor.EspecialidadNombres.Add(especialidadNombre);

        //                        // Agregar a la lista de respuesta
        //                        response.Add(proveedor);
        //                    }
        //                    else
        //                    {
        //                        // Si ya existe, añade la especialidad a las listas
        //                        proveedor.IDespecialidades.Add(especialidadID);
        //                        proveedor.EspecialidadNombres.Add(especialidadNombre);
        //                    }
        //                }
        //            }
        //            return response;
        //        }
        //    }
        //}

        //----------------------------------------------------------Listar proveedores con sus especialidades por GENERAL PARA LISTAR Y COMPROBAR------------------
        //public async Task<List<ProveedorEspecialidadGeneralDTO>> ObtenerProveedoresEspecialidadesGeneral()
        //{
        //    using (SqlConnection sql = new SqlConnection(_connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("usp_ObtenerProveedorConEspecialidad", sql))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            var response = new List<ProveedorEspecialidadGeneralDTO>();
        //            await sql.OpenAsync();

        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    // Obtener el ID y el nombre del proveedor
        //                    int proveedorID = reader.GetInt32(reader.GetOrdinal("ID proveedor"));
        //                    string proveedorNombre = reader.GetString(reader.GetOrdinal("Nombre Proveedor"));
        //                    int especialidadID = reader.GetInt32(reader.GetOrdinal("ID especialidad"));
        //                    string especialidadNombre = reader.GetString(reader.GetOrdinal("Nombre Especialidad"));

        //                    // Buscar si ya existe un proveedor en la lista
        //                    var proveedor = response.FirstOrDefault(p => p.IDproveedor == proveedorID);

        //                    if (proveedor == null)
        //                    {
        //                        // Si no existe, añadir un nuevo proveedor con sus listas de especialidades
        //                        proveedor = new ProveedorEspecialidadGeneralDTO
        //                        {
        //                            IDproveedor = proveedorID,
        //                            ProveedorNombre = proveedorNombre,
        //                        };

        //                        // Añadir la especialidad inicial
        //                        proveedor.IDespecialidades.Add(especialidadID);
        //                        proveedor.EspecialidadNombres.Add(especialidadNombre);

        //                        // Agregar a la lista de respuesta
        //                        response.Add(proveedor);
        //                    }
        //                    else
        //                    {
        //                        // Si ya existe, solo añadir la especialidad a las listas
        //                        proveedor.IDespecialidades.Add(especialidadID);
        //                        proveedor.EspecialidadNombres.Add(especialidadNombre);
        //                    }
        //                }
        //            }
        //            return response;
        //        }
        //    }
        //}

        //----------------------------------------------------------Insertar Grupo tarea----------------------------------------------------------
        public async Task<(int codErr, string desErr, int? grupoTareaId)> InsertarGrupoTarea(GrupoTareasInsertDTO value) //recordar el ?
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarGrupoTarea", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //añadimos valores a los parametros
                    cmd.Parameters.Add(new SqlParameter("@ACTA_ID", value.ACTA_ID));
                    cmd.Parameters.Add(new SqlParameter("@ROL_ID", value.ROL_ID));
                    cmd.Parameters.Add(new SqlParameter("@ENCARGADO_ID", value.ENCARGADO_ID));

                    //agregamos nuestro manejo de errores
                    cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.VarChar, 100)).Direction = ParameterDirection.Output;
                    //PARA tratar TAREAS
                    cmd.Parameters.Add(new SqlParameter("@ID_GRUPO_TAREA", SqlDbType.Int)).Direction = ParameterDirection.Output;


                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    //lo que debemos retornar
                    codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    desError = (cmd.Parameters["@des_err"].Value).ToString();
                    //
                    int? grupoTareaId = codError == 0 ? (int?)Convert.ToInt32(cmd.Parameters["@ID_GRUPO_TAREA"].Value) : null;

                    return (codError, desError, grupoTareaId);
                }
            }
        }

        //----------------------------------------------------------Insertar grupo tarea x tareas-----------------------------------------------
        public async Task<(int codErr, string desErr)> InsertarTareasXGrupoTarea(int grupoTareaId, List<int> tareasIds)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarTareasXGrupoTarea", sql)) 
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //añadimos valores a los parametros
                    cmd.Parameters.Add(new SqlParameter("@ID_GRUPO_TAREA", grupoTareaId));
                    string tareasIdsStr = string.Join(",", tareasIds);
                    cmd.Parameters.Add(new SqlParameter("@LISTA_TAREAS", tareasIdsStr));


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

        //----------------------------------------------------------Actualizar grupo tarea----------------------------------------------------//aqui se actualiza el estado de grupo de tarea si esta realizado o rechazado y esos datos vendran de la tabla de parametro
        public async Task<(int codErr, string desErr)> ActualizarGrupoTarea(int id, GrupoTareasUpdateDTO value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarGrupoTarea", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@ACTA_ID", value.ACTA_ID));
                    cmd.Parameters.Add(new SqlParameter("@ROL_ID", value.ROL_ID));
                    cmd.Parameters.Add(new SqlParameter("@ENCARGADO_ID", value.ENCARGADO_ID));

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

        //----------------------------------------------------------Actualizar grupo tarea x tareas----------------------------------------------------//aqui se pondra el estado para el si o el no por tarea
        public async Task<(int codErr, string desErr)> ActualizarTareasXGrupoTarea(int grupoTareaId, List<int> tareasIds)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarTareasXGrupoTarea", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros
                    cmd.Parameters.Add(new SqlParameter("@ID_GRUPO_TAREA", grupoTareaId));
                    string tareasIdsStr = string.Join(",", tareasIds);
                    cmd.Parameters.Add(new SqlParameter("@LISTA_TAREAS", tareasIdsStr));


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

        //----------------------------------------------------------eliminar GruposTarea por ID-------------------------------------------------
        public async Task<(int codErr, string desErr)> EliminarGrupoTarea(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_EliminarGrupoTarea", sql))
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

        // cambiar estado de grupo de tareas por tarea a si o no 
        public async Task<(int codErr, string desErr)> ActualizarEstadoTareaEnGrupo(int grupoTareaId, int tareaId, int? estado)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarEstadoTareaEnGrupo", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    cmd.Parameters.Add(new SqlParameter("@grupo_tarea_id", grupoTareaId));
                    cmd.Parameters.Add(new SqlParameter("@tarea_id", tareaId));
                    cmd.Parameters.Add(new SqlParameter("@estado", estado.HasValue ? (object)estado.Value : DBNull.Value));

                    // Parámetros de salida
                    cmd.Parameters.Add(new SqlParameter("@cod_err", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    cmd.Parameters.Add(new SqlParameter("@des_err", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output });

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Leer los parámetros de salida
                    int codError = Convert.ToInt32(cmd.Parameters["@cod_err"].Value);
                    string desError = cmd.Parameters["@des_err"].Value.ToString();

                    return (codError, desError);
                }
            }
        }


        //...........................................................MAPEO (recordar sacar lo de vaores nulos)....................................................

        private GrupoTareasDTO MapToGrupoTareasDTOListar(SqlDataReader reader)
        {
            return new GrupoTareasDTO()
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                ACTA_ID = reader.GetInt32(reader.GetOrdinal("ACTA_ID")),
                ROL_ID = reader.GetInt32(reader.GetOrdinal("ROL_ID")),
                ENCARGADO_ID = reader.GetInt32(reader.GetOrdinal("ENCARGADO_ID")),
                //eliminar mas adelante cuando usuario creacion este habilitado ya que el mapeo no acepta valores nulos y los metodos de insercion no contemplan insertar usuario_creacion
                USUARIO_CREACION = reader.IsDBNull(reader.GetOrdinal("USUARIO_CREACION")) ? null : reader.GetString(reader.GetOrdinal("USUARIO_CREACION")),
                FECHA_CREACION = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
            };
        }
    }
}
