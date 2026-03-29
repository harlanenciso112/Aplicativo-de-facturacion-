using System;
using System.Linq;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmLista_Productos : Form
    {
        private readonly ProductoService _productoService;

        public frmLista_Productos()
        {
            InitializeComponent();
            _productoService = AppContainer.ProductoService;
        }

        private void CargarDatos(string filtro = "")
        {
            try
            {
                colId.DataPropertyName = "id";
                colNombre.DataPropertyName = "nombre";
                colCategoria.DataPropertyName = "categoria";
                colPrecio.DataPropertyName = "precio";
                colStock.DataPropertyName = "stock";
                dgvDatos.AutoGenerateColumns = false;

                var lista = _productoService.Listar(filtro)
                    .Select(p => new
                    {
                        id = p.Id,
                        nombre = p.Nombre,
                        categoria = p.Categoria,
                        precio = p.Precio,
                        stock = p.Stock
                    }).ToList();

                dgvDatos.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLista_Productos_Load(object sender, EventArgs e) => CargarDatos();

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            new frmProductos().ShowDialog();
            CargarDatos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null)
            { MessageBox.Show("Seleccione un registro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["colId"].Value);
            new frmProductos { IdProducto = id }.ShowDialog();
            CargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null)
            { MessageBox.Show("Seleccione un registro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show("¿Eliminar este producto?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["colId"].Value);
                    _productoService.Eliminar(id);
                    CargarDatos();
                }
                catch (Exception ex)
                { MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e) => CargarDatos(txtBuscar.Text.Trim());
    }
}