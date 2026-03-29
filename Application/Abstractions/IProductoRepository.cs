using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Abstractions
{
    public interface IProductoRepository
    {
        List<Producto> ObtenerTodos(string filtro);
        Producto? ObtenerPorId(int id);
        void Crear(Producto producto);
        void Actualizar(Producto producto);
        void Eliminar(int id);
        List<string> ObtenerCategorias();
    }
}