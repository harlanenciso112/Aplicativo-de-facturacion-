using System;
using System.Collections.Generic;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Abstractions
{
    public interface IFacturaPdfExporter
    {
        void Exportar(string filePath, DateTime fechaInicio, DateTime fechaFin, List<Factura> facturas);
    }
}