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
                destino.Monto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Monto.Value, new CultureInfo("es-PE")))
                );

            CreateMap<VMGasto, Gasto>()
                .ForMember(destino =>
                destino.Monto,
                opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Monto, new CultureInfo("es-PE")))
                );

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
            CreateMap<Usuario, VMUsuario>()
                .ForMember(destino =>
                    destino.NombreRol,
                    opt => opt.MapFrom(origen => origen.IdRolNavigation.Descripcion)
                );

            CreateMap<VMUsuario, Usuario>()
                .ForMember(destino =>
                    destino.IdRolNavigation,
                    opt => opt.Ignore()
                );
            #endregion Usuario
        }
    }
}
