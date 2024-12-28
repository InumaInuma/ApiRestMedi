using ApiRestMedi.Context;
using ApiRestMedi.Models;
using ApiRestMedi.Repository;
using System.Formats.Asn1;

namespace ApiRestMedi.Service
{
    public class Servicio
    {
        private readonly IRepository _repository;

        public Servicio(IRepository irepositorio)
        {
            _repository = irepositorio;
        }

        public async Task <bool> InsertarDatosAsync(Productos productos)
        {
            return await _repository.InsertarDatosAsync(productos);
        }

        public async Task<IEnumerable<Productos>> ListarProductosAsync()
        {
            return await _repository.ListarProductosAsync();
        }
        public async Task<IEnumerable<Productos>> BuscarProductosPorNombreAsync(string nombre)
        {
            return await _repository.BuscarProductosPorNombreAsync(nombre);
        }
        public async Task<bool> ActualizarProductoAsync(Productos productos)
        {
            return await _repository.ActualizarProductoAsync(productos);
        }
        public async Task<bool> DardeBajaProducto(Productos productos)
        {
            return await _repository.DardeBajaProducto(productos);
        }
    }
}
