// ELIMINAR ESTE CONTROLLER SI NO SE OCUPA, MEJOR BUSCAR UN METODO PARA MOVER LAS FUNCIONES DE LOG IN AL INICIO PARA QUE SE PUEDAN EJECUTAR PRIMERO

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DTO.Usuario;
using Data.Repositorios;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInConreoller : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        //
        private readonly TokenService _tokenService;

        //hay que aplicar esta logica a las demas clases construidas en repository
        public LogInConreoller(UsuarioRepository usuarioRepository, IMapper mapper, TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            //
            _tokenService = tokenService;
        }

        //----------------------------------------------------------------Log in----------------------------------------------------
        // Endpoint para iniciar sesión y obtener un token JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO loginDTO)
        {
            var usuario = await _usuarioRepository.ObtenerUsuarioPorEmail(loginDTO.Email, loginDTO.Password);

            if (usuario == null)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            var token = _tokenService.GenerateJwtToken(usuario);
            return Ok(new { Token = token });
        }

        //funcion de cerrar cesion, no hace nada solo retorna un mensaje xd
        [HttpPost("logout")]
        [Authorize]  // Esto asegura que solo usuarios autenticados puedan cerrar sesión
        public IActionResult Logout()
        {
            // En este caso, no necesitamos hacer nada más en el servidor.
            // El frontend simplemente eliminará el token JWT que tiene almacenado.
            return Ok("Sesión cerrada correctamente.");
        }
    }
}
