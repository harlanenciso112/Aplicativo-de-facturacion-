using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmAdminSeguridad : Form
    {
        private readonly UsuarioService _usuarioService;
        private readonly RolService _rolService;
        private System.Collections.Generic.List<Usuario> _usuarios = new System.Collections.Generic.List<Usuario>();
        private System.Collections.Generic.List<Rol> _roles = new System.Collections.Generic.List<Rol>();

        public frmAdminSeguridad()
        {
            InitializeComponent();
            _usuarioService = AppContainer.UsuarioService;
            _rolService = AppContainer.RolService;
        }

        private void frmAdminSeguridad_Load(object sender, EventArgs e)
        {
            CargarRoles();
            CargarUsuarios();
        }

        private void CargarRoles()
        {
            _roles = _rolService.ListarActivos();
        }

        private void CargarUsuarios()
        {
            try
            {
                colId.DataPropertyName = "id";
                colUsuario.DataPropertyName = "usuario";
                colRol.DataPropertyName = "rol_nombre";
                colEstado.DataPropertyName = "estado";
                colUltimoAcceso.DataPropertyName = "ultimo_acceso";
                dgvUsuarios.AutoGenerateColumns = false;

                _usuarios = _usuarioService.Listar();
                var lista = _usuarios
                    .Select(u => new
                    {
                        id = u.Id,
                        usuario = u.UsuarioLogin,
                        rol_nombre = u.RolNombre,
                        estado = u.Estado,
                        ultimo_acceso = u.UltimoAcceso
                    })
                    .ToList();

                dgvUsuarios.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevoUsuario_Click(object sender, EventArgs e)
        {
            using (Form formNuevo = new Form())
            {
                formNuevo.Text = "Nuevo Usuario";
                formNuevo.Size = new Size(380, 320);
                formNuevo.FormBorderStyle = FormBorderStyle.FixedDialog;
                formNuevo.StartPosition = FormStartPosition.CenterParent;
                formNuevo.MaximizeBox = false;
                formNuevo.MinimizeBox = false;

                var lblNombre = new Label { Text = "Nombre completo:", Location = new Point(20, 20), AutoSize = true };
                var txtNombre = new TextBox { Location = new Point(20, 40), Width = 330 };
                var lblUsuario = new Label { Text = "Usuario:", Location = new Point(20, 75), AutoSize = true };
                var txtUsuario = new TextBox { Location = new Point(20, 95), Width = 330 };
                var lblRol = new Label { Text = "Rol:", Location = new Point(20, 130), AutoSize = true };
                var cboRol = new ComboBox
                {
                    Location = new Point(20, 150),
                    Width = 330,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                foreach (var rol in _roles)
                {
                    cboRol.Items.Add(new RolItem(rol.Id, rol.Nombre));
                }

                if (cboRol.Items.Count > 0)
                {
                    cboRol.SelectedIndex = 0;
                }

                var lblPassword = new Label
                {
                    Text = "Contraseña inicial: se genera automáticamente (simple, no repetida)",
                    Location = new Point(20, 190),
                    AutoSize = true
                };

                var btnGuardar = new Button { Text = "Guardar", Location = new Point(165, 230), Width = 90, DialogResult = DialogResult.OK };
                var btnCancelar = new Button { Text = "Cancelar", Location = new Point(260, 230), Width = 90, DialogResult = DialogResult.Cancel };

                formNuevo.Controls.AddRange(new Control[]
                {
                    lblNombre, txtNombre, lblUsuario, txtUsuario, lblRol, cboRol, lblPassword, btnGuardar, btnCancelar
                });
                formNuevo.AcceptButton = btnGuardar;
                formNuevo.CancelButton = btnCancelar;

                if (formNuevo.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(txtUsuario.Text))
                    {
                        MessageBox.Show("Usuario es obligatorio.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cboRol.SelectedItem is not RolItem rolSeleccionado)
                    {
                        MessageBox.Show("Debe seleccionar un rol.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        var passwordGenerada = _usuarioService.GenerarPasswordSimpleNoRepetida();

                        _usuarioService.Crear(new Usuario
                        {
                            UsuarioLogin = txtUsuario.Text.Trim(),
                            Password = passwordGenerada,
                            Nombre = txtNombre.Text.Trim(),
                            RolId = rolSeleccionado.Id,
                            Activo = true
                        });

                        MessageBox.Show(
                            $"Usuario creado correctamente.\n\nUsuario: {txtUsuario.Text.Trim()}\nContraseña inicial: {passwordGenerada}",
                            "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarUsuarios();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al crear usuario: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEditarUsuario_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["colId"].Value);
            var usuarioActual = _usuarios.FirstOrDefault(x => x.Id == id);
            if (usuarioActual == null)
            {
                MessageBox.Show("No se pudo cargar el usuario seleccionado.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Form formEditar = new Form())
            {
                formEditar.Text = "Editar Usuario";
                formEditar.Size = new Size(380, 300);
                formEditar.FormBorderStyle = FormBorderStyle.FixedDialog;
                formEditar.StartPosition = FormStartPosition.CenterParent;
                formEditar.MaximizeBox = false;
                formEditar.MinimizeBox = false;

                var lblNombre = new Label { Text = "Nombre completo:", Location = new Point(20, 20), AutoSize = true };
                var txtNombre = new TextBox { Text = usuarioActual.Nombre, Location = new Point(20, 40), Width = 330 };
                var lblUsuario = new Label { Text = "Usuario:", Location = new Point(20, 75), AutoSize = true };
                var txtUsuario = new TextBox { Text = usuarioActual.UsuarioLogin, Location = new Point(20, 95), Width = 330 };
                var lblRol = new Label { Text = "Rol:", Location = new Point(20, 130), AutoSize = true };
                var cboRol = new ComboBox
                {
                    Location = new Point(20, 150),
                    Width = 330,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                foreach (var rol in _roles)
                {
                    cboRol.Items.Add(new RolItem(rol.Id, rol.Nombre));
                }

                for (var i = 0; i < cboRol.Items.Count; i++)
                {
                    if (cboRol.Items[i] is RolItem item && item.Id == usuarioActual.RolId)
                    {
                        cboRol.SelectedIndex = i;
                        break;
                    }
                }

                if (cboRol.SelectedIndex == -1 && cboRol.Items.Count > 0)
                {
                    cboRol.SelectedIndex = 0;
                }

                var chkActivo = new CheckBox
                {
                    Text = "Activo",
                    Location = new Point(20, 190),
                    Checked = usuarioActual.Activo,
                    AutoSize = true
                };

                var btnGuardar = new Button { Text = "Guardar", Location = new Point(165, 220), Width = 90, DialogResult = DialogResult.OK };
                var btnCancelar = new Button { Text = "Cancelar", Location = new Point(260, 220), Width = 90, DialogResult = DialogResult.Cancel };

                formEditar.Controls.AddRange(new Control[]
                {
                    lblNombre, txtNombre, lblUsuario, txtUsuario, lblRol, cboRol, chkActivo, btnGuardar, btnCancelar
                });
                formEditar.AcceptButton = btnGuardar;
                formEditar.CancelButton = btnCancelar;

                if (formEditar.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(txtUsuario.Text))
                    {
                        MessageBox.Show("El usuario es obligatorio.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cboRol.SelectedItem is not RolItem rolSeleccionado)
                    {
                        MessageBox.Show("Debe seleccionar un rol.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        _usuarioService.Actualizar(new Usuario
                        {
                            Id = id,
                            UsuarioLogin = txtUsuario.Text.Trim(),
                            Nombre = txtNombre.Text.Trim(),
                            RolId = rolSeleccionado.Id,
                            Activo = chkActivo.Checked
                        });
                        MessageBox.Show("Usuario actualizado correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarUsuarios();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var result = MessageBox.Show("¿Está seguro de eliminar este usuario?",
                "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["colId"].Value);
                    _usuarioService.Eliminar(id);
                    CargarUsuarios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCambiarPassword_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario para cambiar la contraseña.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["colId"].Value);
            string usuarioNombre = dgvUsuarios.CurrentRow.Cells["colUsuario"].Value?.ToString() ?? "";

            using (Form formPass = new Form())
            {
                formPass.Text = "Cambiar Contraseña - " + usuarioNombre;
                formPass.Size = new Size(350, 230);
                formPass.FormBorderStyle = FormBorderStyle.FixedDialog;
                formPass.StartPosition = FormStartPosition.CenterParent;
                formPass.MaximizeBox = false;
                formPass.MinimizeBox = false;

                var lblNueva = new Label { Text = "Nueva contraseña:", Location = new Point(20, 20), AutoSize = true };
                var txtNueva = new TextBox { Location = new Point(20, 42), Width = 295, UseSystemPasswordChar = true };
                var lblConfirmar = new Label { Text = "Confirmar contraseña:", Location = new Point(20, 75), AutoSize = true };
                var txtConfirmar = new TextBox { Location = new Point(20, 97), Width = 295, UseSystemPasswordChar = true };
                var btnGuardar = new Button { Text = "Guardar", Location = new Point(130, 145), Width = 90, DialogResult = DialogResult.OK };
                var btnCancelar = new Button { Text = "Cancelar", Location = new Point(225, 145), Width = 90, DialogResult = DialogResult.Cancel };

                formPass.Controls.AddRange(new Control[] { lblNueva, txtNueva, lblConfirmar, txtConfirmar, btnGuardar, btnCancelar });
                formPass.AcceptButton = btnGuardar;
                formPass.CancelButton = btnCancelar;

                if (formPass.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(txtNueva.Text))
                    {
                        MessageBox.Show("La contraseña no puede estar vacía.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (txtNueva.Text != txtConfirmar.Text)
                    {
                        MessageBox.Show("Las contraseñas no coinciden.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    try
                    {
                        _usuarioService.CambiarPassword(id, txtNueva.Text);
                        MessageBox.Show("Contraseña actualizada correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar contraseña: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnPermisos_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo de permisos no implementado.", "Información",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private class RolItem
        {
            public int Id { get; }
            public string Nombre { get; }

            public RolItem(int id, string nombre)
            {
                Id = id;
                Nombre = nombre;
            }

            public override string ToString()
            {
                return Nombre;
            }
        }
    }
}
