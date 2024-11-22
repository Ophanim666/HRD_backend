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

        //---------------------------------------------------A�adir Archivo---------------------------------------------------
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> A�adirArchivo([FromBody] ArchivoInsertDTO value)
        {
            var response = await _archivoRepository.A�adirArchivo(value);

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
        [HttpDelete("{id}")]
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

        //---------------------------------------------------SP para actualizar Tarea---------------------------------------------------
        [HttpPut("{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarArchivo(int id, [FromBody] ArchivoUpdateDTO value)
        {
            var response = await _archivoRepository.ActualizarArchivo(id, value);

            // Crear el objeto de respuesta est�ndar
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
                // Confirmaci�n de �xito
                objetoRequest.Estado.Ack = true;
            }

            return objetoRequest;
        }
    }

}
