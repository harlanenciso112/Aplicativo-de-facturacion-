using System;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmProductos : Form
    {
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int IdProducto { get; set; } = 0;
        private readonly ProductoService _productoService;

        public frmProductos()
        {
            InitializeComponent();
            _productoService = AppContainer.ProductoService;
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            try
            {
                var categorias = _productoService.ObtenerCategorias();
                cboCategoria.Items.Clear();
                foreach (var cat in categorias)
                    cboCategoria.Items.Add(cat);
            }
            catch
            {
                cboCategoria.Items.Clear();
                cboCategoria.Items.Add("Electrónica");
                cboCategoria.Items.Add("Accesorios");
                cboCategoria.Items.Add("Software");
            }

            if (IdProducto > 0)
            {
                try
                {
                    var producto = _productoService.ObtenerPorId(IdProducto);
                    if (producto != null)
                    {
                        txtNombre.Text = producto.Nombre;
                        txtCodigo.Text = producto.Codigo;
                        txtPrecio.Text = producto.Precio.ToString();
                        txtStock.Text = producto.Stock.ToString();
                        txtDescripcion.Text = producto.Descripcion;
                        cboCategoria.Text = producto.Categoria;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {
                    decimal precio = Convert.ToDecimal(
                        txtPrecio.Text.Replace(',', '.'),
                        System.Globalization.CultureInfo.InvariantCulture);
                    int stock = string.IsNullOrWhiteSpace(txtStock.Text)
                        ? 0 : Convert.ToInt32(txtStock.Text);

                    var producto = new Producto
                    {
                        Id = IdProducto,
                        Nombre = txtNombre.Text.Trim(),
                        Codigo = txtCodigo.Text.Trim(),
                        Precio = precio,
                        Stock = stock,
                        Descripcion = txtDescripcion.Text.Trim(),
                        Categoria = cboCategoria.Text
                    };

                    _productoService.Guardar(producto);
                    MessageBox.Show(IdProducto == 0
                        ? "Producto registrado correctamente."
                        : "Producto actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => this.Close();

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear(); txtCodigo.Clear();
            txtPrecio.Clear(); txtStock.Clear();
            txtDescripcion.Clear();
            cboCategoria.SelectedIndex = -1;
            errorProvider1.Clear();
        }

        private bool ValidarCampos()
        {
            bool esValido = true;
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            { errorProvider1.SetError(txtNombre, "El nombre es obligatorio"); esValido = false; }
            else errorProvider1.SetError(txtNombre, "");

            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            { errorProvider1.SetError(txtCodigo, "El código es obligatorio"); esValido = false; }
            else errorProvider1.SetError(txtCodigo, "");

            if (!decimal.TryParse(txtPrecio.Text.Replace(',', '.'),
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture, out _))
            { errorProvider1.SetError(txtPrecio, "Ingrese un precio válido"); esValido = false; }
            else errorProvider1.SetError(txtPrecio, "");

            if (string.IsNullOrWhiteSpace(cboCategoria.Text))
            { errorProvider1.SetError(cboCategoria, "Seleccione una categoría"); esValido = false; }
            else errorProvider1.SetError(cboCategoria, "");

            return esValido;
        }
    }
}