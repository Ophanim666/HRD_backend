using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.Especialidad;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadController : ControllerBase
    {
        private readonly EspecialidadRepository _especialidadRepository;
        private readonly IMapper _mapper;

        public EspecialidadController(EspecialidadRepository especialidadRepository, IMapper mapper)
        {
            _especialidadRepository = especialidadRepository;
            _mapper = mapper;
        }
        
        // Listar especialidades
        [HttpGet]
        public async Task<ActionResult<List<EspecialidadDTO>>> ListAll()
        {
            var response = await _especialidadRepository.ListAll();
            if (response == null || response.Count == 0)
            {
                return NotFound();
            }

            var EspecialidadDTOs = _mapper.Map<List<EspecialidadDTO>>(response);
            return Ok(EspecialidadDTOs);
        }

        // Añadir especialidades
        [HttpPost]
        public async Task<IActionResult> AñadirEspecialidad([FromBody] EspecialidadDTO especialidadDTO)
        {
            if (especialidadDTO == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var tipoParametro = new Especialidad
            {
                NOMBRE = especialidadDTO.NOMBRE,
                ESTADO = especialidadDTO.ESTADO,
                // se comenta por cambios de procediemitno almacenado ya no recibe estso campos
                //USUARIO_CREACION = especialidadDTO.USUARIO_CREACION,
                //FECHA_CREACION = especialidadDTO.FECHA_CREACION
            };

            try
            {
                var result = await _especialidadRepository.AñadirEspecialidad(tipoParametro);

                if (result > 0)
                {
                    return Ok("Especialidad añadida correctamente.");
                }
                else
                {
                    return StatusCode(500, "Error al añadir especialidad.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Eliminar especialidades
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEspecialidad(int id)
        {
            try
            {
                var result = await _especialidadRepository.EliminarEspecialidad(id);

                if (result != 0)
                {
                    return Ok("Especialidad eliminada correctamente.");
                }
                else
                {
                    return NotFound("Especialidad no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // SP para actualizar especialidad
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEspecialidad(int id, [FromBody] EspecialidadDTO especialidadDTO)
        {
            if (especialidadDTO == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var especialidad = new Especialidad
            {
                ID = id,
                NOMBRE = especialidadDTO.NOMBRE,
                ESTADO = especialidadDTO.ESTADO,
                // se comenta por cambios de procediemitno almacenado ya no recibe estso campos
                //USUARIO_CREACION = especialidadDTO.USUARIO_CREACION,
                //FECHA_CREACION = especialidadDTO.FECHA_CREACION
            };

            try
            {
                var response = await _especialidadRepository.ActualizarEspecialidad(especialidad);

                if (response != 0)
                {
                    return Ok("Especialidad actualizada correctamente.");
                }
                else
                {
                    return BadRequest("Error al actualizar la especialidad.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}