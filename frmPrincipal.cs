using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmPrincipal : Form
    {
        private bool menuExpandido = true;
        private readonly Usuario? _usuarioActual;
        private readonly FlowLayoutPanel panelMenuSuperior;
        private readonly Button btnModuloPrincipal;
        private readonly Button btnModuloTablas;
        private readonly Button btnModuloFacturacion;
        private readonly Button btnModuloSeguridad;
        private readonly Button btnModuloAyuda;

        public frmPrincipal()
            : this(SesionUsuario.Actual)
        {
        }

        public frmPrincipal(Usuario? usuarioActual)
        {
            InitializeComponent();
            _usuarioActual = usuarioActual;

            panelBarraTitulo.Height = 90;

            panelMenuSuperior = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 38,
                BackColor = Color.FromArgb(0, 122, 204),
                Padding = new Padding(10, 3, 0, 3),
                WrapContents = false
            };

            btnModuloPrincipal = CrearBotonModulo("Principal", btnModuloPrincipal_Click);
            btnModuloTablas = CrearBotonModulo("Tablas", btnModuloTablas_Click);
            btnModuloFacturacion = CrearBotonModulo("Facturación", btnModuloFacturacion_Click);
            btnModuloSeguridad = CrearBotonModulo("Seguridad", btnModuloSeguridad_Click);
            btnModuloAyuda = CrearBotonModulo("Ayuda", btnModuloAyuda_Click);

            panelMenuSuperior.Controls.Add(btnModuloPrincipal);
            panelMenuSuperior.Controls.Add(btnModuloTablas);
            panelMenuSuperior.Controls.Add(btnModuloFacturacion);
            panelMenuSuperior.Controls.Add(btnModuloSeguridad);
            panelMenuSuperior.Controls.Add(btnModuloAyuda);

            panelBarraTitulo.Controls.Add(panelMenuSuperior);

            ConfigurarAccesoPorRol();
            MostrarModuloPrincipal();
        }

        private static string NormalizarRol(string? rolNombre)
        {
            return (rolNombre ?? string.Empty).Trim().ToLowerInvariant();
        }

        private bool EsAdmin()
        {
            return NormalizarRol(_usuarioActual?.RolNombre) == "administrador";
        }

        private bool PuedeAccederPantalla(string pantalla)
        {
            if (EsAdmin())
            {
                return true;
            }

            var rol = NormalizarRol(_usuarioActual?.RolNombre);
            return rol switch
            {
                "vendedor" => pantalla is "clientes" or "productos" or "facturas" or "ayuda" or "acercade",
                "contador" => pantalla is "facturas" or "informes" or "ayuda" or "acercade",
                "inventario" => pantalla is "productos" or "categorias" or "informes" or "ayuda" or "acercade",
                "cajero" => pantalla is "facturas" or "ayuda" or "acercade",
                _ => pantalla is "ayuda" or "acercade"
            };
        }

        private bool PuedeAccederModulo(string modulo)
        {
            if (EsAdmin())
            {
                return true;
            }

            var rol = NormalizarRol(_usuarioActual?.RolNombre);
            return modulo switch
            {
                "principal" => true,
                "tablas" => rol is "vendedor" or "inventario",
                "facturacion" => rol is "vendedor" or "contador" or "cajero" or "inventario",
                "seguridad" => false,
                "ayuda" => true,
                _ => false
            };
        }

        private void ConfigurarAccesoPorRol()
        {
            btnModuloPrincipal.Visible = PuedeAccederModulo("principal");
            btnModuloTablas.Visible = PuedeAccederModulo("tablas");
            btnModuloFacturacion.Visible = PuedeAccederModulo("facturacion");
            btnModuloSeguridad.Visible = PuedeAccederModulo("seguridad");
            btnModuloAyuda.Visible = PuedeAccederModulo("ayuda");
        }

        private bool AutorizarPantalla(string pantalla, string nombrePantalla)
        {
            if (PuedeAccederPantalla(pantalla))
            {
                return true;
            }

            MessageBox.Show($"No tiene permisos para acceder a {nombrePantalla}.", "Acceso denegado",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private Button CrearBotonModulo(string texto, EventHandler eventoClick)
        {
            var boton = new Button
            {
                Text = texto,
                Width = 120,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 122, 204),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 8, 0)
            };

            boton.FlatAppearance.BorderColor = Color.White;
            boton.FlatAppearance.BorderSize = 1;
            boton.Click += eventoClick;

            return boton;
        }

        private void ResaltarModulo(Button botonSeleccionado)
        {
            foreach (Button boton in panelMenuSuperior.Controls)
            {
                boton.BackColor = Color.FromArgb(0, 122, 204);
            }

            botonSeleccionado.BackColor = Color.FromArgb(39, 39, 58);
        }

        private void MostrarOpcionesMenuIzquierdo(params Button[] botonesVisibles)
        {
            var todosLosBotones = new List<Button>
            {
                btnClientes,
                btnProductos,
                btnCategorias,
                btnFacturas,
                btnEmpleados,
                btnRoles,
                btnSeguridad,
                btnInformes,
                btnAyuda,
                btnAcercaDe,
                btnCerrarSesion,
                btnSalir
            };

            foreach (var boton in todosLosBotones)
            {
                boton.Visible = false;
            }

            int top = 90;
            foreach (var boton in botonesVisibles)
            {
                boton.Visible = true;
                boton.Top = top;
                top += 45;
            }
        }

        private void MostrarModuloPrincipal()
        {
            var textoUsuario = string.Empty;
            if (_usuarioActual != null)
            {
                var nombreMostrar = string.IsNullOrWhiteSpace(_usuarioActual.Nombre)
                    ? _usuarioActual.UsuarioLogin
                    : _usuarioActual.Nombre;
                var rolMostrar = string.IsNullOrWhiteSpace(_usuarioActual.RolNombre)
                    ? "Sin rol"
                    : _usuarioActual.RolNombre;
                textoUsuario = $" - {nombreMostrar} ({rolMostrar})";
            }

            lblTitulo.Text = "Sistema de Facturación - Principal" + textoUsuario;
            ResaltarModulo(btnModuloPrincipal);
            MostrarOpcionesMenuIzquierdo(btnSalir);
            panelContenedor.Controls.Clear();
        }

        private void MostrarModuloTablas()
        {
            if (!PuedeAccederModulo("tablas"))
            {
                MostrarModuloPrincipal();
                return;
            }

            lblTitulo.Text = "Sistema de Facturación - Tablas";
            ResaltarModulo(btnModuloTablas);
            var botones = new List<Button>();
            if (PuedeAccederPantalla("clientes")) botones.Add(btnClientes);
            if (PuedeAccederPantalla("productos")) botones.Add(btnProductos);
            if (PuedeAccederPantalla("categorias")) botones.Add(btnCategorias);
            MostrarOpcionesMenuIzquierdo(botones.ToArray());
        }

        private void MostrarModuloFacturacion()
        {
            if (!PuedeAccederModulo("facturacion"))
            {
                MostrarModuloPrincipal();
                return;
            }

            lblTitulo.Text = "Sistema de Facturación - Facturación";
            ResaltarModulo(btnModuloFacturacion);
            var botones = new List<Button>();
            if (PuedeAccederPantalla("facturas")) botones.Add(btnFacturas);
            if (PuedeAccederPantalla("informes")) botones.Add(btnInformes);
            MostrarOpcionesMenuIzquierdo(botones.ToArray());
        }

        private void MostrarModuloSeguridad()
        {
            if (!PuedeAccederModulo("seguridad"))
            {
                MostrarModuloPrincipal();
                return;
            }

            lblTitulo.Text = "Sistema de Facturación - Seguridad";
            ResaltarModulo(btnModuloSeguridad);
            var botones = new List<Button>();
            if (PuedeAccederPantalla("seguridad")) botones.Add(btnSeguridad);
            if (PuedeAccederPantalla("roles")) botones.Add(btnRoles);
            if (PuedeAccederPantalla("empleados")) botones.Add(btnEmpleados);
            MostrarOpcionesMenuIzquierdo(botones.ToArray());
        }

        private void MostrarModuloAyuda()
        {
            lblTitulo.Text = "Sistema de Facturación - Ayuda";
            ResaltarModulo(btnModuloAyuda);
            var botones = new List<Button>();
            if (PuedeAccederPantalla("ayuda")) botones.Add(btnAyuda);
            if (PuedeAccederPantalla("acercade")) botones.Add(btnAcercaDe);
            MostrarOpcionesMenuIzquierdo(botones.ToArray());
        }

        private void AbrirFormularioEnPanel(Form formulario)
        {
            if (panelContenedor.Controls.Count > 0)
                panelContenedor.Controls.RemoveAt(0);

            formulario.TopLevel = false;
            formulario.Dock = DockStyle.Fill;
            panelContenedor.Controls.Add(formulario);
            panelContenedor.Tag = formulario;
            formulario.Show();
        }

        // Menú toggle
        private void btnMenu_Click(object sender, EventArgs e)
        {
            timerMenu.Start();
        }

        private void timerMenu_Tick(object sender, EventArgs e)
        {
            if (menuExpandido)
            {
                panelMenu.Width -= 10;
                if (panelMenu.Width <= 60)
                {
                    menuExpandido = false;
                    timerMenu.Stop();
                }
            }
            else
            {
                panelMenu.Width += 10;
                if (panelMenu.Width >= 220)
                {
                    menuExpandido = true;
                    timerMenu.Stop();
                }
            }
        }

        // Eventos de botones del menú
        private void btnClientes_Click(object sender, EventArgs e)
        {
            if (!AutorizarPantalla("clientes", "Clientes")) return;
            AbrirFormularioEnPanel(new frmLista_Clientes());
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            if (!AutorizarPantalla("productos", "Productos")) return;
            AbrirFormularioEnPanel(new frmLista_Productos());
        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            if (!AutorizarPantalla("categorias", "Categorías")) return;
            AbrirFormularioEnPanel(new frmLista_CategoriaProductos());
        }

        private void btnFacturas_Click(object sender, EventArgs e)
        {
            if (!AutorizarPantalla("facturas", "Facturas")) return;
            AbrirFormularioEnPanel(new frmListaFacturas());
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            if (!AutorizarPantalla("empleados", "Empleados")) return;
            AbrirFormularioEnPanel(new frmLista_Empleados());
        }

        private void btnRoles_Click(object sender, EventArgs e)
        {
            if (!AutorizarPantalla("roles", "Roles")) return;
            AbrirFormularioEnPanel(new frmLista_RolEmpleados());
        }

        private void btnSeguridad_Click(object sender, EventArgs e)
        {
            if (!AutorizarPantalla("seguridad", "Seguridad")) return;
            AbrirFormularioEnPanel(new frmAdminSeguridad());
        }

        private void btnInformes_Click(object sender, EventArgs e)
        {
            if (!AutorizarPantalla("informes", "Informes")) return;
            AbrirFormularioEnPanel(new frmInformes());
        }

        private void btnAyuda_Click(object sender, EventArgs e)
        {
            if (!AutorizarPantalla("ayuda", "Ayuda")) return;
            AbrirFormularioEnPanel(new frmAyuda());
        }

        private void btnAcercaDe_Click(object sender, EventArgs e)
        {
            if (!AutorizarPantalla("acercade", "Acerca de")) return;
            AbrirFormularioEnPanel(new frmAcercaDe());
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            SesionUsuario.Cerrar();
            frmLogin login = new frmLogin();
            login.Show();
            this.Close();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void btnModuloPrincipal_Click(object? sender, EventArgs e)
        {
            MostrarModuloPrincipal();
        }

        private void btnModuloTablas_Click(object? sender, EventArgs e)
        {
            MostrarModuloTablas();
        }

        private void btnModuloFacturacion_Click(object? sender, EventArgs e)
        {
            MostrarModuloFacturacion();
        }

        private void btnModuloSeguridad_Click(object? sender, EventArgs e)
        {
            MostrarModuloSeguridad();
        }

        private void btnModuloAyuda_Click(object? sender, EventArgs e)
        {
            MostrarModuloAyuda();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void timerReloj_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
