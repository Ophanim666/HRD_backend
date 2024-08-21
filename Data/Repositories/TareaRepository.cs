﻿using Data.Repositorios;
using DTO.Tarea;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Data.Repositorios
{
    public class TareaRepository
    {
        private readonly string _connectionString;

        public TareaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Listar todas las tareas
        public async Task<List<Tarea>> ListAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ListarTareas", sql)) // Procedimiento almacenado correcto
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Tarea>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToTarea(reader));
                        }
                    }

                    return response;
                }
            }
        }

        // Insertar una nueva tarea
        public async Task Insert(Tarea tarea)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarTarea", sql)) // Procedimiento almacenado correcto
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOMBRE", tarea.Nombre);
                    cmd.Parameters.AddWithValue("@CODIGO", tarea.Codigo);
                    cmd.Parameters.AddWithValue("@ESTADO", tarea.Estado);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", tarea.Usuario_creacion);
                    cmd.Parameters.AddWithValue("@FECHA_CREACION", tarea.Fecha_creacion == default(DateTime) ? DateTime.Now : tarea.Fecha_creacion);

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Eliminar una tarea por ID
        public async Task Delete(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarTarea", sql)) // Procedimiento almacenado correcto
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        // Actualizar una tarea por ID
        public async Task Update(Tarea tarea)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarTarea", sql)) // Procedimiento almacenado correcto
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", tarea.Id);
                    cmd.Parameters.AddWithValue("@NOMBRE", tarea.Nombre);
                    cmd.Parameters.AddWithValue("@CODIGO", tarea.Codigo);
                    cmd.Parameters.AddWithValue("@ESTADO", tarea.Estado);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", tarea.Usuario_creacion);

                    await sql.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("No se pudo actualizar el registro.");
                    }
                }
            }
        }

        // Mapeo de los resultados
        private Tarea MapToTarea(SqlDataReader reader)
        {
            return new Tarea()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                Estado = reader.GetInt32(reader.GetOrdinal("Estado")),
                Usuario_creacion = reader.GetString(reader.GetOrdinal("Usuario_creacion")),
                Fecha_creacion = reader.GetDateTime(reader.GetOrdinal("Fecha_creacion")),
            };
        }
    }


}