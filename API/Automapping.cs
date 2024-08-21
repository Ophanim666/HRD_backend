using AutoMapper;
using DTO;
using DTO.Tarea;
using DTO.Usuario;
using Models.Entidades;

namespace API
{
    public class Automapping : Profile
    {
        public Automapping() {
            CreateMap<Usuario, UsuarioDTO>()/*.ReverseMap()*/;
            CreateMap<Tarea, TareaDTO>()/*.ReverseMap()*/;
        }
    }
}
