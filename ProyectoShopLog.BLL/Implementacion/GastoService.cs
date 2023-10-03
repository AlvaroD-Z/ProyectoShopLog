using Microsoft.EntityFrameworkCore;
using ProyectoShopLog.BLL.Interfaces;
using ProyectoShopLog.DAL.Interfaces;
using ProyectoShopLog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoShopLog.BLL.Implementacion
{
    public class GastoService : IGastoService
    {
        private readonly IGenericRepository<Gasto> _repositorio;
        private readonly IUtilidadesService _utilidadesService;
        private readonly ICorreoService _correoService;
        public GastoService(
            IGenericRepository<Gasto> repositorio,
            IUtilidadesService utilidadesService,
            ICorreoService correoService
            )
        {
            _repositorio = repositorio;
            _utilidadesService = utilidadesService;
            _correoService = correoService;
        }
        public async Task<List<Gasto>> Lista()
        {
            IQueryable<Gasto> query = await _repositorio.Consultar();
            return query.ToList();
        }

        public async Task<Gasto> Crear(Gasto entidad)
        {
            try
            {
                Gasto gasto_creado = await _repositorio.Crear(entidad);

                if (gasto_creado.GastoId == 0)
                {
                    throw new TaskCanceledException("No se pudo ingresar el gasto");
                }
                else
                {
                    Console.WriteLine("Gasto ingresado correctamente");
                }
                   
                IQueryable<Gasto> query = await _repositorio.Consultar(u => u.GastoId == gasto_creado.GastoId);
                gasto_creado = query.First();

                return gasto_creado;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Gasto> Editar(Gasto entidad)
        {
            try
            {
                IQueryable<Gasto> queryGasto = await _repositorio.Consultar(u => u.GastoId == entidad.GastoId);

                Gasto Gasto_editar = queryGasto.First();
                Gasto_editar.Nombre = entidad.Nombre;
                Gasto_editar.Descripcion = entidad.Descripcion;
                Gasto_editar.Monto = entidad.Monto;

                bool respuesta = await _repositorio.Editar(Gasto_editar);

                if (respuesta)
                {
                    throw new TaskCanceledException("No se pudo modificar el Gasto");
                }
                Gasto gasto_editado = queryGasto.Include(r => r.GastoId).First();

                return gasto_editado;
            }
            catch
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int GastoId)
        {
            try
            {
                Gasto gasto_encontrado = await _repositorio.Obtener(u => u.GastoId == GastoId);

                if (gasto_encontrado == null)
                {
                    throw new TaskCanceledException("El Gasto no existe");
                }
                bool respuesta = await _repositorio.Eliminar(gasto_encontrado);

                return true;
            }
            catch
            {

                throw;
            }
        }

        public async Task<Gasto> ObtenerPorId(int GastoId)
        {
            IQueryable<Gasto> query = await _repositorio.Consultar(u => u.GastoId == GastoId);

            Gasto resultado = query.FirstOrDefault();
            return resultado;
        }
    }
}
