using AutoMapper;
using Data.Repositories;
using DTO.Acta;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActaController : Controller
    {
        private readonly ActaRepository _actaRepositorio;
        private readonly IMapper _mapper;
        //
        private readonly TokenService _tokenService;
        public ActaController(ActaRepository actaRepositorio, IMapper mapper, TokenService tokenService)
        {
            _actaRepositorio = actaRepositorio;
            _mapper = mapper;
            _tokenService = tokenService;
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

        //funcion acta con usuario
        [HttpGet("user-actas")]
        public async Task<ActionResult<ObjetoRequest>> ObtenerActasPorUsuario()
        {
            // Obtener el ID del usuario logueado desde el JWT
            var usuarioId = HttpContext.User?.Claims
                .FirstOrDefault(c => c.Type == "user_id")?.Value;

            // Agregar un WriteLine para depuración
            Console.WriteLine($"ID del usuario autenticado: {usuarioId}");

            // Validar si el usuario ID fue obtenido correctamente
            if (string.IsNullOrEmpty(usuarioId) || !int.TryParse(usuarioId, out int id))
            {
                return Unauthorized(new { message = "Usuario no autenticado o ID inválido" });
            }

            // Llamar al repositorio para obtener las actas del usuario usando el ID
            var actas = await _actaRepositorio.ObtenerActasPorUsuario(id);

            // Crear el objeto de respuesta que sigue el formato definido
            ObjetoRequest objetoRequest = new ObjetoRequest
            {
                Estado = new EstadoRequest()
            };

            if (actas == null || !actas.Any())
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No se encontraron actas para el usuario";
                objetoRequest.Estado.ErrCon = "[ActaController]";
                return NotFound(objetoRequest);
            }

            // Agregar las actas al cuerpo de la respuesta
            objetoRequest.Body = new BodyRequest()
            {
                Response = actas
            };

            // Retornar las actas en formato de respuesta exitosa
            return Ok(objetoRequest);
        }


    }
}
