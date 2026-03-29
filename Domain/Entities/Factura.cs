using System;
using System.Collections.Generic;

namespace Pantallas_Sistema_facturacion.Domain.Entities
{
    public class Factura
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Estado { get; set; } = "ACTIVA";
        public List<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();
    }
}