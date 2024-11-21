using AutoMapper;
using Data.Repositories;
using DTO.Proveedor;
using DTO;
using Microsoft.AspNetCore.Mvc;
using DTO.GrupoTareas;
using DTO.GrupoTareasXTareaDTO;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrupoTareaController : Controller
    {
        private readonly GrupoTareasRepository _grupoTareasRepository;
        private readonly IMapper _mapper;
        public GrupoTareaController(GrupoTareasRepository grupoTareasRepository, IMapper mapper)
        {
            _grupoTareasRepository = grupoTareasRepository;
            _mapper = mapper;
        }

        //----------------------------------------------------------------listar grupo tarea----------------------------------------------------------------
        //[Authorize(Policy = "AdminPolicy")]
        [HttpGet("ListarGrupoTareas")]
        public async Task<ActionResult<ObjetoRequest>> ListAll()
        {
            var response = await _grupoTareasRepository.ListAll();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay grupo tarea registrados";
                objetoRequest.Estado.ErrCon = "[GrupoTareaController]";
            }

            var grupoTareaDTOs = _mapper.Map<List<GrupoTareasDTO>>(response);
            objetoRequest.Body = new BodyRequest()
            {
                Response = grupoTareaDTOs
            };
            return objetoRequest;
        }

        //---------------------------------------------------------------listadoTesting...............................................................................NEW
        //[Authorize(Policy = "AdminPolicy")]
        [HttpGet("Listado")]
        public async Task<ActionResult<ObjetoRequest>> ListAllGListAllGrupoTareaxTareas()
        {
            var response = await _grupoTareasRepository.ListAllGrupoTareaxTareas();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null || response.Count == 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay grupos registrados con tareas";
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
            }
            else
            {
                objetoRequest.Estado.Ack = true;
            }

            var grupoTareaDTOs = _mapper.Map<List<ListarTareasXGrupoTareasDTO>>(response);
            objetoRequest.Body = new BodyRequest()
            {
                Response = grupoTareaDTOs
            };
            return objetoRequest;
        }


        //----------------------------------------------------------------listar proveedores con especialidades Por ID---------------------------------------------------
        //[Authorize(Policy = "AdminPolicy")]
        //[HttpGet("con-especialidades/{id}")]
        //public async Task<ActionResult<ObjetoRequest>> ListarProveedoresConEspecialidades(int id)
        //{
        //    var response = await _proveedorRepository.ListarProveedoresConEspecialidades(id);
        //    ObjetoRequest objetoRequest = new ObjetoRequest();
        //    objetoRequest.Estado = new EstadoRequest();

        //    if (response == null || response.Count == 0)
        //    {
        //        objetoRequest.Estado.Ack = false;
        //        objetoRequest.Estado.ErrNo = "001.01";
        //        objetoRequest.Estado.ErrDes = "No hay proveedores con especialidades registrados";
        //        objetoRequest.Estado.ErrCon = "[ProveedorController]";
        //        return NotFound(objetoRequest);
        //    }

        //    objetoRequest.Body = new BodyRequest()
        //    {
        //        Response = response
        //    };

        //    return Ok(objetoRequest);
        //}

        //----------------------------------------------------------Listar proveedores con sus especialidades por GENERAL PARA LISTAR Y COMPROBAR------------------
        //[Authorize(Policy = "AdminPolicy")]
        //[HttpGet("ObtenerProveedorConEspecialidadGeneral")] //preguntar cual seria la mejor forma de parametrizar las rutas, ejemplo [Route("Lista")] o [HttpGet("Lista")]
        //public async Task<ActionResult<ObjetoRequest>> ObtenerProveedoresEspecialidadesGeneral()
        //{
        //    var response = await _proveedorRepository.ObtenerProveedoresEspecialidadesGeneral();
        //    ObjetoRequest objetoRequest = new ObjetoRequest();
        //    objetoRequest.Estado = new EstadoRequest();

        //    if (response == null || response.Count == 0)
        //    {
        //        objetoRequest.Estado.Ack = false;
        //        objetoRequest.Estado.ErrNo = "001.01";
        //        objetoRequest.Estado.ErrDes = "No hay proveedores con especialidades registrados";
        //        objetoRequest.Estado.ErrCon = "[ProveedorController]";
        //        return NotFound(objetoRequest);
        //    }

        //    objetoRequest.Body = new BodyRequest()
        //    {
        //        Response = response
        //    };

        //    return Ok(objetoRequest);
        //}

        //----------------------------------------------------------------insertar Grupo Tareas------------------------------------------------------------
        //[Authorize(Policy = "AdminPolicy")]
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> InsertarGrupoTarea([FromBody] GrupoTareasInsertDTO value)
        {
            var responseGrupoTarea = await _grupoTareasRepository.InsertarGrupoTarea(value);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (responseGrupoTarea.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = responseGrupoTarea.codErr.ToString();
                objetoRequest.Estado.ErrDes = responseGrupoTarea.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[GrupoTareaController]";
                return objetoRequest;
            }

            var responseTareas = await _grupoTareasRepository.InsertarTareasXGrupoTarea(responseGrupoTarea.grupoTareaId.Value, value.ListaTareas);

            if (responseTareas.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = responseTareas.codErr.ToString();
                objetoRequest.Estado.ErrDes = responseTareas.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[GrupoTareaController]";
                return objetoRequest;
            }
            return Ok(objetoRequest);

        }

        //----------------------------------------------------------------Actualizar GRupo TAreas--------------------------------------------------------------
        //[Authorize(Policy = "AdminPolicy")]
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarGrupoTarea(int id, [FromBody] GrupoTareasUpdateDTO value)
        {
            var response = await _grupoTareasRepository.ActualizarGrupoTarea(id, value);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[GrupoTareaController]";
                return objetoRequest;
            }

            var responseTareas = await _grupoTareasRepository.ActualizarTareasXGrupoTarea(id, value.ListaTareas);

            if (responseTareas.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = responseTareas.codErr.ToString();
                objetoRequest.Estado.ErrDes = responseTareas.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[GrupoTareaController]";
                return objetoRequest;
            }

            objetoRequest.Estado.Ack = true;
            return Ok(objetoRequest);
        }

        //----------------------------------------------------------------eliminar el grupo tarea por ID----------------------------------------------------
        //[Authorize(Policy = "AdminPolicy")]
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> EliminarGrupoTarea(int id)
        {
            var response = await _grupoTareasRepository.EliminarGrupoTarea(id);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();
            //objetoRequest.Estado.ErrDes = response.desErr.ToString();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[GrupoTareaController]";
            }
            return objetoRequest;
        }

        //cambiar estado grupo tareas por tarea:
        [HttpPut("ActualizarEstadoTarea/{grupoTareaId}/{tareaId}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarEstadoTareaEnGrupo(int grupoTareaId, int tareaId, [FromBody] GrupoTareasXTareaUpdateDTO value)
        {
            var response = await _grupoTareasRepository.ActualizarEstadoTareaEnGrupo(grupoTareaId, tareaId, value.estado);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr;
                objetoRequest.Estado.ErrCon = "[TareaController]";
                return BadRequest(objetoRequest);
            }

            objetoRequest.Estado.Ack = true;
            return Ok(objetoRequest);
        }

    }
}
