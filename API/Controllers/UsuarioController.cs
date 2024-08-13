using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DTO.Usuario;
using Models.Entidades;
using Data.Repositorios;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> ListAll()
        {
            var response = await _usuarioRepositorio.ListAll();
            if (response == null || response.Count == 0)
            {
                return NotFound();
            }

            var usuarioDTOs = _mapper.Map<List<UsuarioDTO>>(response);
            return Ok(usuarioDTOs);
        }

        //funcion delete esta funcion quedara como softdelete que no eliminara a los usaurios de la base de datos 
        


    }
}
