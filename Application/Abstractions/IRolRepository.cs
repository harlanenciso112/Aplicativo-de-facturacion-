using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Abstractions
{
    public interface IRolRepository
    {
        List<Rol> ObtenerTodos(string filtro);
        List<Rol> ObtenerActivos();
        Rol? ObtenerPorId(int id);
        Rol? ObtenerPorNombre(string nombre);
        void Crear(Rol rol);
        void Actualizar(Rol rol);
        void Eliminar(int id);
    }
}