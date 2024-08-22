using AutoMapper;
using DTO;
using DTO.TipoParametro;
using DTO.Parametro;
using DTO.Usuario;
using Models.Entidades;

namespace API
{
    public class Automapping : Profile
    {
        public Automapping() {
            CreateMap<Usuario, UsuarioDTO>()/*.ReverseMap()*/;
            CreateMap<TipoParametro, TipoParametroDTO>();
            CreateMap<Parametro, ParametroDTO>();
        }
    }
}
