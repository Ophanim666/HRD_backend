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

        [HttpPost]
        public async Task<IActionResult> InsertarTipoParametro([FromBody] TipoParametroDTO tipoParametroDTO)
        {
            if (tipoParametroDTO == null)
            {
                return BadRequest("Datos inválidos.");
            }

            // Mapeo del DTO a la entidad del modelo si es necesario
            var tipoParametro = new TipoParametro
            {
                TIPO_PARAMETRO = tipoParametroDTO.TIPO_PARAMETRO,
                ESTADO = tipoParametroDTO.ESTADO,
                USUARIO_CREACION = tipoParametroDTO.USUARIO_CREACION,
                FECHA_CREACION = tipoParametroDTO.FECHA_CREACION
            };

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

        //funcion actualiar
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTipoParametro(int id, TipoParametroDTO tipoParametroDto)
        {
            // Mapear DTO a entidad
            var tipoParametro = new TipoParametro
            {
                ID = id,
                TIPO_PARAMETRO = tipoParametroDto.TIPO_PARAMETRO,
                ESTADO = tipoParametroDto.ESTADO,
                USUARIO_CREACION = tipoParametroDto.USUARIO_CREACION,
                FECHA_CREACION = tipoParametroDto.FECHA_CREACION
            };

            var response = await _tipoParametroRepositorio.ActualizarTipoParametro(tipoParametro);

            if (response > 0) // Si el número de filas afectadas es mayor a 0
            {
                return Ok("TipoParametro actualizado correctamente.");
            }
            else
            {
                return BadRequest("Error al actualizar el TipoParametro.");
            }
        }

        //Funcion listar
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



    }
}
