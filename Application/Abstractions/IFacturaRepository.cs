using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Abstractions
{
    public interface IFacturaRepository
    {
        List<Factura> ObtenerTodos(string filtro);
        Factura? ObtenerPorId(int id);
        string ObtenerSiguienteNumero();
        void Crear(Factura factura);
        void Actualizar(Factura factura);
        void Eliminar(int id);
    }
}