using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.TipoParametro;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoParametroController : ControllerBase
    {
        private readonly TipoParametroRepository _tipoParametroRepositorio;
        private readonly IMapper _mapper;

        public TipoParametroController(TipoParametroRepository tipoParametroRepositorio, IMapper mapper)
        {
            _tipoParametroRepositorio = tipoParametroRepositorio;
            _mapper = mapper;
        }
        //listar tipoparametros
        [HttpGet]
        public async Task<ActionResult<List<TipoParametroDTO>>> ListAll()
        {
            var response = await _tipoParametroRepositorio.ListAll();
            if (response == null || response.Count == 0)
            {
                return NotFound();
            }

            var tipoParametroDTOs = _mapper.Map<List<TipoParametroDTO>>(response);
            return Ok(tipoParametroDTOs);
        }
        //insertar tipoparametros
        [HttpPost]
        public async Task<IActionResult> InsertarTipoParametro([FromBody] TipoParametroDTO tipoParametroDTO)
        {
            if (tipoParametroDTO == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var tipoParametro = new TipoParametro
            {
                TIPO_PARAMETRO = tipoParametroDTO.TIPO_PARAMETRO,
                ESTADO = tipoParametroDTO.ESTADO,
                USUARIO_CREACION = tipoParametroDTO.USUARIO_CREACION,
                FECHA_CREACION = tipoParametroDTO.FECHA_CREACION
            };

            try
            {
                var result = await _tipoParametroRepositorio.InsertarTipoParametro(tipoParametro);

                if (result > 0)
                {
                    return Ok("TipoParametro insertado correctamente.");
                }
                else
                {
                    return StatusCode(500, "Error al insertar el TipoParametro.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //editar tipo parametros
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTipoParametro(int id, [FromBody] TipoParametroDTO tipoParametroDto)
        {
            if (tipoParametroDto == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var tipoParametro = new TipoParametro
            {
                ID = id,
                TIPO_PARAMETRO = tipoParametroDto.TIPO_PARAMETRO,
                ESTADO = tipoParametroDto.ESTADO,
                USUARIO_CREACION = tipoParametroDto.USUARIO_CREACION,
                FECHA_CREACION = tipoParametroDto.FECHA_CREACION
            };

            try
            {
                var response = await _tipoParametroRepositorio.ActualizarTipoParametro(tipoParametro);

                if (response != 0)
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
        // Método para eliminar TipoParametro por ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTipoParametro(int id)
        {
            try
            {
                var result = await _tipoParametroRepositorio.EliminarTipoParametro(id);

                if (result != 0)
                {
                    return Ok("TipoParametro eliminado correctamente." + result);
                }
                else
                {
                    return NotFound("TipoParametro no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
