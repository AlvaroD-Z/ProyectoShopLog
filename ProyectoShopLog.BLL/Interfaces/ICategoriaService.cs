using ProyectoShopLog.Entity;

namespace ProyectoShopLog.BLL.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> Lista();
        Task<List<Categoria>> ListaByTipoMovimiento(string tipoMovimiento);
        Task<Categoria> Crear(Categoria categoria);
        Task<Categoria> Editar(Categoria categoria);
    }
}