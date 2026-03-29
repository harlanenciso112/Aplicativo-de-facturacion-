using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Abstractions
{
    public interface IClienteRepository
    {
        List<Cliente> ObtenerTodos(string filtro);
        Cliente? ObtenerPorId(int id);
        void Crear(Cliente empleado);
        void Actualizar(Cliente empleado);
        void Eliminar(int id);
    }
}