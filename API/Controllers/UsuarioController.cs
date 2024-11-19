using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DTO.Usuario;
using Data.Repositorios;
using DTO;
using Microsoft.AspNetCore.Authorization;
// Importar dependencias necesarias
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        //
        private readonly TokenService _tokenService;

        //hay que aplicar esta logica a las demas clases construidas en repository
        public UsuariosController(UsuarioRepository usuarioRepository, IMapper mapper, TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            //
            _tokenService = tokenService;
        }
        //---------------------------------------------------------------Listar Usuarios---------------------------------------------------------------
        //[Authorize(Policy = "AdminPolicy")] //es para que solo los admins puedan ejecutar estas funciones
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

        //---------------------------------------------------------------Insertar Usuarios--------------------------------------------------------------- Aqui se hicieorn cambios para obtener el usuario creacion
        //[Authorize(Policy = "AdminPolicy")]
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> InsertarUsuario([FromBody] UsuarioInsertDTO value)
        {
            // Obtener el Email del usuario logueado desde el JWT
            var usuarioCreacion = HttpContext.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;  // Extrae el 'Email' o 'ID' del usuario logueado

            if (usuarioCreacion == null)
            {
                return Unauthorized(new { message = "Usuario no autenticado" });
            }

            // Llamar al repositorio para insertar el usuario, pasando el 'usuarioCreacion'
            var response = await _usuarioRepository.InsertarUsuario(value, usuarioCreacion);

            // Preparar la respuesta de la API
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();
            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[UsuarioController]";
            }
            else
            {
                objetoRequest.Estado.Ack = true;
            }

            return objetoRequest;
        }
        // en si casi toda la logica se hace en el controller y el insert qeu vemos aqui a demas de el tokenservice 

        //----------------------------------------------------------------Actualizar Usuarios--------------------------------------------------------------
        //[Authorize(Policy = "AdminPolicy")]
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
        //[Authorize(Policy = "AdminPolicy")] // Solo los administradores pueden eliminar usuarios
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


        //----------------------------------------------------------------Log in----------------------------------------------------
        // Endpoint para iniciar sesión y obtener un token JWT
        //ESTAS FUNCIONES E MOVIERON A LOG IN CONTROLLER
        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO loginDTO)
        //{
        //    var usuario = await _usuarioRepository.ObtenerUsuarioPorEmail(loginDTO.Email, loginDTO.Password);

        //    if (usuario == null)
        //    {
        //        return Unauthorized("Credenciales inválidas.");
        //    }

        //    var token = _tokenService.GenerateJwtToken(usuario);
        //    return Ok(new { Token = token });
        //}

        ////funcion de cerrar cesion, no hace nada solo retorna un mensaje xd
        //[HttpPost("logout")]
        //[Authorize]  // Esto asegura que solo usuarios autenticados puedan cerrar sesión
        //public IActionResult Logout()
        //{
        //    // En este caso, no necesitamos hacer nada más en el servidor.
        //    // El frontend simplemente eliminará el token JWT que tiene almacenado.

        //    return Ok("Sesión cerrada correctamente.");
        //}

    }
}
