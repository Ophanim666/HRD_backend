using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.Obra;
using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObraController : ControllerBase
    {
        private readonly ObraRepository _obraRepository;
        private readonly IMapper _mapper;

        public ObraController(ObraRepository obraRepository, IMapper mapper)
        {
            _obraRepository = obraRepository;
            _mapper = mapper;
        }

        //---------------------------------------------------Listar obras---------------------------------------------------
        [Authorize]
        [HttpGet("ObtenerObras")] // se parametrizo cambiar en el frontend
        public async Task<ActionResult<ObjetoRequest>> ListAll()
        {
            var response = await _obraRepository.ListAll();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay obras registradas";
                objetoRequest.Estado.ErrCon = "[ObraController]";
            }

            var ObraDTOs = _mapper.Map<List<ObraDTO>>(response);

            objetoRequest.Body = new BodyRequest()
            {
                Response = ObraDTOs
            };
            return objetoRequest;
        }

        //---------------------------------------------------Listar obras simple (nombre, id) para listarlas---------------------------------------------------
        //[HttpGet("ListadoDeObrasSimple")]
        //public async Task<ActionResult<ObjetoRequest>> LstObra()
        //{
        //    var response = await _obraRepository.LstObra();
        //    ObjetoRequest objetoRequest = new ObjetoRequest();
        //    objetoRequest.Estado = new EstadoRequest();

        //    if (response == null /*|| response.Count == 0*/)
        //    {
        //        objetoRequest.Estado.Ack = false;
        //        objetoRequest.Estado.ErrNo = "001.01";
        //        objetoRequest.Estado.ErrDes = "No hay obras registradas";
        //        objetoRequest.Estado.ErrCon = "[ObraController]";
        //    }

        //    var ObraDTOs = _mapper.Map<List<LstObraDTO>>(response);

        //    objetoRequest.Body = new BodyRequest()
        //    {
        //        Response = ObraDTOs
        //    };
        //    return objetoRequest;
        //}

        //---------------------------------------------------Añadir obras---------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> AñadirObra([FromBody] ObraInsertDTO value)
        {
            // Obtener el Email del usuario logueado desde el JWT
            var usuarioCreacion = HttpContext.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;  // Extrae el 'Email' o 'ID' del usuario logueado

            if (usuarioCreacion == null)
            {
                return Unauthorized(new { message = "Usuario no autenticado" });
            }

            var response = await _obraRepository.AñadirObra(value, usuarioCreacion);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ObraController]";
            }
            return objetoRequest;
        }

        //---------------------------------------------------Eliminar obras---------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> EliminarObra(int id)
        {
            var response = await _obraRepository.EliminarObra(id);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ObraController]";
            }
            return objetoRequest;
        }

        //---------------------------------------------------Actualizar obras---------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarObra(int id, [FromBody] ObraUpdateDTO value)
        {
            var response = await _obraRepository.ActualizarObra(id, value);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ObraController]";
            }
            return objetoRequest;
        }
    }
}

