// ELIMINAR ESTE CONTROLLER SI NO SE OCUPA, MEJOR BUSCAR UN METODO PARA MOVER LAS FUNCIONES DE LOG IN AL INICIO PARA QUE SE PUEDAN EJECUTAR PRIMERO

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DTO.Usuario;
using Data.Repositorios;
using Microsoft.AspNetCore.Authorization;
using DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        //
        private readonly TokenService _tokenService;

        //hay que aplicar esta logica a las demas clases construidas en repository
        public LogInController(UsuarioRepository usuarioRepository, IMapper mapper, TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            //
            _tokenService = tokenService;
        }

        //----------------------------------------------------------------Log in----------------------------------------------------

        //admin
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO loginDTO)
        {
            var (usuario, codErr, desErr) = await _usuarioRepository.ObtenerUsuarioPorEmail(loginDTO.Email, loginDTO.Password);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            // Verificar el código de error
            if (codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = codErr.ToString();
                objetoRequest.Estado.ErrDes = desErr;
                objetoRequest.Estado.ErrCon = "[LoginController]";
                return BadRequest(objetoRequest);
            }

            if (usuario == null)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "10001";
                objetoRequest.Estado.ErrDes = "Credenciales inválidas.";
                objetoRequest.Estado.ErrCon = "[LoginController]";
                return Unauthorized(objetoRequest);
            }

            // Generar el token JWT
            var token = _tokenService.GenerateJwtToken(usuario);
            return Ok(new { Token = token });
        }

        //Usuario
        [HttpPost("loginUsuario")]
        public async Task<IActionResult> LoginUsuarioNoAdmin([FromBody] UsuarioLoginDTO loginDTO)
        {
            var (usuario, codErr, desErr) = await _usuarioRepository.ObtenerUsuarioNoAdminPorEmail(loginDTO.Email, loginDTO.Password);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            // Verificar el código de error
            if (codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = codErr.ToString();
                objetoRequest.Estado.ErrDes = desErr;
                objetoRequest.Estado.ErrCon = "[LoginUsuarioNoAdminController]";
                return BadRequest(objetoRequest);
            }

            if (usuario == null)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "10001";
                objetoRequest.Estado.ErrDes = "Credenciales inválidas.";
                objetoRequest.Estado.ErrCon = "[LoginUsuarioNoAdminController]";
                return Unauthorized(objetoRequest);
            }

            // Generar el token JWT
            var token = _tokenService.GenerateJwtToken(usuario);
            return Ok(new { Token = token });
        }
    }
}
