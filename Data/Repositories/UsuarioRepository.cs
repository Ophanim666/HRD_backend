using Data.Repositorios;
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
        //Funcion para eliminar usuarios esta funcion quedara como softdelete que no eliminara a los usaurios de la base de datos 
        

        //Funciion listar
        public async Task<List<Usuario>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                // Lo llama por procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand("AAAGetUsuarios", sql))
                {
                    var response = new List<Usuario>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToUsuario(reader));
                        }
                    }

                    return response;
                }
            }
        }
        // DE MOMENTO EL SISTEMA NO PUDE TRATAR CON VALORES NULOS Y SE DEBE VERIFICAR QUE EN LA BASE DE DATOS NO TENGA VALORES NULOS SINO NO FUNCIONARA LA EJECUCION
        private Usuario MapToUsuario(SqlDataReader reader)
        {
            return new Usuario()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Rut = reader.GetString(reader.GetOrdinal("Rut")),
                Primer_Nombre = reader.GetString(reader.GetOrdinal("Primer_Nombre")),
                Segundo_Nombre = reader.GetString(reader.GetOrdinal("Segundo_Nombre")),
                Primer_Apellido = reader.GetString(reader.GetOrdinal("Primer_Apellido")),
                Segundo_Apellido = reader.GetString(reader.GetOrdinal("Segundo_Apellido")),
                Fecha_de_nacimiento = reader.GetDateTime(reader.GetOrdinal("Fecha_nacimiento")),
                Rol = reader.GetString(reader.GetOrdinal("Rol")),
                Especialidad = reader.GetString(reader.GetOrdinal("Especialidad"))
            };
        }
    }
}


