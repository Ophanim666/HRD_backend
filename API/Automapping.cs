using AutoMapper;
using DTO;
using DTO.Especialidad;
using DTO.TipoParametro;
using DTO.Parametro;
using DTO.Usuario;
using DTO.Proveedor;
using Models.Entidades;

namespace API
{
    public class Automapping : Profile
    {
        public Automapping() {
            CreateMap<Usuario, UsuarioDTO>()/*.ReverseMap()*/;
            CreateMap<Especialidad, EspecialidadDTO>().ReverseMap();
            CreateMap<TipoParametro, TipoParametroDTO>();
            CreateMap<Parametro, ParametroDTO>();
            CreateMap<Proveedor, ProveedorDTO>()/*.ReverseMap()*/;
        }
    }
}
