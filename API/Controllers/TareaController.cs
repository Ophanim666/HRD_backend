using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.Especialidad;
using AutoMapper;
using DTO;
using DTO.Tarea;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareaController : ControllerBase
    {
        private readonly TareaRepository _tareaRepository;
        private readonly IMapper _mapper;

        public TareaController(TareaRepository tareaRepository, IMapper mapper)
        {
            _tareaRepository = tareaRepository;
            _mapper = mapper;
        }

        //---------------------------------------------------Listar Tarea---------------------------------------------------
        [Authorize]
        [HttpGet("ListarTareas")] // se parametrizo cambiar en el frontend
        public async Task<ActionResult<ObjetoRequest>> ListAll()
        {
            var response = await _tareaRepository.ListAll();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay tareas registradas";
                objetoRequest.Estado.ErrCon = "[Tareacontroller]";
            }

            var TareaDTOs = _mapper.Map<List<TareaDTO>>(response);

            objetoRequest.Body = new BodyRequest()
            {
                Response = TareaDTOs
            };
            return objetoRequest;
        }

        //---------------------------------------------------Añadir Tarea---------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> AñadirTarea([FromBody] TareaInsertDTO value)
        {
            var response = await _tareaRepository.AñadirTarea(value);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[Tareacontroller]";
            }
            return objetoRequest;
        }

        //---------------------------------------------------Eliminar Tarea---------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> EliminarTarea(int id)
        {
            var response = await _tareaRepository.EliminarTarea(id);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();


            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[TareaController]";
            }
            return objetoRequest;
        }


        //---------------------------------------------------SP para actualizar Tarea---------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarTarea(int id, [FromBody] TareaUpdateDTO value)
        {
            var response = await _tareaRepository.ActualizarTarea(id, value);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[Tareacontroller]";
            }
            return objetoRequest;
        }
    }
}
