using ProyectoShopLog.BLL.Interfaces;
using ProyectoShopLog.DAL.Interfaces;
using ProyectoShopLog.Entity;

namespace ProyectoShopLog.BLL.Implementacion
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _repositorio;

        public CategoriaService(IGenericRepository<Categoria> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Categoria> Crear(Categoria categoria)
        {
            try
            {
                Categoria categoriaCreada = await _repositorio.Crear(categoria);

                if (categoriaCreada.CategoriaId == 0)
                {
                    throw new TaskCanceledException("No se pudo ingresar la categoria");
                }

                IQueryable<Categoria> query = await _repositorio.Consultar(u => u.CategoriaId == categoriaCreada.CategoriaId);
                return query.First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Categoria> Editar(Categoria categoria)
        {
            try
            {
                IQueryable<Categoria> queryCategoria = await _repositorio.Consultar(u => u.CategoriaId == categoria.CategoriaId);

                Categoria categoriaEdit = queryCategoria.First();
                categoriaEdit.Nombre = categoria.Nombre;
                categoriaEdit.Descripcion = categoria.Descripcion;
                categoriaEdit.TipoMovimiento = categoria.TipoMovimiento;

                bool respuesta = await _repositorio.Editar(categoriaEdit);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo modificar la categoria");
                }

                return queryCategoria.First();
            }
            catch
            {

                throw;
            }
        }

        public async Task<List<Categoria>> Lista()
        {
            IQueryable<Categoria> categoriaQuery = await _repositorio.Consultar();
            return categoriaQuery.ToList();
        }

        public async Task<List<Categoria>> ListaByTipoMovimiento(string tipoMovimiento)
        {
            IQueryable<Categoria> categoriaQuery = await _repositorio.Consultar(categoria => categoria.TipoMovimiento == tipoMovimiento);
            return categoriaQuery.ToList();
        }
    }
}