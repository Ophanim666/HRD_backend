using AutoMapper;
using Data.Repositories;
using DTO.Parametro;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParametroController : Controller
    {
        private readonly ParametroRepository _ParametroRepositorio;
        private readonly IMapper _mapper;
        public ParametroController(ParametroRepository ParametroRepositorio, IMapper mapper)
        {
            _ParametroRepositorio = ParametroRepositorio;
            _mapper = mapper;
        }
        //obtener parametros
        [HttpGet]
        public async Task<ActionResult<List<ParametroDTO>>> ListAll()
        {
            var response = await _ParametroRepositorio.ListAll();
            if (response == null || response.Count == 0)
            {
                return NotFound();
            }

            var ParametroDTOs = _mapper.Map<List<ParametroDTO>>(response);
            return Ok(ParametroDTOs);
        }




    }
}
