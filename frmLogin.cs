using System;
using System.Drawing;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmLogin : Form
    {
        private readonly AuthService _authService;

        public frmLogin()
        {
            InitializeComponent();
            _authService = AppContainer.AuthService;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            bool esValido = true;

            string usuario = txtUsuario.Text == "Usuario" ? "" : txtUsuario.Text.Trim();
            string password = txtPassword.Text == "Contraseña" ? "" : txtPassword.Text;

            if (string.IsNullOrWhiteSpace(usuario))
            {
                errorProvider1.SetError(txtUsuario, "El usuario es obligatorio");
                esValido = false;
            }
            else
            {
                errorProvider1.SetError(txtUsuario, "");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errorProvider1.SetError(txtPassword, "La contraseña es obligatoria");
                esValido = false;
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
            }

            if (esValido)
            {
                try
                {
                    var usuarioAutenticado = _authService.Autenticar(usuario, password);
                    if (usuarioAutenticado != null)
                    {
                        SesionUsuario.Iniciar(usuarioAutenticado);
                        frmPrincipal principal = new frmPrincipal(usuarioAutenticado);
                        principal.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos.", "Acceso denegado",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error de conexión: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "Usuario")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.FromArgb(64, 64, 64);
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                txtUsuario.Text = "Usuario";
                txtUsuario.ForeColor = Color.Gray;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Contraseña")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.FromArgb(64, 64, 64);
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Contraseña";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }
    }
}
