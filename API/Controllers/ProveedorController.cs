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

        //---------------------------------------------------------------listadoTesting...............................................................................NEW
        [HttpGet("Listado")]
        public async Task<ActionResult<ObjetoRequest>> ListAllProveedoresConEspecialidades()
        {
            var response = await _proveedorRepository.ListAllProveedoresConEspecialidades();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null || response.Count == 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay proveedores registrados con especialidades";
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
            }
            else
            {
                objetoRequest.Estado.Ack = true;
            }

            var proveedorDTOs = _mapper.Map<List<ListarProveedoresXEspecialidadesDTO>>(response);
            objetoRequest.Body = new BodyRequest()
            {
                Response = proveedorDTOs
            };
            return objetoRequest;
        }


        //----------------------------------------------------------------listar proveedores con especialidades Por ID---------------------------------------------------
        [HttpGet("con-especialidades/{id}")]
        public async Task<ActionResult<ObjetoRequest>> ListarProveedoresConEspecialidades(int id)
        {
            var response = await _proveedorRepository.ListarProveedoresConEspecialidades(id);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null || response.Count == 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay proveedores con especialidades registrados";
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
                return NotFound(objetoRequest);
            }

            objetoRequest.Body = new BodyRequest()
            {
                Response = response
            };

            return Ok(objetoRequest);
        }

        //----------------------------------------------------------Listar proveedores con sus especialidades por GENERAL PARA LISTAR Y COMPROBAR------------------
        [HttpGet("ObtenerProveedorConEspecialidadGeneral")] //preguntar cual seria la mejor forma de parametrizar las rutas, ejemplo [Route("Lista")] o [HttpGet("Lista")]
        public async Task<ActionResult<ObjetoRequest>> ObtenerProveedoresEspecialidadesGeneral()
        {
            var response = await _proveedorRepository.ObtenerProveedoresEspecialidadesGeneral();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null || response.Count == 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay proveedores con especialidades registrados";
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
                return NotFound(objetoRequest);
            }

            objetoRequest.Body = new BodyRequest()
            {
                Response = response
            };

            return Ok(objetoRequest);
        }

        //----------------------------------------------------------------insertar Proveedores------------------------------------------------------------
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> InsertarProveedor([FromBody] ProveedorInsertDTO value)
        {
            var responseProveedor = await _proveedorRepository.InsertarProveedor(value);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (responseProveedor.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = responseProveedor.codErr.ToString();
                objetoRequest.Estado.ErrDes = responseProveedor.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
                return BadRequest(objetoRequest);
            }

            var responseEspecialidades = await _proveedorRepository.InsertarProveedorXEspecialidad(responseProveedor.proveedorId.Value, value.ListaEspecialidades);

            if (responseEspecialidades.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = responseEspecialidades.codErr.ToString();
                objetoRequest.Estado.ErrDes = responseEspecialidades.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
                return BadRequest(objetoRequest);
            }
            return Ok(objetoRequest);

        }

        //----------------------------------------------------------------Actualizar Proveedores--------------------------------------------------------------
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarProveedor(int id, [FromBody] ProveedorUpdateDTO value)
        {
            var response = await _proveedorRepository.ActualizarProveedor(id, value);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
                return BadRequest(objetoRequest);
            }

            var responseEspecialidades = await _proveedorRepository.ActualizarProveedorXEspecialidad(id, value.ListaEspecialidades);

            if (responseEspecialidades.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = responseEspecialidades.codErr.ToString();
                objetoRequest.Estado.ErrDes = responseEspecialidades.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
                return BadRequest(objetoRequest);
            }

            objetoRequest.Estado.Ack = true;
            return Ok(objetoRequest);
        }

        //----------------------------------------------------------------eliminar el Proveedor por ID----------------------------------------------------
        [HttpDelete("Eliminar/{id}")]
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
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
            }
            return objetoRequest;
        }
    }
}

//realizada la fusion de main a esta rama