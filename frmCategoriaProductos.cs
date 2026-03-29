using System;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmCategoriaProductos : Form
    {
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int IdCategoria { get; set; } = 0;

        private readonly CategoriaProductoService _categoriaService;

        public frmCategoriaProductos()
        {
            InitializeComponent();
            _categoriaService = AppContainer.CategoriaProductoService;
            this.Load += frmCategoriaProductos_Load;
        }

        private void frmCategoriaProductos_Load(object? sender, EventArgs e)
        {
            if (IdCategoria <= 0)
            {
                return;
            }

            try
            {
                var categoria = _categoriaService.ObtenerPorId(IdCategoria);
                if (categoria == null)
                {
                    MessageBox.Show("La categoría seleccionada no existe.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                txtNombre.Text = categoria.Nombre;
                txtDescripcion.Text = categoria.Descripcion;
                chkActivo.Checked = categoria.Activo;
                lblTitulo.Text = "📋 Editar Categoría";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {
                    var categoria = new CategoriaProducto
                    {
                        Id = IdCategoria,
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = txtDescripcion.Text.Trim(),
                        Activo = chkActivo.Checked
                    };

                    _categoriaService.Guardar(categoria);
                    MessageBox.Show(IdCategoria == 0
                        ? "Categoría registrada correctamente."
                        : "Categoría actualizada correctamente.", "Éxito",
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidarCampos()
        {
            bool esValido = true;

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "El nombre es obligatorio");
                esValido = false;
            }
            else
            {
                errorProvider1.SetError(txtNombre, "");
            }

            return esValido;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            chkActivo.Checked = true;
            errorProvider1.Clear();
        }
    }
}
