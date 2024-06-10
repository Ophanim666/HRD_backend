using AutoMapper;
using DTO;
using DTO.Usuario;
using Models.Entidades;

namespace API
{
    public class Automapping : Profile
    {
        public Automapping() {
            CreateMap<Usuario, UsuarioDTO>()/*.ReverseMap()*/;
        }
    }
}
