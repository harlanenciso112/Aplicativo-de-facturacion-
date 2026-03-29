using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Pantallas_Sistema_facturacion.Application.Services;
using Pantallas_Sistema_facturacion.Composition;
using Pantallas_Sistema_facturacion.Domain.Entities;

namespace Pantallas_Sistema_facturacion
{
    public partial class frmFacturas : Form
    {
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int IdFactura { get; set; }

        private readonly FacturaService _facturaService;
        private readonly ClienteService _clienteService;
        private readonly ProductoService _productoService;

        private readonly List<Cliente> _clientes = new List<Cliente>();
        private readonly List<Producto> _productos = new List<Producto>();

        public frmFacturas()
        {
            InitializeComponent();
            _facturaService = AppContainer.FacturaService;
            _clienteService = AppContainer.ClienteService;
            _productoService = AppContainer.ProductoService;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {
                    if (cboCliente.SelectedItem is not ComboItem clienteSeleccionado)
                    {
                        MessageBox.Show("Seleccione un cliente válido.", "Validación",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var detalles = ObtenerDetalles();
                    var total = detalles.Sum(d => d.Subtotal);

                    var factura = new Factura
                    {
                        Id = IdFactura,
                        Numero = txtNumero.Text.Trim(),
                        Fecha = dtpFecha.Value,
                        ClienteId = clienteSeleccionado.Id,
                        Total = total,
                        Estado = "ACTIVA",
                        Detalles = detalles
                    };

                    _facturaService.Guardar(factura);

                    MessageBox.Show(
                        IdFactura == 0 ? "Factura guardada correctamente." : "Factura actualizada correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar factura: " + ex.Message, "Error",
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

            if (cboCliente.SelectedIndex == -1)
            {
                errorProvider1.SetError(cboCliente, "Seleccione un cliente");
                esValido = false;
            }
            else
            {
                errorProvider1.SetError(cboCliente, "");
            }

            if (dgvDetalle.Rows.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un producto", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                esValido = false;
            }

            return esValido;
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (cboProducto.SelectedItem is ComboItem productoSeleccionado && int.TryParse(txtCantidad.Text, out var cantidad) && cantidad > 0)
            {
                var producto = _productos.FirstOrDefault(p => p.Id == productoSeleccionado.Id);
                if (producto == null)
                {
                    MessageBox.Show("No se pudo obtener el producto seleccionado.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (producto.Stock < cantidad)
                {
                    MessageBox.Show($"Stock insuficiente. Disponible: {producto.Stock}", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var precio = producto.Precio;
                var subtotal = precio * cantidad;

                dgvDetalle.Rows.Add(
                    producto.Nombre,
                    cantidad,
                    precio.ToString("N2", CultureInfo.InvariantCulture),
                    subtotal.ToString("N2", CultureInfo.InvariantCulture));

                dgvDetalle.Rows[dgvDetalle.Rows.Count - 1].Tag = producto.Id;
                CalcularTotal();
                cboProducto.SelectedIndex = -1;
                txtCantidad.Clear();
            }
            else
            {
                MessageBox.Show("Seleccione un producto e ingrese una cantidad válida.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnQuitarProducto_Click(object sender, EventArgs e)
        {
            if (dgvDetalle.SelectedRows.Count > 0)
            {
                dgvDetalle.Rows.Remove(dgvDetalle.SelectedRows[0]);
                CalcularTotal();
            }
        }

        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvDetalle.Rows)
            {
                var cell = row.Cells["colSubtotal"];
                if (cell != null && cell.Value != null)
                {
                    var texto = cell.Value.ToString() ?? "0";
                    if (decimal.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out var subtotal))
                    {
                        total += subtotal;
                    }
                }
            }

            lblTotalValor.Text = "$" + total.ToString("N2", CultureInfo.InvariantCulture);
        }

        private void frmFacturas_Load(object sender, EventArgs e)
        {
            CargarCombos();

            if (IdFactura > 0)
            {
                CargarFacturaExistente();
            }
            else
            {
                txtNumero.Text = _facturaService.ObtenerSiguienteNumero();
                dtpFecha.Value = DateTime.Now;
                lblTitulo.Text = "🧾 Nueva Factura";
            }
        }

        private void CargarCombos()
        {
            _clientes.Clear();
            _clientes.AddRange(_clienteService.Listar());

            _productos.Clear();
            _productos.AddRange(_productoService.Listar());

            cboCliente.Items.Clear();
            foreach (var cliente in _clientes)
            {
                cboCliente.Items.Add(new ComboItem(cliente.Id, cliente.Nombre));
            }

            cboProducto.Items.Clear();
            foreach (var producto in _productos)
            {
                cboProducto.Items.Add(new ComboItem(producto.Id, $"{producto.Nombre} (Stock: {producto.Stock})"));
            }
        }

        private void CargarFacturaExistente()
        {
            var factura = _facturaService.ObtenerPorId(IdFactura);
            if (factura == null)
            {
                MessageBox.Show("No se encontró la factura seleccionada.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            txtNumero.Text = factura.Numero;
            dtpFecha.Value = factura.Fecha;
            lblTitulo.Text = " Editar Factura";

            for (var i = 0; i < cboCliente.Items.Count; i++)
            {
                if (cboCliente.Items[i] is ComboItem item && item.Id == factura.ClienteId)
                {
                    cboCliente.SelectedIndex = i;
                    break;
                }
            }

            dgvDetalle.Rows.Clear();
            foreach (var det in factura.Detalles)
            {
                dgvDetalle.Rows.Add(
                    det.ProductoNombre,
                    det.Cantidad,
                    det.PrecioUnitario.ToString("N2", CultureInfo.InvariantCulture),
                    det.Subtotal.ToString("N2", CultureInfo.InvariantCulture));

                dgvDetalle.Rows[dgvDetalle.Rows.Count - 1].Tag = det.ProductoId;
            }

            CalcularTotal();
        }

        private List<FacturaDetalle> ObtenerDetalles()
        {
            var detalles = new List<FacturaDetalle>();
            foreach (DataGridViewRow row in dgvDetalle.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                var productoId = row.Tag is int id ? id : 0;
                var cantidadTexto = row.Cells["colCantidad"].Value?.ToString() ?? "0";
                var precioTexto = row.Cells["colPrecio"].Value?.ToString() ?? "0";

                if (productoId == 0
                    || !int.TryParse(cantidadTexto, out var cantidad)
                    || !decimal.TryParse(precioTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out var precio))
                {
                    continue;
                }

                detalles.Add(new FacturaDetalle
                {
                    ProductoId = productoId,
                    Cantidad = cantidad,
                    PrecioUnitario = precio
                });
            }

            return detalles;
        }

        private class ComboItem
        {
            public int Id { get; }
            public string Texto { get; }

            public ComboItem(int id, string texto)
            {
                Id = id;
                Texto = texto;
            }

            public override string ToString()
            {
                return Texto;
            }
        }
    }
}
