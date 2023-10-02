using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProyectoShopLog.Entity;

namespace ProyectoShopLog.BLL.Interfaces
{
    public interface IRolService
    {

        Task<List<Rol>> Lista();

    }
}
