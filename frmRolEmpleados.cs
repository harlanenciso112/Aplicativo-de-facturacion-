using System;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmRolEmpleados : Form
    {
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int IdRol { get; set; }

        private readonly RolService _rolService;

        public frmRolEmpleados()
        {
            InitializeComponent();
            _rolService = AppContainer.RolService;
        }

        private void frmRolEmpleados_Load(object sender, EventArgs e)
        {
            if (IdRol <= 0)
            {
                return;
            }

            try
            {
                var rol = _rolService.ObtenerPorId(IdRol);
                if (rol == null)
                {
                    MessageBox.Show("No se encontró el rol seleccionado.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }

                txtNombre.Text = rol.Nombre;
                txtDescripcion.Text = rol.Descripcion;
                chkActivo.Checked = rol.Activo;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar rol: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {
                    _rolService.Guardar(new Rol
                    {
                        Id = IdRol,
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = txtDescripcion.Text.Trim(),
                        Activo = chkActivo.Checked
                    });

                    MessageBox.Show(IdRol == 0 ? "Rol registrado correctamente." : "Rol actualizado correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar rol: " + ex.Message, "Error",
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
