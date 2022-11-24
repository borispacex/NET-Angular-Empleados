using AutoMapper;
using BackEndApi_minimal.Data.Models;
using BackEndApi_minimal.DTOs;
using System.Globalization;

namespace BackEndApi_minimal.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Departamento
            CreateMap<Departamento, DepartamentoDTO>().ReverseMap();
            #endregion

            #region Empleado
            CreateMap<Empleado, EmpleadoDTO>()
                .ForMember(destino => destino.NombreDepartamento, opt => opt.MapFrom(origen => origen.IdDepartamentoNavigation.Nombre))
                .ForMember(destino => destino.FechaContrato, 
                    opt => opt.MapFrom(origen => origen.FechaContrato.Value.ToString("dd/MM/yyyy")));
            CreateMap<EmpleadoDTO, Empleado>()
                .ForMember(destino => destino.IdDepartamentoNavigation, opt => opt.Ignore())
                .ForMember(destino => destino.FechaContrato, 
                    opt => opt.MapFrom(origin => DateTime.ParseExact(origin.FechaContrato,"dd/MM/yyyy", CultureInfo.InvariantCulture)));

            #endregion
        }
    }
}
