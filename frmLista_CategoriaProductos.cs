using System;
using System.Linq;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmLista_CategoriaProductos : Form
    {
        private readonly CategoriaProductoService _categoriaService;

        public frmLista_CategoriaProductos()
        {
            InitializeComponent();
            _categoriaService = AppContainer.CategoriaProductoService;
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

                var lista = _categoriaService.Listar(filtro)
                    .Select(c => new
                    {
                        id = c.Id,
                        nombre = c.Nombre,
                        descripcion = c.Descripcion,
                        estado = c.Activo ? "Activo" : "Inactivo"
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
            new frmCategoriaProductos().ShowDialog();
            CargarDatos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un registro.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["colId"].Value);
            new frmCategoriaProductos { IdCategoria = id }.ShowDialog();
            CargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un registro.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro de eliminar este registro?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["colId"].Value);
                    _categoriaService.Eliminar(id);
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

        private void frmLista_CategoriaProductos_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }
    }
}
