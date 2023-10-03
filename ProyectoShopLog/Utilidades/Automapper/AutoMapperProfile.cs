using ProyectoShopLog.AplicacionWeb.Models.ViewModels;
using ProyectoShopLog.Entity;
using System.Globalization;
using AutoMapper;

namespace ProyectoShopLog.AplicacionWeb.Utilidades.Automapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, VMRol>().ReverseMap();
            #endregion Rol
            #region Gasto
            CreateMap<Gasto, VMGasto>()
                .ForMember(destino =>
                destino.Nombre,
                opt => opt.MapFrom(origen => origen.Nombre)
                )
                .ForMember(destino =>
                destino.Monto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Monto.Value, new CultureInfo("es-PE")))
                ).ForMember(destino =>
                destino.Descripcion,
                opt => opt.MapFrom(origen => origen.Descripcion));

            CreateMap<VMGasto, Gasto>()
                .ForMember(destino =>
                destino.Nombre,
                opt => opt.Ignore()
                )
                .ForMember(destino =>
                destino.Monto,
                opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Monto, new CultureInfo("es-PE")))
                ).ForMember(destino =>
                destino.Descripcion,
                opt => opt.Ignore())
                .ForMember(destino =>
                destino.FechaDeIngreso,
                opt => opt.MapFrom(origen => origen.FechaDeIngreso.Value.ToString("dd/MM/yyyy")));

            #endregion
            #region HistorialComentario
            CreateMap<Historialcomentario, VMHistorialcomentario>().ReverseMap();
            #endregion
            #region HistorialGastoMensual
            CreateMap<Historialgastomensual, VMHistorialgastomensual>().ReverseMap();
            #endregion
            #region Menu
            CreateMap<Menu, VMMenu>()
                .ForMember(destino =>
                destino.SubMenus,
                opt => opt.MapFrom(origen => origen.InverseIdMenuPadreNavigation));
            #endregion
            #region Usuario
            CreateMap<Usuario, VMUsuario>().ReverseMap();
            #endregion Usuario
        }
    }
}
