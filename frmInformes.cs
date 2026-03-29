using System;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmInformes : Form
    {
        private readonly InformeFacturasService _informeFacturasService;

        public frmInformes()
        {
            InitializeComponent();
            _informeFacturasService = AppContainer.InformeFacturasService;
        }

        private void frmInformes_Load(object sender, EventArgs e)
        {
            dtpFechaInicio.Value = DateTime.Now.AddMonths(-1);
            dtpFechaFin.Value = DateTime.Now;
        }

        private void btnVentasDiarias_Click(object sender, EventArgs e)
        {
            txtVistaPrevia.Text = GenerarInformeFacturas();
        }

        private void btnVentasMensuales_Click(object sender, EventArgs e)
        {
            txtVistaPrevia.Text = GenerarInformeFacturas();
        }

        private void btnProductosMasVendidos_Click(object sender, EventArgs e)
        {
            txtVistaPrevia.Text = GenerarInformeProductosMasVendidos();
        }

        private void btnClientesFrecuentes_Click(object sender, EventArgs e)
        {
            txtVistaPrevia.Text = GenerarInformeClientesFrecuentes();
        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            txtVistaPrevia.Text = GenerarInformeInventario();
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            try
            {
                using var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Archivo PDF (*.pdf)|*.pdf",
                    FileName = $"InformeFacturas_{DateTime.Now:yyyyMMdd_HHmm}.pdf",
                    Title = "Guardar informe de facturas"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _informeFacturasService.ExportarPdf(
                    saveFileDialog.FileName,
                    dtpFechaInicio.Value,
                    dtpFechaFin.Value);

                MessageBox.Show("PDF generado correctamente.", "Exportar PDF",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar PDF: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Exportando informe a Excel...", "Exportar Excel", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enviando a impresora...", "Imprimir", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GenerarInformeFacturas()
        {
            return _informeFacturasService.GenerarVistaPrevia(
                dtpFechaInicio.Value,
                dtpFechaFin.Value);
        }

        private string GenerarInformeProductosMasVendidos()
        {
            return "═══════════════════════════════════════════\r\n" +
                   "       PRODUCTOS MÁS VENDIDOS              \r\n" +
                   "═══════════════════════════════════════════\r\n\r\n" +
                   $"Período: {dtpFechaInicio.Value:dd/MM/yyyy} - {dtpFechaFin.Value:dd/MM/yyyy}\r\n\r\n" +
                   "Sin datos de prueba cargados.\r\n\r\n" +
                   "Use este módulo para previsualizar la estructura del informe.\r\n\r\n" +
                   "═══════════════════════════════════════════";
        }

        private string GenerarInformeClientesFrecuentes()
        {
            return "═══════════════════════════════════════════\r\n" +
                   "          CLIENTES FRECUENTES              \r\n" +
                   "═══════════════════════════════════════════\r\n\r\n" +
                   $"Período: {dtpFechaInicio.Value:dd/MM/yyyy} - {dtpFechaFin.Value:dd/MM/yyyy}\r\n\r\n" +
                   "Sin datos de prueba cargados.\r\n\r\n" +
                   "Use este módulo para previsualizar la estructura del informe.\r\n\r\n" +
                   "═══════════════════════════════════════════";
        }

        private string GenerarInformeInventario()
        {
            return "═══════════════════════════════════════════\r\n" +
                   "          INFORME DE INVENTARIO            \r\n" +
                   "═══════════════════════════════════════════\r\n\r\n" +
                   $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}\r\n\r\n" +
                   "Sin datos de prueba cargados.\r\n\r\n" +
                   "Use este módulo para previsualizar la estructura del informe.\r\n" +
                   "═══════════════════════════════════════════";
        }
    }
}
