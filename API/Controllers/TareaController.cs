using Microsoft.AspNetCore.Mvc;
using Data.Repositorios;
using Models.Entidades;
using System.Threading.Tasks;
using System.Collections.Generic;
using Data.Repositories;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        private readonly TareaRepository _tareaRepository;

        public TareaController(TareaRepository tareaRepository)
        {
            _tareaRepository = tareaRepository;
        }

        // GET: api/tarea
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas()
        {
            var tareas = await _tareaRepository.ListAll();
            return Ok(tareas);
        }

        // POST: api/tarea
        [HttpPost]
        public async Task<ActionResult> InsertTarea([FromBody] Tarea tarea)
        {
            if (tarea == null)
            {
                return BadRequest("Tarea no puede ser nula.");
            }

            await _tareaRepository.Insert(tarea);
            return Ok("Tarea insertada exitosamente.");
        }
        // PUT: api/tarea
        [HttpPut]
        public async Task<ActionResult> UpdateTarea([FromBody] Tarea tarea)
        {
            if (tarea == null)
            {
                return BadRequest("Tarea no puede ser nula.");
            }

            try
            {
                await _tareaRepository.Update(tarea);
                return Ok("Tarea actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la tarea: {ex.Message}");
            }
        }

        // DELETE: api/tarea/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTarea(int id)
        {
            await _tareaRepository.Delete(id);
            return Ok("Tarea eliminada exitosamente.");
        }
    }
}