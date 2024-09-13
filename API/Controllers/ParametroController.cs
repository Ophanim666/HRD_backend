using AutoMapper;
using Data.Repositories;
using DTO;
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
        //---------------------------------------------------------------Insertar parametro---------------------------------------------------------------
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> InsertarParametro([FromBody] ParametroInsertDTO value)
        {
            var response = await _parametroRepositorio.InsertarParametro(value);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();
            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ParametroController]";
            }
            return objetoRequest;
        }

        //---------------------------------------------------------------Actualizar parametro---------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarParametro(int id, [FromBody] ParametroUpdateDTO value)
        {
            var response = await _parametroRepositorio.ActualizarParametro(id, value);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ParametroController]";
            }
            return objetoRequest;

        }

        //---------------------------------------------------------------Obtener Parametro---------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<ObjetoRequest>> ListAll()
        {
            var response = await _parametroRepositorio.ListAll();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay parametro registrados";
                objetoRequest.Estado.ErrCon = "[ParametroController]";
            }

            var tipoParametroDTOs = _mapper.Map<List<TipoParametroDTO>>(response);
            objetoRequest.Body = new BodyRequest()
            {
                Response = tipoParametroDTOs
            };
            return objetoRequest;
        }

        //---------------------------------------------------------------Eliminar Parametro---------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<ActionResult<ObjetoRequest>> EliminarParametro(int id)
        {
            var response = await _parametroRepositorio.EliminarParametro(id);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ParametroController]";
            }
            return objetoRequest;


        }
    }
}
