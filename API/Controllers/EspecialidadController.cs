using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.Especialidad;
using AutoMapper;

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
        
        //listar usuarios
        [HttpGet]
        public async Task<ActionResult<List<EspecialidadDTO>>> ListAll()
        {
            var response = await _especialidadRepository.ListAll();
            if (response == null || response.Count == 0)
            {
                return NotFound();
            }

            var EspecialidadDTOs = _mapper.Map<List<EspecialidadDTO>>(response);
            return Ok(EspecialidadDTOs);
        }
    }
}