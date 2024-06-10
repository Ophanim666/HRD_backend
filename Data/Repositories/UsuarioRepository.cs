using Data.Repositorios;
using DTO.Usuario;
using Models.Entidades;
using System;
using System.Collections.Generic;
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

        ////public IEnumerable<Usuario> GetUsuarios()
        ////{
        ////    var usuarios = new List<Usuario>();

        ////    using (var connection = new SqlConnection(_connectionString))
        ////    {
        ////        var command = new SqlCommand("SELECT Id, UserName FROM Usuarios", connection);
        ////        connection.Open();
        ////        using (var reader = command.ExecuteReader())
        ////        {
        ////            while (reader.Read())
        ////            {
        ////                var usuario = new Usuario
        ////                {
        ////                    Id = reader.GetInt32(0),
        ////                    UserName = reader.GetString(1),
        ////                };
        ////                usuarios.Add(usuario);
        ////            }
        ////        }
        ////    }

        ////    return usuarios;
        ////}

        public async Task<List<Usuario>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                //Lo llama por procedimiento almacenado
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

        private Usuario MapToUsuario(SqlDataReader reader)
        {
            return new Usuario()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                UserName = reader.GetString(reader.GetOrdinal("UserName"))
            };

        }
    }
}

