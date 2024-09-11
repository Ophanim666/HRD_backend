using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.Proveedor;
using AutoMapper;
using DTO;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly ProveedorRepository _proveedorRepository;
        private readonly IMapper _mapper;

        public ProveedorController(ProveedorRepository proveedorRepository, IMapper mapper)
        {
            _proveedorRepository = proveedorRepository;
            _mapper = mapper;
        }

        //----------------------------------------------------------------listar proveedor----------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<ObjetoRequest>> ListAll()
        {
            var response = await _proveedorRepository.ListAll();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay proveedor registrados";
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
            }

            var proveedorDTOs = _mapper.Map<List<ProveedorDTO>>(response);
            objetoRequest.Body = new BodyRequest()
            {
                Response = proveedorDTOs
            };
            return objetoRequest;
        }

        //-----------------------------------------------------------------Listar proveedor por ID--------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorDTO>> ListarPorIdProveedor(int id)
        {
            var response = await _proveedorRepository.ListarPorIdProveedor(id);
            if (response == null)
            {
                return NotFound();
            }

            var proveedorDTO = _mapper.Map<ProveedorDTO>(response);
            return Ok(proveedorDTO);
        }

        //----------------------------------------------------------------insertar Proveedores------------------------------------------------------------
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> InsertarProveedor([FromBody] ProveedorInsertDTO value)
        {
            var response = await _proveedorRepository.InsertarProveedor(value);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();
            //Ya no es necesario ya que se soluciono en el procedimiento almacenado
            //objetoRequest.Estado.ErrDes = response.desErr.ToString();
            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[TipoParametroController]";
            }
            return objetoRequest;

        }

        //----------------------------------------------------------------editar Proveedores--------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarProveedor(int id, [FromBody] ProveedorUpdateDTO value)
        {
            var response = await _proveedorRepository.ActualizarProveedor(id, value);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();
            //objetoRequest.Estado.ErrDes = response.desErr.ToString();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
            }
            return objetoRequest;
        }

        //----------------------------------------------------------------eliminar el Proveedor por ID----------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<ActionResult<ObjetoRequest>> EliminarProveedor(int id)
        {
            var response = await _proveedorRepository.EliminarProveedor(id);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();
            //objetoRequest.Estado.ErrDes = response.desErr.ToString();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[TipoParametroController]";
            }
            return objetoRequest;
        }
    }
}