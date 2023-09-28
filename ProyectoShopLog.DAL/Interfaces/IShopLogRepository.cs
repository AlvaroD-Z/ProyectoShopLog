using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProyectoShopLog.Entity;

namespace ProyectoShopLog.DAL.Interfaces
{
    public interface IShopLogRepository: IGenericRepository<Gasto>
    {
        Task<Gasto> Registrar(Gasto entidad);
        Task<List<Gasto>> Reporte(DateTime FechaIngreso);
    }
}
