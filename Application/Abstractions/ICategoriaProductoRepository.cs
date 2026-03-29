using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Abstractions
{
    public interface ICategoriaProductoRepository
    {
        List<CategoriaProducto> ObtenerTodos(string filtro);
        CategoriaProducto? ObtenerPorId(int id);
        void Crear(CategoriaProducto categoria);
        void Actualizar(CategoriaProducto categoria);
        void Eliminar(int id);
    }
}