using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DTO;
using DTO.Usuario;
using Models.Entidades;
using System.Collections.Generic;
using Data.Repositorios;
using System.Threading.Tasks;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepositorio;
        private readonly IMapper _mapper;

        public UsuariosController(UsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepository;
            _mapper = mapper;
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<UsuarioDTO>> Get()
        //{
        //    var usuarios = _usuarioRepositorio.GetUsuarios();
        //    var usuarioDTOs = _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);

        //    return Ok(usuarioDTOs);
        //}

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> ListAll()
        {
            var response = await _usuarioRepositorio.ListAll();
            if (response == null)
            {
                return NotFound();
            }

            var UsuarioDTO = _mapper.Map<List<UsuarioDTO>>(response);
            //var UsuarioDTO = mapper.Map<List<Usuario>, List<UsuarioDTO>>(response);


            return Ok(UsuarioDTO);

            //return response;
        }
    }
}
