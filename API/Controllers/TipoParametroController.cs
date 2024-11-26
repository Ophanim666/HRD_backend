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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ObjetoRequest>> ListAll()
        {
            var response = await _tipoParametroRepositorio.ListAll();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay tipo parametro registrados";
                objetoRequest.Estado.ErrCon = "[TipoParametroController]";
            }

            var tipoParametroDTOs = _mapper.Map<List<TipoParametroDTO>>(response);
            objetoRequest.Body = new BodyRequest()
            {
                Response = tipoParametroDTOs
            };
            return objetoRequest;
        }

        //---------------------------------------------------------------ejecucion Lsttipoparametros---------------------------------------------------------------
        [Authorize]
        [HttpGet("LstTipoParametros")]
        public async Task<ActionResult<ObjetoRequest>> LstTipoParametro()
        {
            var response = await _tipoParametroRepositorio.LstTipoParametro();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay tipo parametro registrados";
                objetoRequest.Estado.ErrCon = "[TipoParametroController]";
            }

            var tipoParametroDTOs = _mapper.Map<List<LstTipoParametroDTO>>(response);
            objetoRequest.Body = new BodyRequest()
            {
                Response = tipoParametroDTOs
            };
            return objetoRequest;
        }

        //---------------------------------------------------------------Insertar tipoparametros---------------------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
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
                objetoRequest.Estado.ErrCon = "[TipoParametroController]";
            }
            return objetoRequest;

        }

        //---------------------------------------------------------------Actualizar tipo parametros---------------------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarTipoParametro(int id, [FromBody] TipoParametroUpdateDTO value)
        {
            var response = await _tipoParametroRepositorio.ActualizarTipoParametro(id, value);
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

        //---------------------------------------------------------------Eliminar TipoParametro por ID---------------------------------------------------------------
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> EliminarTipoParametro(int id)
        {
            var response = await _tipoParametroRepositorio.EliminarTipoParametro(id);
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
