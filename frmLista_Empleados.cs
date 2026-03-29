using System;
using System.Linq;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmLista_Empleados : Form
    {
        private readonly EmpleadoService _empleadoService;

        public frmLista_Empleados()
        {
            InitializeComponent();
            _empleadoService = AppContainer.EmpleadoService;
        }

        private void CargarDatos(string filtro = "")
        {
            try
            {
                colId.DataPropertyName = "id";
                colNombre.DataPropertyName = "nombre";
                colCedula.DataPropertyName = "cedula";
                colRol.DataPropertyName = "rol";
                colEstado.DataPropertyName = "estado";
                dgvDatos.AutoGenerateColumns = false;

                var lista = _empleadoService.Listar(filtro)
                    .Select(e => new
                    {
                        id = e.Id,
                        nombre = e.Nombre,
                        cedula = e.Cedula,
                        rol = e.Rol,
                        estado = e.Activo ? "Activo" : "Inactivo"
                    })
                    .ToList();

                dgvDatos.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            frmEmpleados frm = new frmEmpleados();
            frm.ShowDialog();
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
            frmEmpleados frm = new frmEmpleados { IdEmpleado = id };
            frm.ShowDialog();
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
            var resultado = MessageBox.Show("¿Está seguro de eliminar este registro?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["colId"].Value);
                    _empleadoService.Eliminar(id);
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

        private void frmLista_Empleados_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }
    }
}
