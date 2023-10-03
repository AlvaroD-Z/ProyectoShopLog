using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using ProyectoShopLog.DAL.DBContext;
using ProyectoShopLog.DAL.Interfaces;
using ProyectoShopLog.Entity;

namespace ProyectoShopLog.DAL.Implementacion
{
    public class ShopLogRepository : GenericRepository<Gasto>, IShopLogRepository
    {

        private readonly DbShoplogContext _dbContext;

        public ShopLogRepository(DbShoplogContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Gasto> Registrar(Gasto entidad)
        {
            Gasto gastoGenerado = new Gasto();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                
                try
                {
                    Gasto dv = entidad;
                    Gasto gasto_encontrado = _dbContext.Gastos.Where(p => p.UsuarioId == dv.UsuarioId).First();

                    gasto_encontrado.Monto = dv.Monto;
                    gasto_encontrado.Nombre = dv.Nombre;
                    gasto_encontrado.Descripcion = dv.Descripcion;
                    gasto_encontrado.FechaDeIngreso = dv.FechaDeIngreso;
                    _dbContext.Gastos.Update(gasto_encontrado);
                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Gastos.AddAsync(entidad);
                    await _dbContext.SaveChangesAsync();
                    gastoGenerado = entidad;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return gastoGenerado;
        }

        public async Task<List<Gasto>> Reporte(DateTime FechaIngreso)
        {
            List<Gasto> listaResumen = await _dbContext.Gastos
                .Include(v => v.GastoId)
                .Where(dv => dv.FechaDeIngreso == FechaIngreso).ToListAsync();

            return listaResumen;
        }
    }
}
