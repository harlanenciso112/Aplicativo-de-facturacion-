using System;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmEmpleados : Form
    {
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int IdEmpleado { get; set; } = 0;
        private readonly EmpleadoService _empleadoService;
        private readonly RolService _rolService;

        public frmEmpleados()
        {
            InitializeComponent();
            _empleadoService = AppContainer.EmpleadoService;
            _rolService = AppContainer.RolService;
        }

        private void frmEmpleados_Load(object sender, EventArgs e)
        {
            CargarRoles();

            if (IdEmpleado > 0)
            {
                try
                {
                    var empleado = _empleadoService.ObtenerPorId(IdEmpleado);
                    if (empleado != null)
                    {
                        txtNombre.Text = empleado.Nombre;
                        txtCedula.Text = empleado.Cedula;
                        txtTelefono.Text = empleado.Telefono;
                        txtEmail.Text = empleado.Email;
                        txtDireccion.Text = empleado.Direccion;
                        cboRol.Text = empleado.Rol;
                        chkActivo.Checked = empleado.Activo;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CargarRoles()
        {
            cboRol.Items.Clear();
            foreach (var rol in _rolService.ListarActivos())
            {
                cboRol.Items.Add(rol.Nombre);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {
                    var empleado = new Empleado
                    {
                        Id = IdEmpleado,
                        Nombre = txtNombre.Text.Trim(),
                        Cedula = txtCedula.Text.Trim(),
                        Telefono = txtTelefono.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Direccion = txtDireccion.Text.Trim(),
                        Rol = cboRol.Text,
                        Activo = chkActivo.Checked
                    };

                    var resultado = _empleadoService.Guardar(empleado);
                    if (IdEmpleado == 0 && resultado.UsuarioCreado)
                    {
                        MessageBox.Show(
                            $"Empleado registrado correctamente.\n\nSe creó el acceso al sistema:\nUsuario: {resultado.UsuarioLogin}\nContraseña temporal: {resultado.PasswordTemporal}",
                            "Éxito",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Empleado actualizado correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

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
            else errorProvider1.SetError(txtNombre, "");

            if (string.IsNullOrWhiteSpace(txtCedula.Text))
            {
                errorProvider1.SetError(txtCedula, "La cédula es obligatoria");
                esValido = false;
            }
            else errorProvider1.SetError(txtCedula, "");

            if (cboRol.SelectedIndex == -1)
            {
                errorProvider1.SetError(cboRol, "Seleccione un rol");
                esValido = false;
            }
            else errorProvider1.SetError(cboRol, "");

            return esValido;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
            txtCedula.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            cboRol.SelectedIndex = -1;
            chkActivo.Checked = true;
            errorProvider1.Clear();
        }
    }
}
