using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.Especialidad;
using AutoMapper;
using DTO;

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

        //---------------------------------------------------Listar especialidades---------------------------------------------------
        [HttpGet("ListarEspecialidades")] // se parametrizo cambiar en el frontend
        public async Task<ActionResult<ObjetoRequest>> ListAll()
        {
            var response = await _especialidadRepository.ListAll();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay especialidades registradas";
                objetoRequest.Estado.ErrCon = "[Especialidadcontroller]";
            }

            var EspecialidadDTOs = _mapper.Map<List<EspecialidadDTO>>(response);

            objetoRequest.Body = new BodyRequest()
            {
                Response = EspecialidadDTOs
            };
            return objetoRequest;
        }

        //---------------------------------------------------Listar especialidades simple (nombre, id) para listarlos y asignar a proveedores---------------------------------------------------
        [HttpGet("ListadoDeespecialidadesSimple")]
        public async Task<ActionResult<ObjetoRequest>> LstEspecialidad()
        {
            var response = await _especialidadRepository.LstEspecialidad();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay especialidades registradas";
                objetoRequest.Estado.ErrCon = "[Especialidadcontroller]";
            }

            var EspecialidadDTOs = _mapper.Map<List<LstEspecialidadDTO>>(response);

            objetoRequest.Body = new BodyRequest()
            {
                Response = EspecialidadDTOs
            };
            return objetoRequest;
        }

        //---------------------------------------------------Añadir especialidades---------------------------------------------------
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> AñadirEspecialidad([FromBody] EspecialidadInsertDTO value)
        {
            var response = await _especialidadRepository.AñadirEspecialidad(value);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[Especialidadcontroller]";
            }
            return objetoRequest;
        }

        //---------------------------------------------------Eliminar especialidades---------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<ActionResult<ObjetoRequest>> EliminarEspecialidad(int id)
        {
            var response = await _especialidadRepository.EliminarEspecialidad(id);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();


            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[EspecialidadController]";
            }
            return objetoRequest;
        }


        //---------------------------------------------------SP para actualizar especialidad---------------------------------------------------
        [HttpPut("{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarEspecialidad(int id, [FromBody] EspecialidadUpdateDTO value)
        {
            var response = await _especialidadRepository.ActualizarEspecialidad(id, value);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[Especialidadcontroller]";
            }
            return objetoRequest;
        }
    }
}
    

