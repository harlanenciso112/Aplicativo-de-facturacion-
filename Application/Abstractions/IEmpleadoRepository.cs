using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Abstractions
{
    public interface IEmpleadoRepository
    {
        List<Empleado> ObtenerTodos(string filtro);
        Empleado? ObtenerPorId(int id);
        void Crear(Empleado empleado);
        void Actualizar(Empleado empleado);
        void Eliminar(int id);
    }
}