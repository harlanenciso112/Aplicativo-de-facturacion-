using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Services
{
    public class ProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public List<Producto> Listar(string filtro = "")
        {
            return _productoRepository.ObtenerTodos(filtro);
        }

        public Producto? ObtenerPorId(int id)
        {
            return _productoRepository.ObtenerPorId(id);
        }

        public void Guardar(Producto producto)
        {
            if (producto.Id == 0)
                _productoRepository.Crear(producto);
            else
                _productoRepository.Actualizar(producto);
        }

        public void Eliminar(int id)
        {
            _productoRepository.Eliminar(id);
        }

        public List<string> ObtenerCategorias()
        {
            return _productoRepository.ObtenerCategorias();
        }
    }
}