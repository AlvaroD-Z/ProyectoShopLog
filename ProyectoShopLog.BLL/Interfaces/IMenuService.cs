using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProyectoShopLog.Entity;

namespace ProyectoShopLog.BLL.Interfaces
{
    public interface IMenuService
    {
        Task<List<Menu>> ObetenerMenus(int idUsuario);
        Task<bool> TienePermisoMenu(int idUsuario, string controlador, string accion);
    }
}
