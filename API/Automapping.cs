using AutoMapper;
using DTO;
using DTO.Proveedor;
using Models.Entidades;

namespace API
{
    public class Automapping : Profile
    {
        public Automapping() {
            CreateMap<Proveedor, ProveedorDTO>()/*.ReverseMap()*/;
        }
    }
}
