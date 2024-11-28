using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.Archivo;
using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Authorization;
using DTO.Tarea;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArchivoController : ControllerBase
    {
        private readonly ArchivoRepository _archivoRepository;
        private readonly IMapper _mapper;

        public ArchivoController(ArchivoRepository archivoRepository, IMapper mapper)
        {
            _archivoRepository = archivoRepository;
            _mapper = mapper;
        }

        //---------------------------------------------------Listar Archivos---------------------------------------------------
        [Authorize]
        [HttpGet("ListarArchivos")] // se parametrizo cambiar en el frontend
        public async Task<ActionResult<ObjetoRequest>> ListAll()
        {
            var response = await _archivoRepository.ListAll();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay archivos registrados";
                objetoRequest.Estado.ErrCon = "[Archivocontroller]";
            }

            var ArchivoDTOs = _mapper.Map<List<ArchivoDTO>>(response);

            objetoRequest.Body = new BodyRequest()
            {
                Response = ArchivoDTOs
            };
            return objetoRequest;
        }

        //-------------------------------------------------------para obtener el base64 de la foto----------------------------------------
        [Authorize]
        [HttpGet("ObtenerFoto/{id}")]
        public async Task<ActionResult<ObjetoRequest>> ObtenerArchivoBase64(int id)
        {
            var archivoBase64 = await _archivoRepository.ObtenerArchivoBase64(id);

            ObjetoRequest objetoRequest = new ObjetoRequest
            {
                Estado = new EstadoRequest()
            };

            if (string.IsNullOrEmpty(archivoBase64))
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "404";
                objetoRequest.Estado.ErrDes = "Archivo no encontrado.";
                return NotFound(objetoRequest);
            }

            objetoRequest.Body = new BodyRequest
            {
                Response = archivoBase64
            };

            objetoRequest.Estado.Ack = true;
            return Ok(objetoRequest);
        }

        //-----------------------------------------------------------------archivos por gripo de tarea-------------------------------------------
        [Authorize]
        [HttpGet("ObtenerArchivosPorGrupo/{grupoTareaId}")]
        public async Task<ActionResult<ObjetoRequest>> ObtenerArchivosPorGrupo(int grupoTareaId)
        {
            var archivosBase64 = await _archivoRepository.ObtenerArchivosPorGrupoTarea(grupoTareaId);

            ObjetoRequest objetoRequest = new ObjetoRequest
            {
                Estado = new EstadoRequest()
            };

            if (archivosBase64 == null || archivosBase64.Count == 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "404";
                objetoRequest.Estado.ErrDes = "No se encontraron archivos para este grupo de tareas.";
                return NotFound(objetoRequest);
            }

            objetoRequest.Body = new BodyRequest
            {
                Response = archivosBase64 // Aquí enviamos la lista completa
            };

            objetoRequest.Estado.Ack = true;
            return Ok(objetoRequest);
        }

        //---------------------------------------------------Añadir Archivo---------------------------------------------------
        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> AñadirArchivo([FromBody] ArchivoInsertDTO value)
        {
            var response = await _archivoRepository.AñadirArchivo(value);

            ObjetoRequest objetoRequest = new ObjetoRequest
            {
                Estado = new EstadoRequest()
            };

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ArchivoController]";
            }
            else
            {
                objetoRequest.Estado.Ack = true;
            }

            return objetoRequest;
        }

        //---------------------------------------------------Eliminar Archivo---------------------------------------------------
        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> EliminarArchivo(int id)
        {
            var response = await _archivoRepository.EliminarArchivo(id);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ArchivoController]";
            }
            return objetoRequest;
        }

        //--------------------------------------------------Actualizar datos de Archivo--------------------------------------------------- Eliminar este enpoint no tiene mucho uso
        [Authorize]
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarArchivo(int id, [FromBody] ArchivoUpdateDTO value)
        {
            var response = await _archivoRepository.ActualizarArchivo(id, value);

            // Crear el objeto de respuesta estándar
            ObjetoRequest objetoRequest = new ObjetoRequest
            {
                Estado = new EstadoRequest()
            };

            if (response.codErr != 0)
            {
                // Manejo de errores en la respuesta
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ArchivoController]";
            }
            else
            {
                // Confirmación de éxito
                objetoRequest.Estado.Ack = true;
            }

            return objetoRequest;
        }
    }
}
