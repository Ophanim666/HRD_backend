using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Repositories;
using System.Threading.Tasks;
using DTO.TipoParametro;
using AutoMapper;
using DTO;
//nuevos usings necesarios
using System.Collections.Generic;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoParametroController : ControllerBase
    {
        private readonly TipoParametroRepository _tipoParametroRepositorio;
        private readonly IMapper _mapper;

        public TipoParametroController(TipoParametroRepository tipoParametroRepositorio, IMapper mapper)
        {
            _tipoParametroRepositorio = tipoParametroRepositorio;
            _mapper = mapper;
        }
        //---------------------------------------------------------------Listar tipoparametros---------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<List<TipoParametroDTO>>> ListAll()
        {
            var response = await _tipoParametroRepositorio.ListAll();
            if (response == null || response.Count == 0)
            {
                return NotFound();
            }

            var tipoParametroDTOs = _mapper.Map<List<TipoParametroDTO>>(response);
            return Ok(tipoParametroDTOs);
        }

        //---------------------------------------------------------------Insertar tipoparametros---------------------------------------------------------------
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> InsertarTipoParametro([FromBody] TipoParametroInsertDTO value)
        {   
            var response = await _tipoParametroRepositorio.InsertarTipoParametro(value);


            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();
            //Ya no es necesario ya que se soluciono en el procedimiento almacenado
            //objetoRequest.Estado.ErrDes = response.desErr.ToString();
            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[controller]";
            }
                return objetoRequest;

        }

        //---------------------------------------------------------------Actualizar tipo parametros---------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarTipoParametro(int id, [FromBody] TipoParametroUpdateDTO value)
        {
            var response = await _tipoParametroRepositorio.ActualizarTipoParametro(value);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();
            //objetoRequest.Estado.ErrDes = response.desErr.ToString();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[controller]";
            }
            return objetoRequest;
        }

        //---------------------------------------------------------------Eliminar TipoParametro por ID---------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTipoParametro(int id)
        {
            try
            {
                var result = await _tipoParametroRepositorio.EliminarTipoParametro(id);

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
