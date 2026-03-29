using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Application.Services
{
    public class InformeFacturasService
    {
        private readonly FacturaService _facturaService;
        private readonly IFacturaPdfExporter _facturaPdfExporter;

        public InformeFacturasService(FacturaService facturaService, IFacturaPdfExporter facturaPdfExporter)
        {
            _facturaService = facturaService;
            _facturaPdfExporter = facturaPdfExporter;
        }

        public List<Factura> ObtenerFacturas(DateTime fechaInicio, DateTime fechaFin)
        {
            var inicio = fechaInicio.Date;
            var fin = fechaFin.Date.AddDays(1).AddTicks(-1);

            return _facturaService.Listar()
                .Where(f => f.Fecha >= inicio && f.Fecha <= fin)
                .OrderBy(f => f.Fecha)
                .ThenBy(f => f.Numero)
                .ToList();
        }

        public string GenerarVistaPrevia(DateTime fechaInicio, DateTime fechaFin)
        {
            var facturas = ObtenerFacturas(fechaInicio, fechaFin);
            var total = facturas.Sum(f => f.Total);

            var sb = new StringBuilder();
            sb.AppendLine("═══════════════════════════════════════════");
            sb.AppendLine("      INFORME SIMPLE DE FACTURAS");
            sb.AppendLine("═══════════════════════════════════════════");
            sb.AppendLine();
            sb.AppendLine($"Período: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}");
            sb.AppendLine($"Facturas encontradas: {facturas.Count}");
            sb.AppendLine($"Total vendido: {total.ToString("C2", CultureInfo.GetCultureInfo("es-EC"))}");
            sb.AppendLine();

            if (facturas.Count == 0)
            {
                sb.AppendLine("No hay facturas en el período seleccionado.");
                sb.AppendLine("═══════════════════════════════════════════");
                return sb.ToString();
            }

            sb.AppendLine("Nro Factura | Fecha       | Cliente                 | Total");
            sb.AppendLine("--------------------------------------------------------------");

            foreach (var factura in facturas)
            {
                var numero = Ajustar(factura.Numero, 10);
                var fecha = factura.Fecha.ToString("dd/MM/yyyy");
                var cliente = Ajustar(factura.ClienteNombre, 22);
                var totalTexto = factura.Total.ToString("N2", CultureInfo.InvariantCulture).PadLeft(10);

                sb.AppendLine($"{numero} | {fecha} | {cliente} | {totalTexto}");
            }

            sb.AppendLine("═══════════════════════════════════════════");
            return sb.ToString();
        }

        public void ExportarPdf(string filePath, DateTime fechaInicio, DateTime fechaFin)
        {
            var facturas = ObtenerFacturas(fechaInicio, fechaFin);
            _facturaPdfExporter.Exportar(filePath, fechaInicio, fechaFin, facturas);
        }

        private static string Ajustar(string valor, int maxLength)
        {
            var texto = valor ?? string.Empty;
            if (texto.Length <= maxLength)
            {
                return texto.PadRight(maxLength);
            }

            return texto.Substring(0, maxLength - 1) + "…";
        }
    }
}