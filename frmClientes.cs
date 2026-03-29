using System;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmClientes : Form
    {
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int IdCliente { get; set; } = 0;
        private readonly ClienteService _clienteService;

        public frmClientes()
        {
            InitializeComponent();
            _clienteService = AppContainer.ClienteService;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (IdCliente > 0)
            {
                try
                {
                    var cliente = _clienteService.ObtenerPorId(IdCliente);
                    if (cliente != null)
                    {
                        txtNombre.Text = cliente.Nombre;
                        txtCedulaRuc.Text = cliente.CedulaRuc;
                        txtTelefono.Text = cliente.Telefono;
                        txtEmail.Text = cliente.Email;
                        txtDireccion.Text = cliente.Direccion;
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
                    var cliente = new Cliente
                    {
                        Id = IdCliente,
                        Nombre = txtNombre.Text.Trim(),
                        CedulaRuc = txtCedulaRuc.Text.Trim(),
                        Telefono = txtTelefono.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Direccion = txtDireccion.Text.Trim()
                    };

                    _clienteService.Guardar(cliente);
                    MessageBox.Show(IdCliente == 0
                        ? "Cliente registrado correctamente."
                        : "Cliente actualizado correctamente.", "Éxito",
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
            txtCedulaRuc.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            errorProvider1.Clear();
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

            if (string.IsNullOrWhiteSpace(txtCedulaRuc.Text))
            {
                errorProvider1.SetError(txtCedulaRuc, "La cédula/RUC es obligatoria");
                esValido = false;
            }
            else errorProvider1.SetError(txtCedulaRuc, "");

            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                errorProvider1.SetError(txtTelefono, "El teléfono es obligatorio");
                esValido = false;
            }
            else errorProvider1.SetError(txtTelefono, "");

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "El email es obligatorio");
                esValido = false;
            }
            else errorProvider1.SetError(txtEmail, "");

            return esValido;
        }
    }
}
