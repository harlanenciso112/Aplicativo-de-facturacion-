using System;
using System.Linq;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmLista_RolEmpleados : Form
    {
        private readonly RolService _rolService;

        public frmLista_RolEmpleados()
        {
            InitializeComponent();
            _rolService = AppContainer.RolService;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var frm = new frmRolEmpleados();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                CargarDatos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un rol para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["colId"].Value);
            using var frm = new frmRolEmpleados { IdRol = id };
            if (frm.ShowDialog() == DialogResult.OK)
            {
                CargarDatos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un rol para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("¿Está seguro de eliminar este registro?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["colId"].Value);
                    _rolService.Eliminar(id);
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar rol: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarDatos(txtBuscar.Text.Trim());
        }

        private void frmLista_RolEmpleados_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos(string filtro = "")
        {
            try
            {
                colId.DataPropertyName = "id";
                colNombre.DataPropertyName = "nombre";
                colDescripcion.DataPropertyName = "descripcion";
                colEstado.DataPropertyName = "estado";
                dgvDatos.AutoGenerateColumns = false;

                var lista = _rolService.Listar(filtro)
                    .Select(r => new
                    {
                        id = r.Id,
                        nombre = r.Nombre,
                        descripcion = r.Descripcion,
                        estado = r.Estado
                    })
                    .ToList();

                dgvDatos.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar roles: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
