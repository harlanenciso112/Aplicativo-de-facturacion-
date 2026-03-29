using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Pantallas_Sistema_facturacion.Application.Abstractions;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion.Infrastructure.Reports
{
    public class PdfSharpFacturaPdfExporter : IFacturaPdfExporter
    {
        public void Exportar(string filePath, DateTime fechaInicio, DateTime fechaFin, List<Factura> facturas)
        {
            var documento = new PdfDocument();
            documento.Info.Title = "Informe de Facturas";

            var pagina = documento.AddPage();
            pagina.Size = PdfSharpCore.PageSize.A4;
            var gfx = XGraphics.FromPdfPage(pagina);

            var fuenteTitulo = new XFont("Arial", 14, XFontStyle.Bold);
            var fuenteSubtitulo = new XFont("Arial", 10, XFontStyle.Regular);
            var fuenteNormal = new XFont("Arial", 9, XFontStyle.Regular);
            var fuenteNegrita = new XFont("Arial", 9, XFontStyle.Bold);

            double y = 40;
            gfx.DrawString("Informe de Facturas", fuenteTitulo, XBrushes.Black, new XPoint(40, y));
            y += 22;
            gfx.DrawString($"Período: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}", fuenteSubtitulo, XBrushes.Black, new XPoint(40, y));
            y += 18;
            gfx.DrawString($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}", fuenteSubtitulo, XBrushes.Black, new XPoint(40, y));
            y += 24;

            gfx.DrawString("Factura", fuenteNegrita, XBrushes.Black, new XPoint(40, y));
            gfx.DrawString("Fecha", fuenteNegrita, XBrushes.Black, new XPoint(140, y));
            gfx.DrawString("Cliente", fuenteNegrita, XBrushes.Black, new XPoint(220, y));
            gfx.DrawString("Total", fuenteNegrita, XBrushes.Black, new XPoint(490, y));
            y += 8;
            gfx.DrawLine(XPens.Gray, 40, y, 555, y);
            y += 16;

            if (facturas.Count == 0)
            {
                gfx.DrawString("No hay facturas en el período seleccionado.", fuenteNormal, XBrushes.Black, new XPoint(40, y));
                y += 24;
            }
            else
            {
                foreach (var factura in facturas)
                {
                    if (y > 780)
                    {
                        pagina = documento.AddPage();
                        pagina.Size = PdfSharpCore.PageSize.A4;
                        gfx = XGraphics.FromPdfPage(pagina);
                        y = 40;
                    }

                    gfx.DrawString(factura.Numero, fuenteNormal, XBrushes.Black, new XPoint(40, y));
                    gfx.DrawString(factura.Fecha.ToString("dd/MM/yyyy"), fuenteNormal, XBrushes.Black, new XPoint(140, y));

                    var cliente = Truncar(factura.ClienteNombre, 42);
                    gfx.DrawString(cliente, fuenteNormal, XBrushes.Black, new XPoint(220, y));

                    var totalTexto = factura.Total.ToString("N2", CultureInfo.InvariantCulture);
                    gfx.DrawString(totalTexto, fuenteNormal, XBrushes.Black, new XRect(470, y - 10, 80, 20), XStringFormats.TopRight);
                    y += 16;
                }
            }

            y += 10;
            gfx.DrawLine(XPens.Gray, 40, y, 555, y);
            y += 18;
            var totalGeneral = facturas.Sum(f => f.Total);
            gfx.DrawString("Total general:", fuenteNegrita, XBrushes.Black, new XPoint(390, y));
            gfx.DrawString(totalGeneral.ToString("N2", CultureInfo.InvariantCulture), fuenteNegrita, XBrushes.Black, new XRect(470, y - 10, 80, 20), XStringFormats.TopRight);

            var directorio = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directorio) && !Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

            documento.Save(filePath);
        }

        private static string Truncar(string valor, int max)
        {
            var texto = valor ?? string.Empty;
            if (texto.Length <= max)
            {
                return texto;
            }

            return texto.Substring(0, max - 3) + "...";
        }
    }
}