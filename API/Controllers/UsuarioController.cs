using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DTO.Usuario;
using Models.Entidades;
using Data.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.TipoParametro;
using DTO;
using DTO.Parametro;
using Data.Repositories;
using DTO.Proveedor;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuariosController(UsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }
        //---------------------------------------------------------------Listar Parametro---------------------------------------------------------------
        [HttpGet("ListarUsuarios")]
        public async Task<ActionResult<ObjetoRequest>> ListAll()
        {
            var response = await _usuarioRepository.ListAll();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay usuarios registrados";
                objetoRequest.Estado.ErrCon = "[UsuarioController]";
            }

            var UsuarioDTOs = _mapper.Map<List<UsuarioDTO>>(response);
            objetoRequest.Body = new BodyRequest()
            {
                Response = UsuarioDTOs
            };
            return objetoRequest;
        }

        //---------------------------------------------------------------Insertar Usuarios---------------------------------------------------------------
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> InsertarUsuario([FromBody] UsuarioInsertDTO value)
        {
            var response = await _usuarioRepository.InsertarUsuario(value);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();
            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[UsuarioController]";
            }
            return objetoRequest;
        }

        //----------------------------------------------------------------Actualizar Proveedores--------------------------------------------------------------
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarUsuario(int id, [FromBody] UsuarioUpdateDTO value)
        {
            var response = await _usuarioRepository.ActualizarUsuario(id, value);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[UsuarioController]";
                return BadRequest(objetoRequest);
            }

            objetoRequest.Estado.Ack = true;
            return Ok(objetoRequest);
        }

        //----------------------------------------------------------------eliminar el USuario por ID----------------------------------------------------
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> EliminarUsuario(int id)
        {
            var response = await _usuarioRepository.EliminarUsuario(id);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[UsuarioController]";
            }
            return objetoRequest;
        }

    }
}
