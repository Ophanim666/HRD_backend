using AutoMapper;
using Data.Repositories;
using DTO.Acta;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActaController : Controller
    {
        private readonly ActaRepository _actaRepositorio;
        private readonly IMapper _mapper;
        public ActaController(ActaRepository actaRepositorio, IMapper mapper)
        {
            _actaRepositorio = actaRepositorio;
            _mapper = mapper;
        }

        //---------------------------------------------------------------Listar Acta---------------------------------------------------------------
        [HttpGet("Listar")]
        public async Task<ActionResult<ObjetoRequest>> ListAll()
        {
            var response = await _actaRepositorio.ListAll();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null || response.Count == 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay actas registradas";
                objetoRequest.Estado.ErrCon = "[ActaController]";
            }
            else
            {
                objetoRequest.Body = new BodyRequest()
                {
                    Response = response 
                };
            }
            return objetoRequest;
        }

        //---------------------------------------------------------------Insertar acta---------------------------------------------------------------
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> InsertarActa([FromBody] ActaInsertDTO value)
        {
            var response = await _actaRepositorio.InsertarActa(value);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();
            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ActaController]";
            }
            return objetoRequest;
        }

        //---------------------------------------------------------------Actualizar acta---------------------------------------------------------------
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarActa(int id, [FromBody] ActaUpdateDTO value)
        {
            var response = await _actaRepositorio.ActualizarActa(id, value);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ActaController]";
            }
            return objetoRequest;

        }

        //---------------------------------------------------------------Eliminar Acta---------------------------------------------------------------
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> EliminarActa(int id)
        {
            var response = await _actaRepositorio.EliminarActa(id);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ActaController]";
            }
            return objetoRequest;


        }
    }
}
