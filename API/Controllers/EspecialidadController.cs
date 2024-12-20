using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.Especialidad;
using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        [Authorize]
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
        [Authorize]
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

        //---------------------------------------------------Aņadir especialidades---------------------------------------------------
        [HttpPost("add")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<ObjetoRequest>> AņadirEspecialidad([FromBody] EspecialidadInsertDTO value)
        {
            // Obtener el Email del usuario logueado desde el JWT
            var usuarioCreacion = HttpContext.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;  // Extrae el 'Email' o 'ID' del usuario logueado

            if (usuarioCreacion == null)
            {
                return Unauthorized(new { message = "Usuario no autenticado" });
            }
            var response = await _especialidadRepository.AņadirEspecialidad(value, usuarioCreacion);

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
        [HttpDelete("Eliminar/{id}")]
        [Authorize(Policy = "AdminPolicy")]
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
        [HttpPut("Actualizar/{id}")]
        [Authorize(Policy = "AdminPolicy")]
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
    

