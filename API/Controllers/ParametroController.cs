using AutoMapper;
using Data.Repositories;
using DTO.Parametro;
using DTO.TipoParametro;
using Microsoft.AspNetCore.Mvc;
using Models.Entidades;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParametroController : Controller
    {
        private readonly ParametroRepository _parametroRepositorio;
        private readonly IMapper _mapper;
        public ParametroController(ParametroRepository parametroRepositorio, IMapper mapper)
        {
            _parametroRepositorio = parametroRepositorio;
            _mapper = mapper;
        }
        //insertar parametros
        [HttpPost]
        public async Task<IActionResult> InsertarParametro([FromBody] ParametroDTO parametroDTO)
        {
            if (parametroDTO == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var parametro = new Parametro
            {
                PARAMETRO = parametroDTO.PARAMETRO,
                VALOR = parametroDTO.VALOR,
                ID_TIPO_PARAMETRO=parametroDTO.ID_TIPO_PARAMETRO,
                USUARIO_CREACION = parametroDTO.USUARIO_CREACION,
            };

            try
            {
                var result = await _parametroRepositorio.InsertarParametro(parametro);

                if (result > 0)
                {
                    return Ok("Parametro insertado correctamente.");
                }
                else
                {
                    return StatusCode(500, "Error al insertar el Parametro.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //editar PARAMETROS
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarParametro(int id, [FromBody] ParametroDTO parametroDto)
        {
            if (parametroDto == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var parametro = new Parametro
            {
                ID = id,
                PARAMETRO = parametroDto.PARAMETRO,
                VALOR = parametroDto.VALOR,
                ID_TIPO_PARAMETRO = parametroDto.ID_TIPO_PARAMETRO,
                ESTADO = parametroDto.ESTADO
            };

            try
            {
                var response = await _parametroRepositorio.ActualizarParametro(parametro);

                if (response > 0)
                {
                    return Ok("TipoParametro actualizado correctamente.");
                }
                else
                {
                    return BadRequest("Error al actualizar el TipoParametro.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //obtener parametros
        [HttpGet]
        public async Task<ActionResult<List<ParametroDTO>>> ListAll()
        {
            var response = await _parametroRepositorio.ListAll();
            if (response == null || response.Count == 0)
            {
                return NotFound();
            }

            var parametroDTOs = _mapper.Map<List<ParametroDTO>>(response);
            return Ok(parametroDTOs);
        }

        // eliminar Parametro
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarParametro(int id)
        {
            try
            {
                var result = await _parametroRepositorio.EliminarParametro(id);

                if (result != 0)
                {
                    return Ok("Parametro eliminado correctamente." + result);
                }
                else
                {
                    return NotFound("Parametro no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
