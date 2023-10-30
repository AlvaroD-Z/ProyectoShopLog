using ProyectoShopLog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoShopLog.BLL.Interfaces
{
    public interface IGastoService
    {
        Task<List<Gasto>> Lista();
        Task<Gasto> Crear(Gasto entidad);
        Task<Gasto> Editar(Gasto entidad);
        Task<bool> Eliminar(int GastoId);
        Task<Gasto> ObtenerPorId(int GastoId);

        Task<List<Gasto>> GetIngresos(DateTime fechaInicio, DateTime fechaFin);
        Task<List<Gasto>> GetGastos(DateTime fechaInicio, DateTime fechaFin);
        Task<Balance> GetBalance(DateTime fechaInicio, DateTime fechaFin);
    }
}
