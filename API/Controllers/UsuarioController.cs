﻿using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DTO.Usuario;
using Models.Entidades;
using Data.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.TipoParametro;
using DTO;
using DTO.Parametro;
using Data.Repositories;
using DTO.Proveedor;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        //
        private readonly TokenService _tokenService;


        public UsuariosController(UsuarioRepository usuarioRepository, IMapper mapper, TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            //
            _tokenService = tokenService;
        }
        //---------------------------------------------------------------Listar Usuarios---------------------------------------------------------------
        [Authorize(Policy = "AdminPolicy")] //es para que solo los admins puedan ejecutar estas funciones
        [HttpGet("ListarUsuarios")]
        public async Task<ActionResult<ObjetoRequest>> ListAll()
        {
            var response = await _usuarioRepository.ListAll();
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response == null /*|| response.Count == 0*/)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = "001.01";
                objetoRequest.Estado.ErrDes = "No hay usuarios registrados";
                objetoRequest.Estado.ErrCon = "[UsuarioController]";
            }

            var UsuarioDTOs = _mapper.Map<List<UsuarioDTO>>(response);
            objetoRequest.Body = new BodyRequest()
            {
                Response = UsuarioDTOs
            };
            return objetoRequest;
        }

        //---------------------------------------------------------------Insertar Usuarios---------------------------------------------------------------
        [HttpPost("add")]
        public async Task<ActionResult<ObjetoRequest>> InsertarUsuario([FromBody] UsuarioInsertDTO value)
        {
            var response = await _usuarioRepository.InsertarUsuario(value);

            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();
            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[UsuarioController]";
            }
            return objetoRequest;
        }

        //----------------------------------------------------------------Actualizar Usuarios--------------------------------------------------------------
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> ActualizarUsuario(int id, [FromBody] UsuarioUpdateDTO value)
        {
            var response = await _usuarioRepository.ActualizarUsuario(id, value);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[UsuarioController]";
                return BadRequest(objetoRequest);
            }

            objetoRequest.Estado.Ack = true;
            return Ok(objetoRequest);
        }

        //----------------------------------------------------------------eliminar el USuario por ID----------------------------------------------------
        [Authorize(Policy = "AdminPolicy")] // Solo los administradores pueden eliminar usuarios
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult<ObjetoRequest>> EliminarUsuario(int id)
        {
            var response = await _usuarioRepository.EliminarUsuario(id);
            ObjetoRequest objetoRequest = new ObjetoRequest();
            objetoRequest.Estado = new EstadoRequest();

            if (response.codErr != 0)
            {
                objetoRequest.Estado.Ack = false;
                objetoRequest.Estado.ErrNo = response.codErr.ToString();
                objetoRequest.Estado.ErrDes = response.desErr.ToString();
                objetoRequest.Estado.ErrCon = "[UsuarioController]";
            }
            return objetoRequest;
        }


        //----------------------------------------------------------------eliminar el USuario por ID----------------------------------------------------
        // Endpoint para iniciar sesión y obtener un token JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO loginDTO)
        {
            var usuario = await _usuarioRepository.ObtenerUsuarioPorEmail(loginDTO.Email, loginDTO.Password);

            if (usuario == null)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            var token = _tokenService.GenerateJwtToken(usuario);
            return Ok(new { Token = token });
        }


    }
}
