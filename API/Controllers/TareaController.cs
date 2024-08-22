using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.Tarea;
using AutoMapper;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareaController : ControllerBase
    {
        private readonly TareaRepository _tareaRepositorio;
        private readonly IMapper _mapper;

        public TareaController(TareaRepository tareaRepositorio, IMapper mapper)
        {
            _tareaRepositorio = tareaRepositorio;
            _mapper = mapper;
        }

        // Listar todas las tareas
        [HttpGet]
        public async Task<ActionResult<List<TareaDTO>>> ListAll()
        {
            var response = await _tareaRepositorio.ListAll();
            if (response == null || response.Count == 0)
            {
                return NotFound();
            }

            var tareaDTOs = _mapper.Map<List<TareaDTO>>(response);
            return Ok(tareaDTOs);
        }

        // Listar tarea por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TareaDTO>> ListarPorId(int id)
        {
            var response = await _tareaRepositorio.ListarPorId(id);
            if (response == null)
            {
                return NotFound();
            }

            var tareaDTO = _mapper.Map<TareaDTO>(response);
            return Ok(tareaDTO);
        }

        // Insertar nueva tarea
        [HttpPost]
        public async Task<IActionResult> InsertarTarea([FromBody] TareaDTO tareaDTO)
        {
            if (tareaDTO == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var tarea = _mapper.Map<Tarea>(tareaDTO);

            try
            {
                var result = await _tareaRepositorio.InsertarTarea(tarea);

                if (result > 0)
                {
                    return Ok("Tarea insertada correctamente.");
                }
                else
                {
                    return StatusCode(500, "Error al insertar la tarea.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Actualizar tarea
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTarea(int id, [FromBody] TareaDTO tareaDTO)
        {
            if (tareaDTO == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var tarea = _mapper.Map<Tarea>(tareaDTO);
            tarea.ID = id;

            try
            {
                var response = await _tareaRepositorio.ActualizarTarea(tarea);

                if (response != 0)
                {
                    return Ok("Tarea actualizada correctamente.");
                }
                else
                {
                    return BadRequest("Error al actualizar la tarea.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Eliminar tarea por ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTarea(int id)
        {
            try
            {
                var result = await _tareaRepositorio.EliminarTarea(id);

                if (result != 0)
                {
                    return Ok("Tarea eliminada correctamente.");
                }
                else
                {
                    return NotFound("Tarea no encontrada.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
