using System;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmAyuda : Form
    {
        public frmAyuda()
        {
            InitializeComponent();
        }

        private void frmAyuda_Load(object sender, EventArgs e)
        {
            panelNavegacion.Visible = false;
            panelEstado.Visible = false;
            webBrowser1.Visible = false;
            lblTitulo.Text = "Ayuda";
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
        }

        private void btnAdelante_Click(object sender, EventArgs e)
        {
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
        }

        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
        }
    }
}
