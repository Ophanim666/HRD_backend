using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.Proveedor;
using AutoMapper;

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
        //listar proveedor
        [HttpGet]
        public async Task<ActionResult<List<ProveedorDTO>>> ListAll()
        {
            var response = await _proveedorRepository.ListAll();
            if (response == null || response.Count == 0)
            {
                return NotFound();
            }

            var proveedorDTO = _mapper.Map<List<ProveedorDTO>>(response);
            return Ok(proveedorDTO);
        }
        //Listar proveedor por ID ---------------------------------------ELIMINAR SI NO FUNCIONA-----------------------------------------
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
        //insertar Proveedores
        [HttpPost]
        public async Task<IActionResult> InsertarTipoParametro([FromBody] ProveedorDTO proveedorDTO)
        {
            if (proveedorDTO == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var proveedor = new Proveedor
            {
                NOMBRE = proveedorDTO.NOMBRE,
                RAZON_SOCIAL = proveedorDTO.RAZON_SOCIAL,
                RUT = proveedorDTO.RUT,
                DV = proveedorDTO.DV,
                NOMBRE_CONTACTO_PRINCIPAL = proveedorDTO.NOMBRE_CONTACTO_PRINCIPAL,
                NUMERO_CONTACTO_PRINCIPAL = proveedorDTO.NUMERO_CONTACTO_PRINCIPAL,
                NOMBRE_CONTACTO_SECUNDARIO = proveedorDTO.NOMBRE_CONTACTO_SECUNDARIO,
                NUMERO_CONTACTO_SECUNDARIO = proveedorDTO.NUMERO_CONTACTO_SECUNDARIO,
                ESTADO = proveedorDTO.ESTADO,
                USUARIO_CREACION = proveedorDTO.USUARIO_CREACION,
                FECHA_CREACION = proveedorDTO.FECHA_CREACION
            };

            try
            {
                var result = await _proveedorRepository.InsertarProveedor(proveedor);

                if (result > 0)
                {
                    return Ok("Proveedor insertado correctamente.");
                }
                else
                {
                    return StatusCode(500, "Error al insertar el Proveedor.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //editar Proveedores
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProveedor(int id, [FromBody] ProveedorDTO proveedorDTO)
        {
            if (proveedorDTO == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var proveedor = new Proveedor
            {
                ID = id,
                NOMBRE = proveedorDTO.NOMBRE,
                RAZON_SOCIAL = proveedorDTO.RAZON_SOCIAL,
                RUT = proveedorDTO.RUT,
                DV = proveedorDTO.DV,
                NOMBRE_CONTACTO_PRINCIPAL = proveedorDTO.NOMBRE_CONTACTO_PRINCIPAL,
                NUMERO_CONTACTO_PRINCIPAL = proveedorDTO.NUMERO_CONTACTO_PRINCIPAL,
                NOMBRE_CONTACTO_SECUNDARIO = proveedorDTO.NOMBRE_CONTACTO_SECUNDARIO,
                NUMERO_CONTACTO_SECUNDARIO = proveedorDTO.NUMERO_CONTACTO_SECUNDARIO,
                ESTADO = proveedorDTO.ESTADO,
                USUARIO_CREACION = proveedorDTO.USUARIO_CREACION,
                FECHA_CREACION = proveedorDTO.FECHA_CREACION

            };

            try
            {
                var response = await _proveedorRepository.ActualizarProveedor(proveedor);

                if (response > 0)
                {
                    return Ok("Proveedor actualizado correctamente.");
                }
                else
                {
                    return BadRequest("Error al actualizar el Proveedor.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Método para eliminar el Proveedor por ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProveedor(int id)
        {
            try
            {
                var result = await _proveedorRepository.EliminarProveedor(id);

                if (result != 0)
                {
                    return Ok("TipoParametro eliminado correctamente." + result);
                }
                else
                {
                    return NotFound("TipoParametro no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}