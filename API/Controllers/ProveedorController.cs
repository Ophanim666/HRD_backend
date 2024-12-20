﻿using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.Proveedor;
using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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

        //----------------------------------------------------------------insertar Proveedores------------------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> InsertarProveedor([FromBody] ProveedorInsertDTO value)
        {

            // Obtener el Email del usuario logueado desde el JWT
            var usuarioCreacion = HttpContext.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;  // Extrae el 'Email' o 'ID' del usuario logueado

            if (usuarioCreacion == null)
            {
                return Unauthorized(new { message = "Usuario no autenticado" });
            }

            var responseProveedor = await _proveedorRepository.InsertarProveedor(value, usuarioCreacion);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (responseProveedor.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = responseProveedor.codErr.ToString();
                objetoRequest.Estado.ErrDes = responseProveedor.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
                return objetoRequest;
            }

            var responseEspecialidades = await _proveedorRepository.InsertarProveedorXEspecialidad(responseProveedor.proveedorId.Value, value.ListaEspecialidades);

            if (responseEspecialidades.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = responseEspecialidades.codErr.ToString();
                objetoRequest.Estado.ErrDes = responseEspecialidades.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
                return objetoRequest;
            }
            return Ok(objetoRequest);

        }

        //----------------------------------------------------------------Actualizar Proveedores--------------------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
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
                return objetoRequest;
            }

            var responseEspecialidades = await _proveedorRepository.ActualizarProveedorXEspecialidad(id, value.ListaEspecialidades);

            if (responseEspecialidades.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = responseEspecialidades.codErr.ToString();
                objetoRequest.Estado.ErrDes = responseEspecialidades.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[ProveedorController]";
                return objetoRequest;
            }

            objetoRequest.Estado.Ack = true;
            return Ok(objetoRequest);
        }

        //----------------------------------------------------------------eliminar el Proveedor por ID----------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
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

