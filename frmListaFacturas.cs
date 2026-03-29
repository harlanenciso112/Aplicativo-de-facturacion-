using System;
using System.Linq;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmListaFacturas : Form
    {
        private readonly FacturaService _facturaService;

        public frmListaFacturas()
        {
            InitializeComponent();
            _facturaService = AppContainer.FacturaService;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var frm = new frmFacturas();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                CargarDatos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una factura para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["colNumero"].Tag);
            using var frm = new frmFacturas { IdFactura = id };
            if (frm.ShowDialog() == DialogResult.OK)
            {
                CargarDatos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una factura para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("¿Está seguro de eliminar este registro?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["colNumero"].Tag);
                    _facturaService.Eliminar(id);
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar factura: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarDatos(txtBuscar.Text.Trim());
        }

        private void frmListaFacturas_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos(string filtro = "")
        {
            try
            {
                var lista = _facturaService.Listar(filtro)
                    .Select(f => new
                    {
                        id = f.Id,
                        numero = f.Numero,
                        fecha = f.Fecha.ToString("dd/MM/yyyy"),
                        cliente = f.ClienteNombre,
                        total = f.Total.ToString("N2"),
                        estado = f.Estado
                    })
                    .ToList();

                dgvDatos.Rows.Clear();
                foreach (var item in lista)
                {
                    var rowIndex = dgvDatos.Rows.Add(item.numero, item.fecha, item.cliente, item.total, item.estado);
                    dgvDatos.Rows[rowIndex].Cells["colNumero"].Tag = item.id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar facturas: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
