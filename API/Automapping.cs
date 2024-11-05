using AutoMapper;
using DTO;
using DTO.Tarea;
using DTO.Especialidad;
using DTO.TipoParametro;
using DTO.Parametro;
using DTO.Usuario;
using DTO.Proveedor;
using DTO.Obra;
using Models.Entidades;

namespace API
{
    public class Automapping : Profile
    {
        public Automapping() {
            //Automapero para usuarios
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();

            //Automapeo para tareas
            CreateMap<Tarea, TareaDTO>().ReverseMap();
            CreateMap<Tarea, TareaDTO>().ReverseMap();
            CreateMap<Tarea, TareaInsertDTO>().ReverseMap();
            CreateMap<Tarea, TareaUpdateDTO>().ReverseMap();

            //Automapeo para especialidad
            CreateMap<Especialidad, EspecialidadDTO>().ReverseMap();
            CreateMap<Especialidad, EspecialidadInsertDTO>().ReverseMap();
            CreateMap<Especialidad, EspecialidadUpdateDTO>().ReverseMap();
            //este es solo para mapear (id, nombre)
            CreateMap<Especialidad, LstEspecialidadDTO>().ReverseMap();

            //Automapero de tipo parametro
            CreateMap<TipoParametro, TipoParametroDTO>().ReverseMap();
            CreateMap<TipoParametro, TipoParametroInsertDTO>().ReverseMap();
            CreateMap<TipoParametro, TipoParametroUpdateDTO>().ReverseMap();
            CreateMap<TipoParametro, LstTipoParametroDTO>().ReverseMap();

            //Automapeo para Obras
            CreateMap<Obra, ObraDTO>().ReverseMap();
            CreateMap<Obra, ObraDTO>().ReverseMap();
            CreateMap<Obra, ObraInsertDTO>().ReverseMap();
            CreateMap<Obra, ObraUpdateDTO>().ReverseMap();

            //Automapeo de parametro
            CreateMap<Parametro, ParametroDTO>().ReverseMap();

            //Automapeo de provveodr
            CreateMap<Proveedor, ProveedorDTO>().ReverseMap();

        }
    }
}
