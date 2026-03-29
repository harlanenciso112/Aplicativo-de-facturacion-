using System;
using System.Linq;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmLista_Clientes : Form
    {
        private readonly ClienteService _clienteService;

        public frmLista_Clientes()
        {
            InitializeComponent();
            _clienteService = AppContainer.ClienteService;
        }

        private void CargarDatos(string filtro = "")
        {
            try
            {
                colId.DataPropertyName = "id";
                colNombre.DataPropertyName = "nombre";
                colTelefono.DataPropertyName = "telefono";
                colEmail.DataPropertyName = "email";
                colDireccion.DataPropertyName = "direccion";
                dgvDatos.AutoGenerateColumns = false;

                var lista = _clienteService.Listar(filtro)
                    .Select(c => new
                    {
                        id = c.Id,
                        nombre = c.Nombre,
                        telefono = c.Telefono,
                        email = c.Email,
                        direccion = c.Direccion
                    }).ToList();

                dgvDatos.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLista_Clientes_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            new frmClientes().ShowDialog();
            CargarDatos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un registro para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["colId"].Value);
            new frmClientes { IdCliente = id }.ShowDialog();
            CargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un registro para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("¿Está seguro de eliminar este registro?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["colId"].Value);
                    _clienteService.Eliminar(id);
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarDatos(txtBuscar.Text.Trim());
        }
    }
}