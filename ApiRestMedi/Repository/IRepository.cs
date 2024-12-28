using ApiRestMedi.Models;

namespace ApiRestMedi.Repository
{
    public interface IRepository
    {
        Task<bool> InsertarDatosAsync(Productos productos);
        Task<IEnumerable<Productos>> ListarProductosAsync();
        Task<IEnumerable<Productos>> BuscarProductosPorNombreAsync(string nombre);
        Task<bool> ActualizarProductoAsync(Productos productos);
        Task<bool> DardeBajaProducto(Productos productos);
    }
}
